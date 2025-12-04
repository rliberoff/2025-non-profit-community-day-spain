using System.Diagnostics;

using Demo.Agent.Fundraising.Models;
using Demo.Agent.Fundraising.Validators;

namespace Demo.Agent.Fundraising.Agent;

/// <summary>
/// Function tools available for the fundraising agent.
/// </summary>
public class Tools
{
    private static readonly ActivitySource ActivitySource = new("Demo.Agent.Fundraising", "1.0.0");

    // Rate limiting for AI message generation (10 requests per minute)
    private static readonly Queue<DateTime> _aiRequestTimestamps = new();
    private static readonly int _maxRequestsPerMinute = 10;
    private static readonly object _rateLimitLock = new();

    private readonly AgentState state;
    private readonly ILogger<Tools> logger;

    public Tools(AgentState state, ILogger<Tools> logger)
    {
        this.state = state;
        this.logger = logger;
    }

    /// <summary>
    /// Checks if the rate limit for AI requests has been exceeded.
    /// </summary>
    /// <returns>True if request is allowed, false if rate limit exceeded.</returns>
    private static bool CheckRateLimit()
    {
        lock (_rateLimitLock)
        {
            var now = DateTime.UtcNow;
            var oneMinuteAgo = now.AddMinutes(-1);

            // Remove timestamps older than 1 minute
            while (_aiRequestTimestamps.Count > 0 && _aiRequestTimestamps.Peek() < oneMinuteAgo)
            {
                _aiRequestTimestamps.Dequeue();
            }

            // Check if limit exceeded
            if (_aiRequestTimestamps.Count >= _maxRequestsPerMinute)
            {
                return false;
            }

            // Add current timestamp
            _aiRequestTimestamps.Enqueue(now);
            return true;
        }
    }

    // ==================== USER STORY 1: CAMPAIGN MANAGEMENT ====================

    /// <summary>
    /// Creates a new fundraising campaign.
    /// </summary>
    /// <param name="name">Campaign name.</param>
    /// <param name="goalEuros">Financial goal in euros.</param>
    /// <returns>Information about the created campaign.</returns>
    public object CreateCampaign(string name, decimal goalEuros)
    {
        using var activity = ActivitySource.StartActivity("CreateCampaign");
        activity?.SetTag("campaign.name", name);
        activity?.SetTag("campaign.goal_euros", goalEuros);

        logger.LogInformation("Creating campaign: {Name}, Goal: {Goal} euros", name, goalEuros);

        // Validate data
        var error = CampaignValidator.ValidateCampaign(name, goalEuros);
        if (error != null)
        {
            activity?.SetTag("validation.failed", true);
            activity?.SetTag("error.message", error);
            logger.LogWarning("Validation error when creating campaign: {Error}", error);
            return new { error, exito = false };
        }

        // Create campaign
        var id = Guid.NewGuid().ToString();
        var campaign = new Campaign(
            Id: id,
            Name: name,
            GoalEuros: goalEuros,
            CreatedAt: DateTime.UtcNow,
            TaskIds: []
        );

        state.Campaigns[id] = campaign;

        activity?.SetTag("campaign.id", id);
        activity?.SetTag("operation.success", true);
        logger.LogInformation("Campaign created successfully: {Id}", id);

        return new
        {
            exito = true,
            id = campaign.Id,
            nombre = campaign.Name,
            meta_euros = campaign.GoalEuros,
            mensaje = $"Campaña '{name}' creada exitosamente con meta de {goalEuros} euros."
        };
    }

    /// <summary>
    /// Lists all existing fundraising campaigns.
    /// </summary>
    /// <returns>List of campaigns with basic information.</returns>
    public object ListCampaigns()
    {
        logger.LogInformation("Listing all campaigns");

        var campaigns = state.Campaigns.Values
            .Select(c => new
            {
                id = c.Id,
                nombre = c.Name,
                meta_euros = c.GoalEuros,
                num_tareas = c.TaskIds?.Count ?? 0,
                fecha_creacion = c.CreatedAt,
            })
            .OrderByDescending(c => c.fecha_creacion)
            .ToList();

        logger.LogInformation("Found {Count} campaigns", campaigns.Count);

        return new
        {
            total = campaigns.Count,
            campañas = campaigns
        };
    }

    /// <summary>
    /// Gets the details of a specific campaign including its tasks.
    /// </summary>
    /// <param name="campaignId">Campaign ID.</param>
    /// <returns>Complete campaign details.</returns>
    public object GetCampaign(string campaignId)
    {
        logger.LogInformation("Getting campaign: {CampaignId}", campaignId);

        var validationError = CampaignValidator.ValidateCampaignExists(campaignId, state);
        if (validationError != null)
        {
            logger.LogWarning("Error getting campaign: {Error}", validationError);
            return new { error = validationError, exito = false };
        }

