# Agent Fundraising - M365 Copilot Agent for Non-Profit Fundraising

**Technology**: Microsoft Agent Framework (C# / .NET 10) + Azure AI Foundry + M365 Copilot Chat  
**Demo Date**: Non-Profit Community Day Spain 2025  
**Status**: Demo

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

### Starting the Agent

**Authentication**: The agent uses Azure Default Credentials. You must be logged in to Azure with an account that has the following roles:

- **Cognitive Services OpenAI User** on the Azure OpenAI resource
- **Reader** on the resource group (minimum)

```powershell
# Login to Azure
az login

# Verify your account and subscription
az account show

# Clone and build
cd e:\repos\personal\2025-non-profit-community-day-spain
dotnet restore Demo.Agent.Fundraising.sln
dotnet build Demo.Agent.Fundraising.sln

# Run the agent (backend)
cd src/Demo.Agent.Fundraising
dotnet run
```

**Note**: No API keys are required. The application authenticates using your Azure CLI credentials via `DefaultAzureCredential`.

## Local Development with M365 Copilot Chat

### Prerequisites for M365 Integration

- [Ngrok](https://ngrok.com/) account and authtoken configured
- [Microsoft 365 Agents Toolkit CLI](https://aka.ms/teamsfx-toolkit-cli) (`npm install -g @microsoft/m365-agents-toolkit`)
- Node.js 18+ (required by `atk` CLI)
- M365 account with Copilot license

### Step 1: Start the Agent Locally

```powershell
# Navigate to the agent project
cd src/Demo.Agent.Fundraising

# Run the agent on port 5052 (configured in launchSettings.json)
dotnet run
```

The agent will start listening on `http://localhost:5052`.

### Step 2: Expose Local Agent with Ngrok

In a **separate terminal**:

```powershell
# Start ngrok tunnel to expose port 5052
ngrok http http://localhost:5052 --host-header="localhost"
```

Ngrok will display a forwarding URL like:

```text
Forwarding   https://abc123def456.ngrok-free.app -> http://localhost:5052
```

**Copy the HTTPS URL** (e.g., `https://abc123def456.ngrok-free.app`).

### Step 3: Configure M365 Agent Project

Navigate to the M365 agent configuration:

```powershell
cd src/Demo.Agent.Fundraising.M365
```

Edit `env/.env.local` and replace `API_URL` with your Ngrok URL:

```dotenv
# env/.env.local
TEAMSFX_ENV=local
APP_NAME_SUFFIX=local
TEAMS_APP_ID=5da7ed47-4ead-451e-8fa4-717233d5d446
API_URL=https://abc123def456.ngrok-free.app
```

### Step 4: Package the Agent

Using the Microsoft 365 Agents Toolkit CLI (`atk`):

```powershell
# Generate the app package with your configuration
atk package --env local
```

This creates `appPackage/build/appPackage.local.zip` with:

- App manifest with your Ngrok URL
- Declarative agent definition
- OpenAPI specification
- Icons and metadata

### Step 5: Deploy via Teams Developer Portal

1. **Open Teams Developer Portal**  
   Navigate to [https://dev.teams.microsoft.com/](https://dev.teams.microsoft.com/)

2. **Import the App Package**
   - Click **Apps** in the left navigation
   - Click **Import app**
   - Upload `src/Demo.Agent.Fundraising.M365/appPackage/build/appPackage.local.zip`

3. **Review App Details**
   - Verify the app name: **Agent Fundrising [local]**
   - Check the **Copilot** section shows the declarative agent
   - Confirm the **Valid domains** includes `*.ngrok-free.app`

4. **Preview in Teams**
   - Click **Preview in Teams** button (top-right corner)
   - Teams will open and prompt you to add the app
   - Click **Add** to install it in your personal scope

### Step 6: Test in M365 Copilot

Type `@Agent Fundrising [local]` to invoke your agent and test with sample prompts:

```text
@Agent Fundrising [local] muéstrame todas las campañas
@Agent Fundrising [local] crea una campaña llamada "Navidad Solidaria" con meta de 5000 euros
@Agent Fundrising [local] genera un mensaje de agradecimiento para María García que donó 100 euros
```

### Updating After Code Changes

When you modify the agent code:

1. **Restart the .NET agent**:

   ```powershell
   # In src/Demo.Agent.Fundraising
   dotnet run
   ```

2. **Changes are live immediately** through the Ngrok tunnel - no repackaging needed!

3. **If Ngrok URL changes** (after restarting Ngrok):
   - Update `API_URL` in `env/.env.local`
   - Repackage: `atk package --env local`
   - Re-upload to Teams Developer Portal (replace existing app)
   - Click **Preview in Teams** again to update

### Alternative: Visual Studio Code Extension

Instead of using the CLI, you can use the VS Code extension:

1. Open `src/Demo.Agent.Fundraising.M365` folder in VS Code
2. Install [Microsoft 365 Agents Toolkit extension](https://aka.ms/teams-toolkit)
3. Press `F5` or select **Preview Local in Copilot (Edge/Chrome)** from debug dropdown
4. The extension handles packaging and sideloading automatically

### Troubleshooting

**Issue**: Agent not responding in M365 Copilot

**Solutions**:

- Verify the agent is running on port 5052: `netstat -an | findstr 5052`
- Check Ngrok tunnel is active: visit the Ngrok dashboard at `http://127.0.0.1:4040`
- Ensure `API_URL` in `src/Demo.Agent.Fundraising.M365/env/.env.local` matches your current Ngrok URL
- Test the agent directly: `curl https://abc123def456.ngrok-free.app/health`

**Issue**: Ngrok shows "502 Bad Gateway"

**Solution**: Restart the .NET agent (`dotnet run`) and verify it's listening.

**Issue**: `atk: command not found`

**Solution**: Install the CLI globally:

```powershell
npm install -g @microsoft/m365-agents-toolkit
```

**Issue**: App not visible in M365 Copilot plugins

**Solutions**:

- Wait 5-10 minutes after adding the app (propagation delay)
- Refresh M365 Copilot page
- Verify your M365 account has Copilot license
- Try logging out and back into M365 Copilot

**Issue**: "Preview in Teams" button is disabled

**Solution**: Ensure all required fields in the manifest are filled and there are no validation errors shown in the portal.

### Using Hot Reload (Optional)

Enable .NET hot reload for faster iteration:

```powershell
cd src/Demo.Agent.Fundraising
dotnet watch run
```

Code changes will automatically reload without restarting the agent.

## Project Structure

```text
src/
├── Demo.Agent.Fundraising/         # Main agent project
│   ├── Agent/                      # Agent implementation
│   │   ├── FundraisingAgent.cs     # Main agent class
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
- **AI Model**: Azure OpenAI
- **Cloud Platform**: Azure AI Foundry
- **Interface**: M365 Copilot Chat (primary)
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

## Session Presentation

This document corresponds to the official presentation of NonProfit Day 2025, an event focused on empowering nonprofit organizations through the use of Microsoft Artificial Intelligence technologies. The session explores how tools such as AI Builder, Copilot Studio, Azure AI Foundry, and Microsoft 365 Copilot can transform operations, expand social impact, and accelerate innovation within the sector.

[Download the session presentation (PDF)](Presentation.pdf)

### Demo Scenario

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
