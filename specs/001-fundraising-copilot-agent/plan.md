# Implementation Plan: M365 Copilot Agent for Non-Profit Fundraising Management

**Branch**: `001-fundraising-copilot-agent` | **Date**: 2025-12-01 | **Spec**: [spec.md](spec.md)  
**Input**: Feature specification from `/specs/001-fundraising-copilot-agent/spec.md`

## Summary

Build an M365 Copilot agent for non-profit fundraising campaign management using **Microsoft Agent Framework** (C# / .NET 10) + **Azure AI Foundry** + **AG-UI/CopilotKit** for the Kanban interface. The agent enables campaign creation, task management with Kanban workflow, collaborative comments, and AI-powered thank-you message generation. Demonstrates progressive complexity: Agent Framework (code) → Azure AI Foundry (cloud) → M365 SDK (enterprise integration).

**Technical Approach**: Agent Framework provides the agent orchestration with function calling for campaign/task operations. Azure AI Foundry hosts the persistent agent with GPT-4o-mini. AG-UI protocol + CopilotKit React components deliver the interactive Kanban board. Optional M365 Agents SDK wrapper enables Copilot deployment.

## Technical Context

**Language/Version**: C# / .NET 10

**Primary Dependencies**:

- `Microsoft.Agents.AI` (Microsoft Agent Framework)
- `Microsoft.Agents.AI.AzureAI.Persistent` (Azure AI Foundry integration)
- `Azure.AI.Projects` (Azure AI SDK)
- `Azure.Identity` (Azure authentication)
- `Azure.AI.OpenAI` (Azure OpenAI client)
- `Microsoft.AspNetCore.App` (Agent hosting)

**Storage**:

- In-memory state (campaigns, tasks, comments) for demo
- Azure AI Foundry thread management (conversation history)
- Optional: Azure Storage (pre-populated sample data)

**Testing**:

- C#: `xUnit` or `NUnit` for function tool tests
- Agent: Agents Playground (local + Foundry portal)
- M365 Integration: M365 Copilot Chat (test in Microsoft 365 environment)

**Target Platform**:

- Backend: Azure App Service (Linux, .NET 10 container) or Azure Container Apps
- Agent: Azure AI Foundry Agent Service
- User Interface: M365 Copilot Chat (primary access point)
- Optional: Adaptive Cards rendered in Copilot Chat for Kanban visualization

**Project Type**: .NET Web API (agent host) + M365 Copilot Chat interface

**Performance Goals**:

- Campaign creation: < 30 seconds (SC-001)
- Task operations: < 1 minute for full workflow (SC-002)
- AI message generation: < 15 seconds (SC-003)
- Comment visibility: < 5 seconds (SC-006)
- Full demo: < 10 minutes (SC-007)

**Constraints**:

- Single-user demo (no concurrent access requirements)
- Demo-session persistence only (no long-term data retention)
- Spanish language for all user-facing content
- Microsoft-only technology stack
- 10-minute demonstration window

**Scale/Scope**:

- 3 predefined campaigns
- 3-5 tasks per campaign (15 total tasks)
- 3 predefined users
- Single demonstrator
- No production deployment requirements

## Constitution Check

_GATE: Must pass before Phase 0 research. Re-check after Phase 1 design._

### ✅ Technology Stack Compliance (Principle II)

**Status**: PASS

- ✅ **Agents**: Microsoft Agent Framework (permitted)
- ✅ **Cloud**: Azure AI Foundry, Azure OpenAI, Azure App Service (permitted)
- ✅ **Integration**: M365 Copilot Chat (native), Adaptive Cards (Microsoft standard)
- ✅ **Languages**: C# / .NET 10 (backend), JavaScript/TypeScript (optional frontend) - both permitted
- ❌ **No violations**: No third-party agent frameworks, no non-Microsoft LLMs

### ✅ Demo-First Development (Principle I)

**Status**: PASS

- ✅ All components demonstrable within 10-minute window
- ✅ Clear progression: local agent → Azure Foundry → M365 Copilot Chat
- ✅ Self-contained: pre-populated sample data eliminates setup time
- ✅ Visual impact: Adaptive Cards in Copilot Chat, live AI message generation

### ✅ Bilingual Separation (Principle III)

**Status**: PASS

- ✅ Specifications: English (this document, spec.md, research.md)
- ✅ Code: Spanish class names, method names, variable names, comments (to be implemented)
- ✅ M365 Copilot Chat: Spanish prompts, responses, Adaptive Card text

### ✅ Three-Scenario Coverage (Principle IV)

**Status**: PASS

- ✅ This feature addresses Scenario 1 of 3: **Captación de fondos** (Fundraising)
- ⚠️ Note: Two additional features required for Scenarios 2 & 3 (future work)

### ✅ Progressive Complexity (Principle V)

**Status**: PASS - **Exemplary**

Demonstrates clear escalation path:

1. **Level 1 - Agent Framework**: Code-first agent development with function calling
2. **Level 2 - Azure AI Foundry**: Cloud-hosted persistent agents with managed infrastructure
3. **Level 3 - M365 SDK** (optional): Enterprise deployment to Microsoft 365 Copilot

**Justification**: Each level builds on the previous, showing when and why to adopt more sophisticated approaches. Agent Framework (not Copilot Studio) chosen as starting point because it demonstrates the code/orchestration layer that Copilot Studio would abstract away - essential for technical audience understanding.

### ✅ Spec-Driven Development (Principle VI)

**Status**: PASS

- ✅ Specification authored first (spec.md)
- ✅ All requirements traced to FR-XXX identifiers
- ✅ Acceptance criteria testable and measurable
- ✅ Implementation follows approved specification

---

**Gate Decision**: ✅ **PROCEED** - All constitutional principles satisfied

## Project Structure

### Documentation (this feature)

```text
specs/001-fundraising-copilot-agent/
├── spec.md              # Feature specification (COMPLETE)
├── plan.md              # This file - implementation plan (IN PROGRESS)
├── research.md          # Phase 0 output - technology selection (COMPLETE)
├── data-model.md        # Phase 1 output - entity definitions (TODO)
├── quickstart.md        # Phase 1 output - getting started guide (TODO)
├── contracts/           # Phase 1 output - API contracts (TODO)
│   ├── function-tools.json      # Agent Framework function definitions
│   └── ag-ui-protocol.json      # AG-UI event schemas
├── tasks.md             # Phase 2 output - task breakdown (TODO)
└── checklists/
    └── requirements.md  # Specification validation (COMPLETE)
```

### Source Code (repository root)

```text
# Backend (.NET 10 Agent Framework)
src/
├── AgenteRecaudacion/             # Main agent project
│   ├── Agente/                    # Agent implementation
│   │   ├── AgenteRecaudacion.cs   # Main agent class (AIAgent)
│   │   ├── Herramientas.cs        # Function tools (AIFunction methods)
│   │   └── Instrucciones.cs       # System instructions (Spanish)
│   ├── Modelos/                   # Data models
│   │   ├── Campaña.cs             # Campaign model
│   │   ├── Tarea.cs               # Task model
│   │   ├── Usuario.cs             # Team member model
│   │   ├── Comentario.cs          # Comment model
│   │   └── MensajeAgradecimiento.cs
│   ├── Datos/                     # Sample data
│   │   └── DatosEjemplo.cs        # Pre-populated campaigns
│   ├── Program.cs                 # Application entry point
│   ├── AgenteRecaudacion.csproj   # Project file
│   ├── appsettings.json           # Configuration
│   └── Dockerfile                 # Container image
├── AgenteRecaudacion.Tests/       # Test project
│   ├── Unit/
│   │   ├── HerramientasTests.cs
│   │   ├── ModelosTests.cs
│   │   └── AgenteTests.cs
│   └── Integration/
│       └── AgenteIntegrationTests.cs
└── AgenteRecaudacion.sln          # Solution file

# Frontend (React + CopilotKit)
frontend/
├── src/
│   ├── components/
│   │   ├── ListaCampañas.tsx    # Campaign list
│   │   ├── TableroKanban.tsx    # Kanban board
│   │   ├── TarjetaTarea.tsx     # Task card
│   │   ├── SeccionComentarios.tsx # Comments
│   │   └── GeneradorAgradecimiento.tsx # Thank-you generator
│   ├── pages/
│   │   ├── index.tsx             # Home page
│   │   └── campaña/[id].tsx     # Campaign detail
│   ├── servicios/
│   │   └── cliente_agente.ts    # AG-UI client
│   └── App.tsx
├── public/
│   └── locales/
│       └── es.json                # Spanish translations
├── package.json
├── tsconfig.json
└── Dockerfile

# Infrastructure (Terraform)
infra/
├── main.tf                        # Main Terraform config
├── variables.tf                   # Input variables
├── outputs.tf                     # Output values
├── terraform.tfvars              # Variable values (sensitive)
├── modules/
│   ├── ai-foundry/               # Azure AI Foundry resources
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   └── outputs.tf
│   ├── app-service/              # Backend hosting
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   └── outputs.tf
│   └── static-web-app/           # Frontend hosting (optional)
│       ├── main.tf
│       ├── variables.tf
│       └── outputs.tf
└── README.md                      # Deployment guide

# Root configuration
.github/
├── workflows/
│   ├── ci-backend.yml            # .NET tests
│   ├── ci-frontend.yml           # React tests
│   └── cd-azure.yml              # Azure deployment
└── instructions/
    └── (existing instruction files)

README.md                          # Project overview
.gitignore
```

**Structure Decision**: **.NET solution with M365 Copilot Chat interface** because:

1. **Backend (C# / .NET 10)**: Agent Framework agent hosted as ASP.NET Core app
2. **M365 Copilot Chat**: Primary user interface (no separate frontend needed)
3. **Adaptive Cards** (optional): Rich Kanban visualization within Copilot Chat
4. **Infrastructure (Terraform)**: Azure resource provisioning
5. **Frontend (React/TypeScript)** (optional): Standalone web UI for demos if needed

This structure aligns with:

- Constitution Principle III (bilingual): C# code in Spanish, Copilot Chat responses in Spanish
- Demo requirements: Users access directly through M365 Copilot Chat (zero additional UI)
- Microsoft standards: Native M365 integration, .NET best practices

## Complexity Tracking

**No constitutional violations detected** - This section intentionally left empty per template instructions.

All technology choices align with Principle II (Microsoft stack):

- ✅ Microsoft Agent Framework (permitted agent platform)
- ✅ Azure AI Foundry (permitted cloud service)
- ✅ C# / .NET 10 and TypeScript (permitted languages)
- ✅ AG-UI protocol (Microsoft-endorsed standard)
- ✅ CopilotKit (community tool compatible with Microsoft ecosystem)

No complexity justification required.

---

## Plan Completion Status

### Phase 0: Research ✅ COMPLETE

**Output**: `research.md`

**Key Decisions**:

- Technology: Microsoft Agent Framework + Azure AI Foundry + M365 Copilot Chat
- Language: C# / .NET 10 (backend), TypeScript/React (optional frontend)
- Progressive complexity: Agent Framework → Foundry → M365 Copilot Chat
- Rationale: Best demo narrative for Microsoft technology showcase

### Phase 1: Design & Contracts ✅ COMPLETE

**Outputs**:

- `data-model.md`: 5 core entities (Campaña, Tarea, Usuario, Comentario, MensajeAgradecimiento) - ✅ **C# / .NET 10**
- `contracts/function-tools.json`: 11 agent function tools with C# signatures
- `quickstart.md`: Development and deployment guide - ✅ **.NET 10**
- `.github/agents/copilot-instructions.md`: Updated agent context - ✅ **.NET 10**

**Key Artifacts**:

- Entity relationship diagram with Mermaid
- Function tool schemas (JSON Schema format)
- Sample data definitions
- Validation rules
- Terraform module structure
- Frontend component structure

### Phase 2: Implementation Tasks 🔜 NEXT

**Command**: `/speckit.tasks` (separate command, not part of `/speckit.plan`)

**Expected Output**: `tasks.md` with phased task breakdown

**Recommended Phases**:

1. **Phase 1**: Core agent (function tools, in-memory state)
2. **Phase 2**: M365 Copilot Chat integration (primary interface)
3. **Phase 3**: Optional frontend UI (React, Adaptive Cards)
4. **Phase 4**: Azure deployment (Terraform, Container Apps)
5. **Phase 5** (optional): Advanced Foundry features

---

## Next Steps

1. ✅ **Planning Complete**: This document captures all design decisions
2. 🔜 **Create Tasks**: Run `/speckit.tasks` to generate task breakdown
3. 🔜 **Begin Implementation**: Start with Phase 1 (core agent)
4. 🔜 **Iterative Development**: Test each phase with Agents Playground
5. 🔜 **Deploy to Azure**: Use Terraform scripts in `infra/`
6. 🔜 **Rehearse Demo**: Practice 10-minute demo flow

---

**Plan Status**: ✅ **COMPLETE**  
**Branch**: `001-fundraising-copilot-agent`  
**Version**: 1.0.0  
**Last Updated**: 2025-12-01