        var campaign = state.Campaigns[campaignId];

        var tasks = new List<object>();
        if (campaign.TaskIds != null)
        {
            tasks = [.. campaign.TaskIds
                .Where(state.Tasks.ContainsKey)
                .Select(tid =>
                {
                    var t = state.Tasks[tid];
                    return new
                    {
                        id = t.Id,
                        descripcion = t.Description,
                        columna = t.Column.ToString().ToLowerInvariant(),
                        asignado_a = t.AssignedTo != null && state.Users.ContainsKey(t.AssignedTo)
                            ? state.Users[t.AssignedTo].Name
                            : null,
                        num_comentarios = t.CommentIds?.Count ?? 0
                    };
                })
                .Cast<object>()];
        }

        return new
        {
            exito = true,
            id = campaign.Id,
            nombre = campaign.Name,
            meta_euros = campaign.GoalEuros,
            fecha_creacion = campaign.CreatedAt,
            tareas = tasks
        };
    }

    // ==================== USER STORY 2: TASK & KANBAN MANAGEMENT ====================

    /// <summary>
    /// Adds a new task to a campaign.
    /// </summary>
    /// <param name="campaignId">Campaign ID.</param>
    /// <param name="description">Task description.</param>
    /// <param name="assignedTo">ID of assigned user (optional).</param>
    /// <returns>Information about the created task.</returns>
    public object AddTask(string campaignId, string description, string? assignedTo = null)
    {
        logger.LogInformation("Adding task to campaign {CampaignId}: {Description}", campaignId, description);

        var error = TaskValidator.ValidateTask(description, campaignId, assignedTo, state);
        if (error != null)
        {
            logger.LogWarning("Validation error when adding task: {Error}", error);
            return new { error, exito = false };
        }

        var id = Guid.NewGuid().ToString();
        var task = new CampaignTask(
            Id: id,
            CampaignId: campaignId,
            Description: description,
            Column: KanbanColumn.ToDo,
            AssignedTo: assignedTo,
            CommentIds: [],
            CreatedAt: DateTime.UtcNow
        );

        state.Tasks[id] = task;

        // Add task to campaign
        var campaign = state.Campaigns[campaignId];
        var updatedTasks = (campaign.TaskIds ?? new List<string>()).ToList();
        updatedTasks.Add(id);
        state.Campaigns[campaignId] = campaign with { TaskIds = updatedTasks };

        var userName = assignedTo != null && state.Users.ContainsKey(assignedTo)
            ? state.Users[assignedTo].Name
            : "sin asignar";

        logger.LogInformation("Task created successfully: {Id}, Assigned to: {User}", id, userName);

        return new
        {
            exito = true,
            id = task.Id,
            descripcion = task.Description,
            columna = "por_hacer",
            asignado_a = userName,
            mensaje = $"Tarea '{description}' agregada a la campaña en 'Por Hacer'."
        };
    }

    /// <summary>
    /// Moves a task to a different Kanban board column.
    /// </summary>
    /// <param name="taskId">Task ID.</param>
    /// <param name="newColumn">Target column (por_hacer, en_progreso, completado).</param>
    /// <returns>Information about the updated task.</returns>
    public object MoveTask(string taskId, string newColumn)
    {
        logger.LogInformation("Moving task {TaskId} to {NewColumn}", taskId, newColumn);

        // Parse column
        KanbanColumn targetColumn;
        try
        {
            targetColumn = newColumn.ToLowerInvariant() switch
            {
                "por_hacer" or "porhacer" => KanbanColumn.ToDo,
                "en_progreso" or "enprogreso" => KanbanColumn.InProgress,
                "completado" => KanbanColumn.Done,
                _ => throw new ArgumentException($"Columna inválida: {newColumn}. Use 'por_hacer', 'en_progreso' o 'completado'.")
            };
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Error parsing column: {Error}", ex.Message);
            return new { error = ex.Message, exito = false };
        }

        var error = TaskValidator.ValidateTaskMove(taskId, targetColumn, state);
        if (error != null)
        {
            logger.LogWarning("Validation error when moving task: {Error}", error);
            return new { error, exito = false };
        }

        var task = state.Tasks[taskId];
        state.Tasks[taskId] = task with { Column = targetColumn };

        logger.LogInformation("Task {TaskId} moved successfully to {Column}", taskId, targetColumn);

        return new
        {
            exito = true,
            id = task.Id,
            descripcion = task.Description,
            columna = newColumn.ToLowerInvariant(),
            mensaje = $"Tarea movida a '{newColumn}'."
        };
    }

    /// <summary>
    /// Assigns a task to a team user.
    /// </summary>
    /// <param name="taskId">Task ID.</param>
    /// <param name="userId">ID of the user to assign.</param>
    /// <returns>Information about the assignment.</returns>
    public object AssignTask(string taskId, string userId)
    {
        logger.LogInformation("Assigning task {TaskId} to user {UserId}", taskId, userId);

