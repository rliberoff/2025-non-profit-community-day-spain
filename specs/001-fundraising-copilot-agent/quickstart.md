# Quickstart: Fundraising Campaign Management Agent

**Feature**: M365 Copilot Agent for Non-Profit Fundraising Management  
**Technology**: Microsoft Agent Framework (C# / .NET 10) + Azure AI Foundry  
**Access Method**: M365 Copilot Chat (primary interface)  
**Estimated Setup Time**: 15-20 minutes

## Prerequisites

### Required

- .NET 10 SDK
- Visual Studio 2026 (18.0+) or Visual Studio Code with C# Dev Kit
- Azure CLI (`az`) with active subscription
- Azure Subscription ID
- Microsoft 365 account with Copilot license (for testing)
- Git

### Optional (for full deployment)

- Terraform 1.14+
- Azure Developer CLI (`azd`)
- Docker Desktop (for containerization)
- Node.js 18+ (for React frontend)

---

## Quick Start (Local Development)

### Step 1: Clone and Setup

```powershell
# Navigate to project root
cd e:\repos\personal\2025-non-profit-community-day-spain

# Restore NuGet packages
cd src
dotnet restore

# Build the solution
dotnet build
```

### Step 2: Configure Environment

Edit `src/AgenteRecaudacion/appsettings.json`:

```json
{
  "AzureAI": {
    "ProjectEndpoint": "https://<your-project>.services.ai.azure.com/api/projects/<project-id>",
    "ModelDeploymentName": "gpt-4o-mini"
  },
  "AzureOpenAI": {
    "Endpoint": "https://<your-resource>.openai.azure.com/",
    "DeploymentName": "gpt-4o-mini"
  },
  "Azure": {
    "SubscriptionId": "..."
  }
}
```

Or use User Secrets for sensitive values:

```powershell
cd src/AgenteRecaudacion
dotnet user-secrets set "AzureOpenAI:Endpoint" "https://<your-resource>.openai.azure.com/"
```

### Step 3: Run Locally with Agents Playground

```powershell
# Start the agent
cd src/AgenteRecaudacion
dotnet run

# In another terminal, start Agents Playground
agentsplayground
```

Navigate to `http://localhost:3000` and interact with the agent.

**Alternative**: Test directly in M365 Copilot Chat after deployment.

### Step 4: Test Function Tools

```text
# Example prompts to test:

# Campaign management
"Crea una campaña llamada 'Navidad Solidaria' con meta de 5000 euros"
"Muéstrame todas las campañas"
"Detalles de la campaña Navidad Solidaria"

# Task management
"Agrega una tarea 'Contactar donantes' a Navidad Solidaria"
"Asigna la tarea a Ana García"
"Mueve la tarea a en progreso"

# Comments
"Agrega un comentario: 'Llamé a 10 donantes, 3 confirmaron'"

# Thank-you messages
"Genera un mensaje de agradecimiento para María García que donó 100 euros a Navidad Solidaria"
```

---

## Development Workflow

### Project Structure

```text
src/
├── AgenteRecaudacion/       # Main agent project
│   ├── Agente/              # Agent implementation
│   │   ├── AgenteRecaudacion.cs   # Main agent
│   │   ├── Herramientas.cs        # Function tools
│   │   └── Instrucciones.cs       # System instructions
│   ├── Modelos/             # Data models
│   │   ├── Campaña.cs
│   │   ├── Tarea.cs
│   │   └── Usuario.cs
│   └── Datos/               # Sample data
│       └── DatosEjemplo.cs
└── AgenteRecaudacion.Tests/ # Test project
```

### Running Tests

```powershell
# All tests
dotnet test

# With code coverage
dotnet test /p:CollectCoverage=true

# Specific test project
dotnet test src/AgenteRecaudacion.Tests/AgenteRecaudacion.Tests.csproj
```

### Code Style (Spanish)

```csharp
// ✅ Correcto: Nombres en español
/// <summary>
/// Crea una nueva campaña de recaudación.
/// </summary>
/// <param name="nombre">Nombre de la campaña</param>
/// <param name="metaEuros">Meta financiera en euros</param>
/// <returns>Campaña creada</returns>
[AIFunction]
public Campaña CrearCampaña(string nombre, decimal metaEuros)
{
    var campaña = new Campaña
    {
        Nombre = nombre,
        MetaEuros = metaEuros
    };
    return campaña;
}

// ❌ Incorrecto: Nombres en inglés
public Campaign CreateCampaign(string name, decimal goalEuros)
{
    // ...
}
```

---

## Azure Deployment

### Option 1: Terraform (Recommended)

```powershell
# Navigate to infrastructure directory
cd infra

# Initialize Terraform
terraform init

# Review planned changes
terraform plan -var-file="terraform.tfvars"

# Apply infrastructure
terraform apply -var-file="terraform.tfvars"

# Get outputs
terraform output
```

### Option 2: Azure Developer CLI

```powershell
# Initialize azd (first time only)
azd init

# Provision + deploy
azd up

# Deploy code only
azd deploy

# Get endpoint URLs
azd show
```

### Post-Deployment

```powershell
# Test deployed agent
curl -X POST https://<agent-endpoint>/api/chat \
  -H "Content-Type: application/json" \
  -d '{"message": "Muéstrame las campañas"}'
```

---

## M365 Copilot Chat (Primary Interface)

**Users interact with the agent directly through M365 Copilot Chat** - no separate frontend required.

### Testing in M365 Copilot

1. Deploy agent to Azure AI Foundry
2. Publish to M365 Copilot from Foundry portal
3. Open Microsoft 365 Copilot Chat
4. Start conversation: "@AgenteRecaudación muéstrame las campañas"

## Optional: Standalone Frontend (Kanban UI)

**Note**: This is optional for standalone demos. Primary interface is M365 Copilot Chat.

### Setup

```powershell
cd frontend

# Install dependencies
npm install

# Configure environment
cp .env.example .env.local

# Edit .env.local with agent endpoint
NEXT_PUBLIC_AGENT_ENDPOINT=https://<your-agent>.azurewebsites.net
```

### Run Development Server

```powershell
npm run dev
```

Navigate to `http://localhost:3000` to see the Kanban board.

### Build for Production

```powershell
npm run build
npm run start
```

---

## Troubleshooting

### Issue: `Package 'Microsoft.Agents.AI' not found`

**Solution**: Add prerelease NuGet source

```powershell
dotnet nuget add source https://pkgs.dev.azure.com/microsoft/_packaging/MSAgents/nuget/v3/index.json --name MSAgents
dotnet restore --force
```

### Issue: Azure authentication failed

**Solution**: Login with Azure CLI

```powershell
az login
az account set --subscription ...
```

### Issue: GPT-4o-mini deployment not found

**Solution**: Create Azure OpenAI deployment

```powershell
az cognitiveservices account deployment create \
  --resource-group <your-rg> \
  --name <your-openai-resource> \
  --deployment-name gpt-4o-mini \
  --model-name gpt-4o-mini \
  --model-version "2024-07-18" \
  --model-format OpenAI \
  --sku-capacity 10 \
  --sku-name "Standard"
```

### Issue: Agents Playground not connecting

**Solution**: Ensure agent is running on correct port

```powershell
# Check agent is listening
netstat -an | findstr 5000

# Restart agent with explicit port (configured in appsettings.json or launchSettings.json)
cd src/AgenteRecaudacion
dotnet run --urls "http://localhost:5000"
```

---

## Demo Preparation

### Pre-Demo Checklist

#### Infrastructure Readiness

- [ ] All Azure resources provisioned (AI Foundry, Azure OpenAI, App Service)
- [ ] Azure AI Foundry agent endpoint responding (`/health` returns 200 OK)
- [ ] Azure OpenAI GPT-4o-mini deployment active and accessible
- [ ] Agent authentication configured (DefaultAzureCredential or API key)
- [ ] M365 Copilot agent manifest uploaded and approved

#### Data Preparation

- [ ] Sample campaigns pre-loaded (3 campaigns: Navidad Solidaria, Educación Rural, Salud Comunitaria)
- [ ] Sample tasks created (3-5 tasks per campaign, distributed across Kanban columns)
- [ ] Sample users available (Ana García, Carlos Ruiz, María López)
- [ ] Sample comments added to at least 2 tasks
- [ ] Test data matches quickstart examples

#### Functional Validation

- [ ] Run validation script: `.\scripts\validate-quickstart.ps1 -AgentUrl <agent-url> -Verbose`
- [ ] Test Campaign Management: Create, List, Get campaign details
- [ ] Test Task Management: Add task, Move task, Assign task, Get Kanban board
- [ ] Test Collaborative Comments: Add comment, List comments
- [ ] Test AI Message Generation: Generate thank-you message with different scenarios
- [ ] Verify Adaptive Cards render correctly in M365 Copilot Chat

#### Demo Environment

- [ ] Frontend deployed and accessible (if using standalone UI)
- [ ] M365 Copilot Chat access confirmed (test @mention)
- [ ] Demo environment stable (no active deployments during demo)
- [ ] Backup environment ready (local agent as fallback)
- [ ] Demo credentials secured (no secrets exposed in screenshots)

#### Presentation Readiness

- [ ] Architecture diagram prepared and visible
- [ ] Demo flow rehearsed (10 minutes, timed)
- [ ] Backup scenarios prepared (in case of network issues)
- [ ] Key talking points memorized:
  - Progressive complexity (Agent Framework → Azure AI Foundry → M365 SDK)
  - Bilingual separation (English specs, Spanish code/UI)
  - Demo-first development (fully functional in 10 minutes)
  - Microsoft-only stack (no third-party frameworks)
- [ ] Q&A preparation (common questions about architecture, scalability, M365 integration)

### Demo Flow (10 minutes)

1. **Introduction** (1 min): Show architecture diagram
2. **Campaign View** (2 min): List campaigns, show Kanban board
3. **Task Management** (3 min): Move tasks, assign users, add comments
4. **AI Message** (2 min): Generate thank-you message
5. **Progressive Complexity** (2 min): Show Agent Framework → Foundry → M365

### Demo Commands (M365 Copilot Chat)

```text
# 1. Show campaigns
@AgenteRecaudación muéstrame todas las campañas

# 2. View Kanban board
@AgenteRecaudación muestra el tablero de Navidad Solidaria

# 3. Move task
@AgenteRecaudación mueve la tarea 'Contactar donantes' a en progreso

# 4. Assign task
@AgenteRecaudación asigna la tarea a Carlos Ruiz

# 5. Add comment
@AgenteRecaudación agrega un comentario a la tarea: 'Llamé a 10 donantes hoy, 3 confirmaron su aportación'

# 6. Generate thank-you
@AgenteRecaudación genera un mensaje de agradecimiento para María García que donó 100 euros a Navidad Solidaria
```

---

## Resources

### Documentation

- [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview)
- [Azure AI Foundry](https://learn.microsoft.com/en-us/azure/ai-foundry/)
- [AG-UI Protocol](https://docs.ag-ui.com/)
- [CopilotKit](https://docs.copilotkit.ai/microsoft-agent-framework)

### Sample Code

- [Agent Framework Samples](https://github.com/microsoft/agent-framework/tree/main/python/samples)
- [AG-UI Dojo](https://dojo.ag-ui.com/microsoft-agent-framework-dotnet)

### Support

- [Feature Specification](./spec.md)
- [Data Model](./data-model.md)
- [Function Tools Contract](./contracts/function-tools.json)
- [Research Document](./research.md)

---

**Version**: 1.0.0  
**Last Updated**: 2025-12-01
