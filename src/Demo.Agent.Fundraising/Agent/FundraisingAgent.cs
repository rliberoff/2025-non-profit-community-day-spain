using Azure.AI.Agents.Persistent;
using Azure.Identity;

using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Demo.Agent.Fundraising.Agent;

/// <summary>
/// Main agent class for fundraising campaign management.
/// Integrates with Microsoft Foundry (Azure AI Foundry) and Azure OpenAI for conversational AI capabilities.
/// </summary>
public class FundraisingAgent
{
    private readonly Tools tools;
    private readonly ILogger<FundraisingAgent> logger;
    private readonly IConfiguration configuration;
    private PersistentAgentsClient? persistentAgentsClient;
    private AIAgent? agent;
    private string? agentId;

    public FundraisingAgent(
        Tools tools,
        ILogger<FundraisingAgent> logger,
        IConfiguration configuration)
    {
        this.tools = tools;
        this.logger = logger;
        this.configuration = configuration;
    }

    /// <summary>
    /// Initializes the Microsoft Foundry agent with function tools.
    /// </summary>
    public async Task InitializeAsync()
    {
        logger.LogInformation("Initializing Fundraising Agent...");

        try
        {
            // Get Azure configuration
            var projectEndpoint = configuration["AzureAI:ProjectEndpoint"];
            var modelDeployment = configuration["AzureAI:ModelDeploymentName"] ?? "gpt-4o-mini";

            if (string.IsNullOrEmpty(projectEndpoint))
            {
                logger.LogWarning("Azure AI Project endpoint not configured. Agent will run in demo mode without cloud persistence.");
                return;
            }

            // Create persistent agents client for Microsoft Foundry
            persistentAgentsClient = new PersistentAgentsClient(
                projectEndpoint,
                new DefaultAzureCredential()
            );

            // Create or get existing agent
            var agentName = "Agent Fundraising";
            var instructions = Instructions.GetCompleteInstructions();

            // Create the agent with function tools
            var allTools = new List<AITool>
            {
                // Campaign management tools
                AIFunctionFactory.Create(
                    (string name, decimal goalEuros) => tools.CreateCampaign(name, goalEuros),
                    name: "CreateCampaign",
                    description: "Crea una nueva campaña de recaudación de fondos con un nombre y una meta financiera en euros"
                ),

                AIFunctionFactory.Create(
                    () => tools.ListCampaigns(),
                    name: "ListCampaigns",
                    description: "Lista todas las campañas de recaudación existentes"
                ),

                AIFunctionFactory.Create(
                    (string campaignId) => tools.GetCampaign(campaignId),
                    name: "GetCampaign",
                    description: "Obtiene los detalles de una campaña específica incluyendo sus tareas"
                ),

                // Task management tools
                AIFunctionFactory.Create(
                    (string campaignId, string description, string? assignedTo = null) =>
                        tools.AddTask(campaignId, description, assignedTo),
                    name: "AddTask",
                    description: "Agrega una nueva tarea a una campaña"
                ),

                AIFunctionFactory.Create(
                    (string taskId, string newColumn) => tools.MoveTask(taskId, newColumn),
                    name: "MoveTask",
                    description: "Mueve una tarea a una columna diferente del tablero Kanban (por_hacer, en_progreso, completado)"
                ),

                AIFunctionFactory.Create(
                    (string taskId, string userId) => tools.AssignTask(taskId, userId),
                    name: "AssignTask",
                    description: "Asigna una tarea a un usuario del equipo"
                ),

                AIFunctionFactory.Create(
                    (string campaignId) => tools.GetKanbanBoard(campaignId),
                    name: "GetKanbanBoard",
                    description: "Obtiene el tablero Kanban de una campaña con tareas agrupadas por columna"
                ),

                AIFunctionFactory.Create(
                    () => tools.ListUsers(),
                    name: "ListUsers",
                    description: "Lista todos los usuarios del equipo disponibles para asignación de tareas"
                ),

                // Comment tools
                AIFunctionFactory.Create(
                    (string taskId, string text) => tools.AddComment(taskId, text),
                    name: "AddComment",
                    description: "Agrega un comentario a una tarea"
                ),

                AIFunctionFactory.Create(
                    (string taskId) => tools.ListComments(taskId),
                    name: "ListComments",
                    description: "Lista todos los comentarios de una tarea en orden cronológico"
                ),

                // Thank-you message generation (User Story 4)
                AIFunctionFactory.Create(
                    (string donorName, string campaignName, decimal? amountEuros = null) =>
                        tools.GenerateThankYouMessage(donorName, campaignName, amountEuros),
                    name: "GenerateThankYouMessage",
                    description: "Genera un mensaje de agradecimiento personalizado para un donante usando IA"
                )
            };

            // Create the agent in Azure AI Foundry
            var createAgentResponse = await persistentAgentsClient.Administration.CreateAgentAsync(
                model: modelDeployment,
                name: agentName,
                description: "Agente de recaudación de fondos para ONG que gestiona campañas, tareas y mensajes de agradecimiento",
                instructions: instructions,
                temperature: 0.7f,
                topP: 0.9f
            );

            var persistentAgent = createAgentResponse.Value;
            agentId = persistentAgent.Id;

            // Get IChatClient for the agent
            var chatClient = persistentAgentsClient.AsIChatClient(agentId);

            // Wrap in ChatClientAgent with function tools
            agent = new ChatClientAgent(
                chatClient,
                options: new ChatClientAgentOptions()
                {
                    Id = agentId,
                    Name = agentName,
                    Instructions = instructions,
                    ChatOptions = new ChatOptions
                    {
                        Tools = allTools
                    }
                }
            );

            logger.LogInformation("Agent initialized successfully with ID: {AgentId}", agentId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to initialize agent");
            // Don't throw - allow running without Foundry for local development
        }
    }

    /// <summary>
    /// Processes a user message and returns the agent's response.
    /// </summary>
    public async Task<string> ProcessMessageAsync(string userMessage)
    {
        if (agent == null)
        {
            logger.LogWarning("Agent not initialized. Running in demo mode.");
            return "Agent no inicializado. Configura las credenciales de Azure AI Foundry para habilitar la funcionalidad completa.";
        }

        logger.LogInformation("Processing message: {Message}", userMessage);

        try
        {
            // Create a new thread for this conversation
            var thread = agent.GetNewThread();

            // Run the agent with streaming (best practice)
            var responseBuilder = new System.Text.StringBuilder();

            await foreach (var update in agent.RunStreamingAsync(userMessage, thread))
            {
                if (!string.IsNullOrEmpty(update.Text))
                {
                    responseBuilder.Append(update.Text);
                }
            }

            var response = responseBuilder.ToString();
            logger.LogInformation("Agent response: {Response}", response);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message");
            return $"Error al procesar el mensaje: {ex.Message}";
        }
    }

    /// <summary>
    /// Processes a message with conversation context (multi-turn).
    /// </summary>
    public async Task<string> ProcessConversationAsync(string userMessage, AgentThread? thread = null)
    {
        if (agent == null)
        {
            logger.LogWarning("Agent not initialized. Running in demo mode.");
            return "Agent no inicializado. Configura las credenciales de Azure AI Foundry para habilitar la funcionalidad completa.";
        }

        logger.LogInformation("Processing conversation message: {Message}", userMessage);

        try
        {
            // Use provided thread or create new one
            thread ??= agent.GetNewThread();

            // Run the agent with streaming
            var responseBuilder = new System.Text.StringBuilder();

            await foreach (var update in agent.RunStreamingAsync(userMessage, thread))
            {
                if (!string.IsNullOrEmpty(update.Text))
                {
                    responseBuilder.Append(update.Text);
                }
            }

            var response = responseBuilder.ToString();
            logger.LogInformation("Agent conversation response: {Response}", response);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing conversation message");
            return $"Error al procesar el mensaje: {ex.Message}";
        }
    }

    /// <summary>
    /// Cleans up resources when done.
    /// </summary>
    public async Task DisposeAsync()
    {
        if (persistentAgentsClient != null && !string.IsNullOrEmpty(agentId))
        {
            try
            {
                await persistentAgentsClient.Administration.DeleteAgentAsync(agentId);
                logger.LogInformation("Agent cleaned up successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error cleaning up agent");
            }
        }
    }
}