        var errorTask = TaskValidator.ValidateTaskExists(taskId, state);
        if (errorTask != null)
        {
            logger.LogWarning("Error assigning task: {Error}", errorTask);
            return new { error = errorTask, exito = false };
        }

        if (!state.Users.ContainsKey(userId))
        {
            var error = $"Usuario '{userId}' no encontrado. Usuarios válidos: {string.Join(", ", state.Users.Keys)}";
            logger.LogWarning("Error assigning task: {Error}", error);
            return new { error, exito = false };
        }

        var task = state.Tasks[taskId];
        var user = state.Users[userId];

        state.Tasks[taskId] = task with { AssignedTo = userId };

        logger.LogInformation("Task {TaskId} assigned to {User}", taskId, user.Name);

        return new
        {
            exito = true,
            id = task.Id,
            descripcion = task.Description,
            asignado_a = user.Name,
            mensaje = $"Tarea asignada a {user.Name}."
        };
    }

    /// <summary>
    /// Gets the Kanban board of a campaign with tasks grouped by column.
    /// </summary>
    /// <param name="campaignId">Campaign ID.</param>
    /// <returns>Kanban board organized by columns.</returns>
    public object GetKanbanBoard(string campaignId)
    {
        logger.LogInformation("Getting Kanban board for campaign: {CampaignId}", campaignId);

        var validationError = CampaignValidator.ValidateCampaignExists(campaignId, state);
        if (validationError != null)
        {
            logger.LogWarning("Error getting board: {Error}", validationError);
            return new { error = validationError, exito = false };
        }

        var campaign = state.Campaigns[campaignId];
        var taskIds = campaign.TaskIds ?? new List<string>();

        var tasks = taskIds
            .Where(tid => state.Tasks.ContainsKey(tid))
            .Select(tid => state.Tasks[tid])
            .ToList();

        var toDo = tasks.Where(t => t.Column == KanbanColumn.ToDo)
            .Select(FormatTask)
            .ToList();

        var inProgress = tasks.Where(t => t.Column == KanbanColumn.InProgress)
            .Select(FormatTask)
            .ToList();

        var done = tasks.Where(t => t.Column == KanbanColumn.Done)
            .Select(FormatTask)
            .ToList();

        return new
        {
            exito = true,
            campaña = campaign.Name,
            tablero = new
            {
                por_hacer = toDo,
                en_progreso = inProgress,
                completado = done
            },
            resumen = new
            {
                total_tareas = tasks.Count,
                por_hacer = toDo.Count,
                en_progreso = inProgress.Count,
                completado = done.Count
            }
        };
    }

    /// <summary>
    /// Lists all team users available for assignment.
    /// </summary>
    /// <returns>List of users.</returns>
    public object ListUsers()
    {
        logger.LogInformation("Listing all users");

        var users = state.Users.Values
            .Select(u => new
            {
                id = u.Id,
                nombre = u.Name,
                rol = u.Role.ToString().ToLowerInvariant()
            })
            .ToList();

        return new
        {
            total = users.Count,
            usuarios = users
        };
    }

    // ==================== USER STORY 3: COMMENTS ====================

    /// <summary>
    /// Adds a comment to a task.
    /// </summary>
    /// <param name="taskId">Task ID.</param>
    /// <param name="text">Comment content.</param>
    /// <returns>Information about the created comment.</returns>
    public object AddComment(string taskId, string text)
    {
        logger.LogInformation("Adding comment to task {TaskId}", taskId);

        var error = CommentValidator.ValidateComment(text, taskId, state);
        if (error != null)
        {
            logger.LogWarning("Validation error when adding comment: {Error}", error);
            return new { error, exito = false };
        }

        var id = Guid.NewGuid().ToString();
        var comment = new TaskComment(
            Id: id,
            TaskId: taskId,
            Text: text,
            CreatedAt: DateTime.UtcNow
        );

        state.Comments[id] = comment;

        // Add comment to task
        var task = state.Tasks[taskId];
        var updatedComments = (task.CommentIds ?? new List<string>()).ToList();
        updatedComments.Add(id);
        state.Tasks[taskId] = task with { CommentIds = updatedComments };

        logger.LogInformation("Comment added successfully: {Id}", id);

        return new
        {
            exito = true,
            id = comment.Id,
            texto = comment.Text,
            fecha_creacion = comment.CreatedAt,
            mensaje = "Comentario agregado exitosamente."
        };
    }

    /// <summary>
    /// Lists all comments of a task in chronological order.
    /// </summary>
    /// <param name="taskId">Task ID.</param>
    /// <returns>List of comments.</returns>
    public object ListComments(string taskId)
    {
        logger.LogInformation("Listing comments for task: {TaskId}", taskId);

