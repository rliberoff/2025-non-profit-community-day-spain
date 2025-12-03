# Agent Fundraising - M365 Copilot Agent for Non-Profit Fundraising

**Technology**: Microsoft Agent Framework (C# / .NET 10) + Azure AI Foundry + M365 Copilot Chat  
**Demo Date**: Non-Profit Community Day Spain 2025  
**Status**: In Development

## Overview

This project demonstrates a progressive AI agent development journey using Microsoft's enterprise AI stack:

1. **Level 1**: Microsoft Agent Framework - Code-first agent development with function calling
2. **Level 2**: Azure AI Foundry - Cloud-hosted persistent agents
3. **Level 3**: M365 Copilot Chat - Enterprise deployment (primary interface)

The agent helps non-profit organizations manage fundraising campaigns through natural language interactions in M365 Copilot Chat.

## Features

### User Story 1: Campaign Creation and Tracking ✅ MVP

- Create fundraising campaigns with financial goals
- List all campaigns
- View campaign details with associated tasks

### User Story 2: Task Organization with Kanban Board

- Add tasks to campaigns
- Organize tasks in Kanban columns (Por Hacer, En Progreso, Completado)
- Assign tasks to team members
- View Kanban board status

### User Story 3: Collaborative Task Notes

- Add comments to tasks
- View comment history chronologically
- Track team communication

### User Story 4: Automated Thank-You Message Generation

- Generate personalized Spanish thank-you messages for donors
- AI-powered message drafts using Azure OpenAI GPT-4o-mini
- Warm professional tone appropriate for non-profit organizations

## Quick Start

### Prerequisites

- .NET 10 SDK
- Visual Studio 2026 (18.0+) or Visual Studio Code with C# Dev Kit
- Azure CLI with active subscription
- Microsoft 365 account with Copilot license

### Local Development

```powershell
# Clone and build
cd e:\repos\personal\2025-non-profit-community-day-spain
dotnet restore AgentFundraising.sln
dotnet build AgentFundraising.sln

# Configure Azure credentials
cd src/AgentFundraising
dotnet user-secrets set "AzureOpenAI:Endpoint" "https://<your-resource>.openai.azure.com/"

# Run the agent
dotnet run
```

### Test in M365 Copilot Chat

After deployment, interact with the agent using natural language:

```text
"@AgentFundraising crea una campaña llamada 'Navidad Solidaria' con meta de 5000 euros"
"@AgentFundraising muéstrame todas las campañas"
"@AgentFundraising agrega una tarea 'Contactar donantes' a Navidad Solidaria"
"@AgentFundraising genera un mensaje de agradecimiento para María García que donó 100 euros"
```

## Project Structure

```text
src/
├── AgentFundraising/               # Main agent project
│   ├── Agent/                      # Agent implementation
│   │   ├── AgentFundraising.cs     # Main agent class (TBD)
│   │   ├── Tools.cs                # Function tools (11 methods)
│   │   └── Instructions.cs         # Spanish system instructions
│   ├── Models/                     # Data models (English names)
│   │   ├── Campaign.cs
│   │   ├── CampaignTask.cs
│   │   ├── User.cs
│   │   ├── TaskComment.cs
│   │   ├── ThankYouMessage.cs
│   │   ├── KanbanColumn.cs         # Enum
│   │   ├── UserRole.cs             # Enum
│   │   └── AgentState.cs
│   ├── Data/                       # Sample data
│   │   └── SampleData.cs
│   ├── Validators/                 # Business logic validators
│   │   ├── CampaignValidator.cs
│   │   ├── TaskValidator.cs
│   │   └── CommentValidator.cs
│   └── Program.cs                  # ASP.NET Core host
```

## Technology Stack

- **Agent Framework**: Microsoft Agent Framework (Microsoft.Agents.AI)
- **AI Model**: Azure OpenAI GPT-4o-mini
- **Cloud Platform**: Azure AI Foundry
- **Interface**: M365 Copilot Chat (primary), Adaptive Cards (visualization)
- **Language**: C# / .NET 10
- **Infrastructure**: Terraform (Azure resources)

## Documentation

- **Specification**: [`specs/001-fundraising-copilot-agent/spec.md`](specs/001-fundraising-copilot-agent/spec.md)
- **Implementation Plan**: [`specs/001-fundraising-copilot-agent/plan.md`](specs/001-fundraising-copilot-agent/plan.md)
- **Data Model**: [`specs/001-fundraising-copilot-agent/data-model.md`](specs/001-fundraising-copilot-agent/data-model.md)
- **Quick Start**: [`specs/001-fundraising-copilot-agent/quickstart.md`](specs/001-fundraising-copilot-agent/quickstart.md)
- **Tasks**: [`specs/001-fundraising-copilot-agent/tasks.md`](specs/001-fundraising-copilot-agent/tasks.md)

## Azure Deployment

### Using Terraform

```powershell
cd infra
terraform init
terraform plan -var-file="terraform.tfvars"
terraform apply -var-file="terraform.tfvars"
```

## Demo Scenario

**Duration**: 10 minutes  
**Audience**: Technical developers at Non-Profit Community Day Spain

1. Show campaign creation via M365 Copilot Chat
2. Demonstrate task management and Kanban workflow
3. Add collaborative comments to tasks
4. Generate AI-powered thank-you message for donor
5. Explain progressive complexity: Agent Framework → Foundry → M365

## License

This project is for educational and demonstration purposes at Non-Profit Community Day Spain 2025.

## Contributing

This is a demo project. For questions or contributions, please refer to the project specification documents.

---

## **Built with ❤️ for Non-Profit Community Day Spain 2025**
