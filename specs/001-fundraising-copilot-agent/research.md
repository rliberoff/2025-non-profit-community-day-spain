# Research: Technology Selection for Fundraising Copilot Agent

**Feature**: M365 Copilot Agent for Non-Profit Fundraising Management  
**Date**: 2025-12-01  
**Status**: Phase 0 Complete

## Executive Summary

After evaluating three Microsoft agent technologies (M365 Agents SDK, Agent Framework, Azure AI Foundry), the **recommended approach** is:

**Primary**: Microsoft Agent Framework (C# / .NET 10) + Azure AI Foundry  
**Deployment**: Azure AI Foundry Agent Service → M365 Copilot Chat

**Rationale**: Best balance of demo clarity, Microsoft alignment, and progressive complexity demonstration. Users interact directly through M365 Copilot Chat interface.

---

## Technology Options Evaluated

### Option 1: Microsoft 365 Agents SDK

**Description**: SDK for creating agents deployable to M365 Copilot, Teams, and custom channels with built-in conversation management.

**Pros**:

- ✅ Direct M365 Copilot integration (primary requirement)
- ✅ Built-in channel adapters for Teams, Copilot
- ✅ Conversation state management handled by SDK
- ✅ Supports C#, JavaScript, Python
- ✅ Microsoft 365 Agents Toolkit for scaffolding
- ✅ Local testing via Agents Playground

**Cons**:

- ⚠️ Focused on conversation/activity protocol (not orchestration)
- ⚠️ Requires additional orchestration layer (Semantic Kernel/Agent Framework)
- ⚠️ Less suitable for complex agent logic (delegates to other frameworks)
- ⚠️ Primary purpose is wrapping existing agents, not building new ones

**Constitution Alignment**:

- ✅ Principle II: Microsoft technology stack
- ⚠️ Principle V: Doesn't show progressive complexity (it's the wrapper layer)

**Best For**: Wrapping existing agents built with other frameworks for M365 deployment

---

### Option 2: Microsoft Agent Framework (MAF)

**Description**: Production-ready framework combining Semantic Kernel and AutoGen for agentic AI systems with enterprise features.

**Pros**:

- ✅ **Agentic patterns out of the box**: Multi-agent orchestration, function calling, streaming
- ✅ **Enterprise-grade**: OpenTelemetry observability, security integration
- ✅ **Standards-based**: A2A protocol, Model Context Protocol (MCP)
- ✅ **Provider-agnostic**: Works with Azure AI Foundry, OpenAI, Azure OpenAI
- ✅ **Multi-language**: C# (.NET 8) and Python 3.11+
- ✅ **AG-UI integration**: Web-based agent UIs with CopilotKit
- ✅ **Azure AI Foundry native**: Direct integration with Foundry agents

**Cons**:

- ⚠️ Requires M365 Agents SDK wrapper for Copilot deployment
- ⚠️ More complex than pure Copilot Studio (but shows progressive complexity)
- ⚠️ Newer framework (public preview status)

**Constitution Alignment**:

- ✅ Principle II: Microsoft technology stack (100% Microsoft)
- ✅ Principle V: **Perfect for progressive complexity** (shows professional AI engineering)
- ✅ Principle I: Demo-first (can show both simple and complex scenarios)

**Best For**: Building sophisticated agents that can be deployed to M365 after wrapping

---

### Option 3: Azure AI Foundry Agents

**Description**: Cloud-based persistent agent service with service-managed threads and state.

**Pros**:

- ✅ **Persistent agents**: Server-side agent instances with managed state
- ✅ **Service-managed threads**: Conversation history handled by Azure
- ✅ **Built-in tools**: Code interpreter, file search
- ✅ **Deployment options**: Direct M365 publishing or Toolkit integration
- ✅ **Compatible with Agent Framework**: Can use MAF to interact with Foundry agents
- ✅ **Portal-based**: Can create agents in Foundry portal or via SDK

**Cons**:

- ⚠️ Requires Azure infrastructure (but we need Azure anyway per constitution)
- ⚠️ Best when combined with Agent Framework for orchestration
- ⚠️ Classic version being superseded by new Foundry portal

**Constitution Alignment**:

- ✅ Principle II: Microsoft technology stack (Azure cloud platform)
- ✅ Principle V: Shows cloud-hosted agent scenario (advanced complexity)
- ✅ Principle I: Demo-ready (portal + SDK demonstration)

**Best For**: Cloud-hosted persistent agents with managed infrastructure

---

## Decision Matrix

| Criterion                  | M365 Agents SDK          | Agent Framework             | Azure AI Foundry         |
| -------------------------- | ------------------------ | --------------------------- | ------------------------ |
| **Demo Clarity**           | Medium (wrapper concept) | High (core logic)           | High (cloud service)     |
| **Microsoft Alignment**    | High (M365 native)       | High (MS framework)         | High (Azure service)     |
| **Progressive Complexity** | Low (single layer)       | **High** (shows escalation) | Medium (advanced only)   |
| **Development Speed**      | Fast (templates)         | Medium (code-first)         | Fast (portal + SDK)      |
| **Agent Logic Clarity**    | Low (delegated)          | **High** (direct)           | Medium (service-managed) |
| **Deployment Complexity**  | Medium (Azure Bot)       | Medium (via SDK)            | Low (portal publish)     |
| **Kanban UI Requirements** | Needs custom UI          | AG-UI/CopilotKit            | Needs custom UI          |
| **Spanish Language**       | Supported                | Supported                   | Supported                |