        var taskValidation = TaskValidator.ValidateTaskExists(taskId, state);
        if (taskValidation != null)
        {
            logger.LogWarning("Error listing comments: {Error}", taskValidation);
            return new { error = taskValidation, exito = false };
        }

        var task = state.Tasks[taskId];
        var commentIds = task.CommentIds ?? new List<string>();

        var comments = commentIds
            .Where(cid => state.Comments.ContainsKey(cid))
            .Select(cid => state.Comments[cid])
            .OrderBy(c => c.CreatedAt)
            .Select(c => new
            {
                id = c.Id,
                texto = c.Text,
                fecha_creacion = c.CreatedAt
            })
            .ToList();

        return new
        {
            exito = true,
            tarea_descripcion = task.Description,
            total_comentarios = comments.Count,
            comentarios = comments
        };
    }

    // ==================== USER STORY 4: AI THANK-YOU MESSAGE GENERATION ====================

    /// <summary>
    /// Generates a personalized thank-you message for a donor using AI.
    /// </summary>
    /// <param name="donorName">Name of the donor.</param>
    /// <param name="campaignName">Name of the campaign they donated to.</param>
    /// <param name="amountEuros">Optional donation amount in euros.</param>
    /// <returns>Generated thank-you message.</returns>
    public async Task<object> GenerateThankYouMessage(string donorName, string campaignName, decimal? amountEuros = null)
    {
        using var activity = ActivitySource.StartActivity("GenerateThankYouMessage");
        activity?.SetTag("donor.name", donorName);
        activity?.SetTag("campaign.name", campaignName);
        activity?.SetTag("amount.euros", amountEuros);

        logger.LogInformation("Generating thank-you message for donor: {DonorName}, Campaign: {CampaignName}, Amount: {Amount}", donorName, campaignName, amountEuros);

        // Check rate limit
        if (!CheckRateLimit())
        {
            activity?.SetTag("rate_limit.exceeded", true);
            logger.LogWarning("Rate limit exceeded for AI message generation");
            return new
            {
                exito = false,
                error = "Se ha excedido el límite de solicitudes para generar mensajes. Por favor, espera un momento e intenta de nuevo."
            };
        }

        try
        {
            // Generate message using template-based approach
            // NOTE: Future enhancement - integrate with Azure OpenAI for true AI generation
            // Current implementation uses a high-quality template that follows Instructions.ThankYouInstructions guidelines
            logger.LogInformation("Generating thank-you message using template-based approach");

            var generatedMessage = GenerateTemplateBasedMessage(donorName, campaignName, amountEuros);

            var thankYouMessage = new ThankYouMessage(
                DonorName: donorName,
                CampaignName: campaignName,
                GeneratedText: generatedMessage,
                AmountEuros: amountEuros
            );

            logger.LogInformation("Thank-you message generated successfully for {DonorName}", donorName);

            return new
            {
                exito = true,
                nombre_donante = thankYouMessage.DonorName,
                campana = thankYouMessage.CampaignName,
                monto_euros = thankYouMessage.AmountEuros,
                mensaje = thankYouMessage.GeneratedText
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating thank-you message");
            return new
            {
                exito = false,
                error = $"Error al generar mensaje de agradecimiento: {ex.Message}"
            };
        }
    }

    // ==================== HELPER METHODS ====================

    private object FormatTask(CampaignTask task)
    {
        return new
        {
            id = task.Id,
            descripcion = task.Description,
            asignado_a = task.AssignedTo != null && state.Users.ContainsKey(task.AssignedTo)
                ? state.Users[task.AssignedTo].Name
                : null,
            num_comentarios = task.CommentIds?.Count ?? 0,
            fecha_creacion = task.CreatedAt
        };
    }

    /// <summary>
    /// Generates a template-based thank-you message as fallback.
    /// </summary>
    private static string GenerateTemplateBasedMessage(string donorName, string campaignName, decimal? amountEuros)
    {
        var message = $"Querido/a {donorName},\n\n";

        if (amountEuros.HasValue)
        {
            message += $"Queremos expresar nuestro más sincero agradecimiento por tu generosa contribución de {amountEuros.Value:N2} euros a nuestra campaña '{campaignName}'. ";
        }
        else
        {
            message += $"Queremos expresar nuestro más sincero agradecimiento por tu generoso apoyo a nuestra campaña '{campaignName}'. ";
        }

        message += "Tu solidaridad hace posible que podamos continuar con nuestra misión de ayudar a quienes más lo necesitan. ";
        message += "Gracias a personas como tú, podemos transformar vidas y construir una comunidad más justa y esperanzadora.\n\n";
        message += "Con profundo agradecimiento,\n";
        message += "El equipo de [Organización]";

        return message;
    }
}