---

## Recommended Architecture

### Primary Approach: Agent Framework + Azure AI Foundry

**Stack**:

- **Language**: C# with .NET 10
- **Agent Core**: Microsoft Agent Framework (`agent-framework`)
- **Hosting**: Azure AI Foundry Agent Service (persistent agents)
- **M365 Integration**: M365 Agents SDK wrapper (for Copilot deployment)
- **UI Layer**: AG-UI protocol + CopilotKit (for Kanban board)
- **Infrastructure**: Azure (Foundry, Storage, App Service)

**Why This Combination**:

1. **Demonstrates Progressive Complexity** ✅

    - Start: Agent Framework basics (function calling, streaming)
    - Middle: Azure AI Foundry deployment (cloud-hosted agents)
    - Advanced: M365 Copilot integration (enterprise deployment)

2. **Aligns with All Constitutional Principles** ✅

    - Principle I: Demo-first (each layer is demonstrable)
    - Principle II: 100% Microsoft stack (Agent Framework, Foundry, M365 SDK)
    - Principle III: Bilingual (specs English, code/UI Spanish)
    - Principle V: Perfect progressive complexity story

3. **Best Technical Fit** ✅

    - Agent Framework handles campaign/task logic naturally
    - AG-UI protocol enables Kanban board interaction
    - Azure AI Foundry provides persistent state for demo
    - M365 SDK enables Copilot deployment (optional demo extension)

4. **Demo Narrative** ✅
    - "Start with Agent Framework to build the agent"
    - "Deploy to Azure AI Foundry for cloud hosting"
    - "Wrap with M365 SDK to surface in Copilot"
    - Shows the complete enterprise journey

---

## Implementation Plan

### Phase 1: Core Agent (Agent Framework)

**Technology**: Microsoft Agent Framework (C# / .NET 10)

**Components**:

- `ChatAgent` with function tools for campaign/task management
- In-memory state for campaigns, tasks, comments
- Spanish language prompts/instructions
- Function tools:
  - `crear_campaña(nombre, meta)`
  - `listar_campañas()`
  - `agregar_tarea(campaña_id, descripción)`
  - `mover_tarea(tarea_id, columna)`
  - `asignar_tarea(tarea_id, usuario)`
  - `agregar_comentario(tarea_id, comentario)`
  - `generar_agradecimiento(nombre_donante, campaña, monto)`

**AI Model**: Azure OpenAI GPT-4o-mini (via Azure AI Foundry connection)

**Testing**: Local Agents Playground

**Output**: Functional agent with all FR requirements met

---

### Phase 2: Kanban UI (AG-UI + CopilotKit)

**Technology**: AG-UI protocol + CopilotKit React components

**Components**:

- FastAPI endpoint exposing AG-UI protocol
- `AgentFrameworkAgent` adapter for Agent Framework integration
- CopilotKit React UI with:
  - Campaign list view
  - Kanban board (3 columns: Por hacer, En progreso, Completado)
  - Task cards with assignee display
  - Comment section per task
  - Thank-you message generator

**State Management**: Bidirectional state sync via AG-UI protocol

**Language**: Spanish UI labels and content

**Testing**: Local web browser

**Output**: Interactive web UI for demo

---

### Phase 3: Azure Deployment (Azure AI Foundry)

**Technology**: Azure AI Foundry Agent Service

**Components**:

- Azure AI Foundry project
- Agent deployment (persistent agent)
- Azure OpenAI connection (GPT-4o-mini)
- Azure Storage (optional, for demo data persistence)
- Container Registry (for AG-UI web app)
- App Service (for AG-UI frontend)

**Infrastructure**: Terraform scripts

**Deployment**: `azd` CLI or manual Terraform apply

**Testing**: Foundry portal Agent Playground

**Output**: Cloud-hosted agent accessible via HTTPS endpoint

---

### Phase 4: M365 Integration (Optional/Advanced Demo)

**Technology**: M365 Agents SDK wrapper

**Components**:

- M365 Agents SDK channel adapter
- Azure Bot Service registration
- Entra ID app registration
- Teams app manifest

**Deployment**: Azure Bot Service

**Testing**: M365 Copilot chat interface

**Output**: Agent available in Microsoft 365 Copilot

---

## Technology Dependencies

### NuGet Packages

```xml
<!-- Agent Framework -->
<PackageReference Include="Microsoft.Agents.AI" Version="0.1.*-preview" />
<PackageReference Include="Microsoft.Agents.AI.AzureAI.Persistent" Version="0.1.*-preview" />
<PackageReference Include="Microsoft.Agents.AI.OpenAI" Version="0.1.*-preview" />

<!-- Azure SDKs -->
<PackageReference Include="Azure.AI.Projects" Version="1.0.*" />
<PackageReference Include="Azure.Identity" Version="1.12.*" />
<PackageReference Include="Azure.AI.OpenAI" Version="2.0.*" />

<!-- ASP.NET Core (for agent hosting) -->
<PackageReference Include="Microsoft.AspNetCore.App" />
```

### Azure Resources (Terraform)

```hcl
- Azure AI Foundry Hub
- Azure AI Foundry Project
- Azure OpenAI Service (GPT-4o-mini deployment)
- Azure Container Registry
- Azure App Service (Linux, .NET 10)
- Azure Storage Account (optional)
- Azure Application Insights (observability)
```

### Development Tools

- .NET 10 SDK
- Visual Studio 2026 (18.0+) or Visual Studio Code with C# Dev Kit
- Agents Playground (local testing)
- Azure CLI (`az`)
- Azure Developer CLI (`azd`)
- Terraform 1.14+

---

## Alternative Considered: Copilot Studio

**Why Not Copilot Studio**:

While Copilot Studio is the "simple" starting point per Principle V, it was **not included in user's technology options** and has significant limitations for this scenario:

1. **Kanban Board**: Copilot Studio has limited custom UI capabilities
2. **State Management**: Complex state (campaigns, tasks, comments) harder to manage
3. **Demo Narrative**: Doesn't show code-first development (low-code only)
4. **Progressive Complexity**: Hard to show escalation to code when starting no-code

**However**: Could mention Copilot Studio in presentation as "where you'd start for simpler scenarios"

---

## Risks & Mitigations

### Risk 1: Agent Framework Complexity

**Risk**: Agent Framework may be too complex for 10-minute demo  
**Mitigation**: Pre-built agent with simple function tools; focus demo on capabilities not code  
**Fallback**: Simplify to Azure OpenAI + custom logic (but loses orchestration demo value)

### Risk 2: AG-UI Integration Immaturity

**Risk**: CopilotKit integration with Agent Framework is newer  
**Mitigation**: Use reference implementation from AG-UI Dojo samples  
**Fallback**: Simple HTML/JS UI with REST API (loses interactivity but still functional)

### Risk 3: Azure Deployment Time

**Risk**: Provisioning Azure resources takes time during demo  
**Mitigation**: Pre-provision all infrastructure before session; demo only shows interaction  
**Fallback**: Show localhost version with "this is running in Azure" narrative

### Risk 4: M365 SDK Wrapper Adds Complexity

**Risk**: Time constraints don't allow full M365 Copilot integration demo  
**Mitigation**: Make Phase 4 optional; focus on Agent Framework + Foundry (still shows 2 levels)  
**Fallback**: Mention M365 integration as "next step" with slide/architecture diagram

---

## Sample Campaigns Pre-population

Per DD-002, three campaigns pre-loaded with tasks:

### Campaign 1: Navidad Solidaria (5,000 €)

**Tasks** (Por hacer):

- Contactar 20 donantes principales
- Diseñar materiales de campaña
- Organizar evento de lanzamiento

**Tasks** (En progreso):

- Enviar emails de agradecimiento

**Tasks** (Completado):

- Configurar página de donaciones

### Campaign 2: Educación Rural (3,000 €)

**Tasks** (Por hacer):

- Reunión con escuelas rurales
- Preparar propuesta de proyecto
- Solicitar subvenciones

**Tasks** (En progreso):

- Reclutar voluntarios docentes

**Tasks** (Completado):

- Investigar necesidades educativas

### Campaign 3: Salud Comunitaria (4,000 €)

**Tasks** (Por hacer):

- Coordinar con centros de salud
- Planificar jornadas de salud

**Tasks** (En progreso):

- Comprar material médico
- Formar voluntarios sanitarios

**Tasks** (Completado):

- Identificar comunidades objetivo

**Team Members**:

- **Coordinator**: Ana García (Coordinadora de Campaña)
- **Volunteer 1**: Carlos Ruiz (Voluntario)
- **Volunteer 2**: María López (Voluntaria)

---

## Next Steps (Phase 1 Implementation)

1. ✅ Create Terraform scripts for Azure infrastructure
2. ✅ Scaffold C# / .NET 10 Agent Framework project
3. ✅ Implement function tools for campaign/task management
4. ✅ Add Spanish language instructions
5. ✅ Pre-populate sample campaigns/tasks
6. ✅ Test locally with Agents Playground
7. ✅ Create AG-UI FastAPI endpoint
8. ✅ Build CopilotKit React UI
9. ✅ Deploy to Azure AI Foundry
10. ✅ Rehearse 10-minute demo flow

---

## References

- [Microsoft Agent Framework Documentation](https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview)
- [Microsoft 365 Agents SDK](https://learn.microsoft.com/en-us/microsoft-365/agents-sdk/agents-sdk-overview)
- [Azure AI Foundry Agents](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/overview)
- [AG-UI Protocol](https://docs.ag-ui.com/introduction)
- [CopilotKit Integration](https://docs.copilotkit.ai/microsoft-agent-framework)
- [Azure Developer CLI AI Agent Extension](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/extensions/azure-ai-foundry-extension)

**Version**: 1.0.0  
**Last Updated**: 2025-12-01
