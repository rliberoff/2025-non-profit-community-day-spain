# Tasks: M365 Copilot Agent for Non-Profit Fundraising Management

**Input**: Design documents from `/specs/001-fundraising-copilot-agent/`  
**Prerequisites**: plan.md, spec.md, data-model.md, contracts/function-tools.json, research.md, quickstart.md  
**Technology**: Microsoft Agent Framework (C# / .NET 10) + Azure AI Foundry + M365 Copilot Chat  
**Interface**: M365 Copilot Chat (primary), optional standalone frontend

**Tests**: Tests are NOT explicitly requested in the feature specification. Test tasks are EXCLUDED from this plan.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `- [ ] [ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3, US4)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic .NET structure

- [x] T001 Create .NET 10 solution structure with projects: src/AgenteRecaudacion/ and src/AgenteRecaudacion.Tests/
- [x] T002 Initialize AgenteRecaudacion.csproj with Microsoft Agent Framework dependencies (Microsoft.Agents.AI, Microsoft.Agents.AI.AzureAI.Persistent, Azure.AI.Projects, Azure.Identity, Azure.AI.OpenAI)
- [x] T003 [P] Configure launchSettings.json with local development ports and profiles in src/AgenteRecaudacion/Properties/
- [x] T004 [P] Create appsettings.json with AzureAI, AzureOpenAI, and Azure configuration sections in src/AgenteRecaudacion/
- [x] T005 [P] Create .gitignore entries for .NET artifacts, user secrets, and Azure credentials
- [x] T006 [P] Create README.md at repository root with project overview and quick start instructions

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T007 [P] Create Campaña model (record) with Id, Nombre, MetaEuros, FechaCreacion, Tareas in src/AgenteRecaudacion/Modelos/Campaña.cs
- [x] T008 [P] Create Tarea model (record) with Id, CampañaId, Descripcion, Columna, AsignadoA, Comentarios, FechaCreacion in src/AgenteRecaudacion/Modelos/Tarea.cs
- [x] T009 [P] Create Usuario model (record) with Id, Nombre, Rol in src/AgenteRecaudacion/Modelos/Usuario.cs
- [x] T010 [P] Create Comentario model (record) with Id, TareaId, Texto, FechaCreacion in src/AgenteRecaudacion/Modelos/Comentario.cs
- [x] T011 [P] Create MensajeAgradecimiento model (record) with NombreDonante, CampañaNombre, TextoGenerado, MontoEuros in src/AgenteRecaudacion/Modelos/MensajeAgradecimiento.cs
- [x] T012 [P] Create ColumnaKanban enum with PorHacer, EnProgreso, Completado values in src/AgenteRecaudacion/Modelos/ColumnaKanban.cs
- [x] T013 [P] Create RolUsuario enum with Coordinadora, Voluntario values in src/AgenteRecaudacion/Modelos/RolUsuario.cs
- [x] T014 Create EstadoAgente class with in-memory dictionaries for Campañas, Tareas, Comentarios, Usuarios in src/AgenteRecaudacion/Modelos/EstadoAgente.cs
- [x] T015 Create DatosEjemplo class with InicializarDatosEjemplo() method to pre-populate 3 campaigns (Navidad Solidaria, Educación Rural, Salud Comunitaria) with 3-5 tasks each in src/AgenteRecaudacion/Datos/DatosEjemplo.cs
- [x] T016 [P] Create ValidadorCampaña static class with ValidarCampaña() method in src/AgenteRecaudacion/Validadores/ValidadorCampaña.cs
- [x] T017 [P] Create ValidadorTarea static class with ValidarTarea() and ValidarMovimientoTarea() methods in src/AgenteRecaudacion/Validadores/ValidadorTarea.cs
- [x] T018 [P] Create ValidadorComentario static class with ValidarComentario() method in src/AgenteRecaudacion/Validadores/ValidadorComentario.cs
- [x] T019 Create Instrucciones.cs with Spanish system instructions for the agent in src/AgenteRecaudacion/Agente/Instrucciones.cs
- [x] T020 Create Program.cs with ASP.NET Core host configuration, dependency injection, and agent initialization in src/AgenteRecaudacion/Program.cs

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Campaign Creation and Basic Tracking (Priority: P1) 🎯 MVP

**Goal**: Enable campaign coordinators to create new fundraising campaigns with financial goals and view all campaigns in a list. This is the foundation of the fundraising management system.

**Independent Test**: Create a campaign with name "Campaña de Prueba" and goal 1000 euros via M365 Copilot Chat command "@AgenteRecaudación crea una campaña llamada 'Campaña de Prueba' con meta de 1000 euros". Verify campaign appears in list via "@AgenteRecaudación muéstrame todas las campañas". Campaign should display with correct name and goal.

### Implementation for User Story 1

- [x] T021 [P] [US1] Implement CrearCampaña [AIFunction] method with nombre and metaEuros parameters, validation, and EstadoAgente.Campañas dictionary insertion in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T022 [P] [US1] Implement ListarCampañas [AIFunction] method returning all campaigns from EstadoAgente.Campañas with id, nombre, meta_euros, num_tareas in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T023 [P] [US1] Implement ObtenerCampaña [AIFunction] method with campañaId parameter returning campaign details and associated tasks in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T024 [US1] Create AgenteRecaudacion class (ChatAgent) with dependency injection for EstadoAgente and registration of US1 function tools (CrearCampaña, ListarCampañas, ObtenerCampaña) in src/AgenteRecaudacion/Agente/AgenteRecaudacion.cs
- [x] T025 [US1] Configure Azure OpenAI client connection with GPT-4o-mini deployment in Program.cs
- [x] T026 [US1] Add logging for campaign creation, listing, and retrieval operations in Herramientas.cs

**Checkpoint**: At this point, User Story 1 should be fully functional. Test via Agents Playground or M365 Copilot Chat: create campaigns, list campaigns, view campaign details. This is the MVP - can be deployed independently.

---

## Phase 4: User Story 2 - Task Organization with Kanban Board (Priority: P2)

**Goal**: Enable team members to organize fundraising activities into actionable tasks and track them through completion stages (To Do, In Progress, Completed) on a Kanban board.

**Independent Test**: Add a task "Contactar donantes principales" to an existing campaign via "@AgenteRecaudación agrega una tarea 'Contactar donantes principales' a Navidad Solidaria". Move task to "En Progreso" via "@AgenteRecaudación mueve la tarea a en progreso". Assign to Ana García via "@AgenteRecaudación asigna la tarea a Ana García". Verify task position, assignment, and state via "@AgenteRecaudación muestra el tablero de Navidad Solidaria".

### Implementation for User Story 2

- [x] T027 [P] [US2] Implement AgregarTarea [AIFunction] method with campañaId, descripcion, asignadoA parameters, validation, and EstadoAgente.Tareas dictionary insertion with initial Columna=PorHacer in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T028 [P] [US2] Implement MoverTarea [AIFunction] method with tareaId and nuevaColumna parameters, validation, and Tarea.Columna update in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T029 [P] [US2] Implement AsignarTarea [AIFunction] method with tareaId and usuarioId parameters, validation, and Tarea.AsignadoA update in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T030 [P] [US2] Implement ObtenerTableroKanban [AIFunction] method with campañaId parameter returning tasks grouped by Columna (por_hacer, en_progreso, completado) in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T031 [P] [US2] Implement ListarUsuarios [AIFunction] method returning predefined users (Ana García, Carlos Ruiz, María López) from EstadoAgente.Usuarios in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T032 [US2] Register US2 function tools (AgregarTarea, MoverTarea, AsignarTarea, ObtenerTableroKanban, ListarUsuarios) in AgenteRecaudacion agent class in src/AgenteRecaudacion/Agente/AgenteRecaudacion.cs
- [x] T033 [US2] Add logging for task creation, movement, assignment, and Kanban board retrieval operations in Herramientas.cs

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently. Test creating campaigns (US1) and managing tasks on Kanban board (US2) without interference.

---

## Phase 5: User Story 3 - Collaborative Task Notes (Priority: P3)

**Goal**: Enable team members to add context, updates, or questions to specific tasks so everyone stays informed about progress and blockers.

**Independent Test**: Add a comment to an existing task via "@AgenteRecaudación agrega un comentario a la tarea 'Contactar donantes': 'Llamé a 10 donantes hoy, 3 confirmaron'". View comments via "@AgenteRecaudación muestra los comentarios de la tarea". Verify comment text and timestamp are visible.

### Implementation for User Story 3

- [x] T034 [P] [US3] Implement AgregarComentario [AIFunction] method with tareaId and texto parameters, validation, and EstadoAgente.Comentarios dictionary insertion with FechaCreacion=DateTime.UtcNow in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T035 [P] [US3] Implement ListarComentarios [AIFunction] method with tareaId parameter returning comments ordered by FechaCreacion ascending in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T036 [US3] Register US3 function tools (AgregarComentario, ListarComentarios) in AgenteRecaudacion agent class in src/AgenteRecaudacion/Agente/AgenteRecaudacion.cs
- [x] T037 [US3] Update ObtenerTableroKanban method to include comment count for each task in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T038 [US3] Add logging for comment addition and retrieval operations in Herramientas.cs

**Checkpoint**: All user stories 1, 2, and 3 should now be independently functional. Test campaign creation (US1), task management (US2), and commenting (US3) together.

---

## Phase 6: User Story 4 - Automated Thank-You Message Generation (Priority: P2)

**Goal**: Enable coordinators to generate personalized thank-you messages for donors quickly using AI, saving significant time while maintaining a personal touch.

**Independent Test**: Request a thank-you message via "@AgenteRecaudación genera un mensaje de agradecimiento para María García que donó 100 euros a Navidad Solidaria". Verify generated message includes: donor name (María García), campaign name (Navidad Solidaria), contribution amount (100 euros), warm professional tone in Spanish, and is suitable for direct use or minor editing.

### Implementation for User Story 4

- [x] T039 [US4] Implement GenerarAgradecimiento [AIFunction] method with nombreDonante, campañaNombre, montoEuros parameters that calls Azure OpenAI GPT-4o-mini with Spanish prompt template for thank-you message generation in src/AgenteRecaudacion/Agente/Herramientas.cs
- [x] T040 [US4] Create Spanish prompt template for thank-you message generation including instructions for: personalized greeting, campaign reference, amount mention (if provided), warm professional non-profit tone, full Spanish language in src/AgenteRecaudacion/Agente/Instrucciones.cs
- [x] T041 [US4] Register US4 function tool (GenerarAgradecimiento) in AgenteRecaudacion agent class in src/AgenteRecaudacion/Agente/AgenteRecaudacion.cs
- [x] T042 [US4] Add error handling for Azure OpenAI API failures with fallback to template-based message generation in Herramientas.cs GenerarAgradecimiento method
- [x] T043 [US4] Add logging for thank-you message generation requests and AI model responses in Herramientas.cs

**Checkpoint**: All user stories should now be independently functional. Test complete workflow: create campaign (US1), add tasks (US2), add comments (US3), generate thank-you messages (US4).

---

## Phase 7: Azure AI Foundry Integration

**Purpose**: Deploy agent to Azure AI Foundry for cloud-hosted persistent agent with managed threads

- [x] T044 Create Terraform configuration for Azure AI Foundry project resource in infra/modules/ai-foundry/main.tf
- [x] T045 Create Terraform configuration for Azure OpenAI resource with GPT-4o-mini deployment in infra/modules/ai-foundry/main.tf
- [x] T046 Create Terraform configuration for Azure App Service (Linux, .NET 10 container) for agent hosting in infra/modules/app-service/main.tf
- [x] T047 Create main Terraform configuration with module references and variable declarations in infra/main.tf
- [x] T048 Create terraform.tfvars with Azure Subscription ID , resource group name, location in infra/terraform.tfvars
- [x] T049 Configure Azure AI Foundry persistent agent integration in Program.cs with connection string from appsettings.json
- [x] T050 Update appsettings.json with AzureAI.ProjectEndpoint and AzureOpenAI.Endpoint placeholders for production deployment
- [x] T051 Create Dockerfile for .NET 10 containerization of AgenteRecaudacion in src/AgenteRecaudacion/Dockerfile
- [x] T052 Configure Azure identity authentication (DefaultAzureCredential) for production deployment in Program.cs
- [x] T053 Add health check endpoint /health returning agent status and Azure connection state in Program.cs

---

## Phase 8: M365 Copilot Chat Integration (Primary Interface)

**Purpose**: Enable direct agent access through M365 Copilot Chat for zero additional UI

- [x] T054 Create agent manifest (declarativeAgent.json) with agent name "Agente de Recaudación", description, capabilities, and function tool schemas in src/AgenteRecaudacion/
- [x] T055 Configure M365 Copilot agent registration with Azure AI Foundry agent endpoint in declarativeAgent.json
- [x] T056 Create Adaptive Card templates for campaign list visualization in src/AgenteRecaudacion/AdaptiveCards/ListaCampañas.json
- [x] T057 Create Adaptive Card templates for Kanban board visualization with three columns in src/AgenteRecaudacion/AdaptiveCards/TableroKanban.json
- [x] T058 Create Adaptive Card template for thank-you message display in src/AgenteRecaudacion/AdaptiveCards/MensajeAgradecimiento.json
- [x] T059 Update Herramientas.cs methods to return Adaptive Card JSON when called from M365 Copilot Chat (detect via user agent or request headers)
- [x] T060 Add M365 Copilot Chat specific instructions to Instrucciones.cs for handling @mentions and conversational context

---

## Phase 9: Optional Standalone Frontend (React + CopilotKit)

**Purpose**: Provide standalone web UI for demos outside M365 environment (OPTIONAL)

- [ ] T061 Initialize Next.js project with TypeScript and CopilotKit dependencies in frontend/
- [ ] T062 [P] Create ListaCampañas.tsx component displaying campaign cards with nombre and metaEuros in frontend/src/components/
- [ ] T063 [P] Create TableroKanban.tsx component with three columns (Por Hacer, En Progreso, Completado) in frontend/src/components/
- [ ] T064 [P] Create TarjetaTarea.tsx component displaying task descripcion, asignado, and comment count in frontend/src/components/
- [ ] T065 [P] Create SeccionComentarios.tsx component displaying task comments in chronological order in frontend/src/components/
- [ ] T066 [P] Create GeneradorAgradecimiento.tsx component with form inputs for donor name, campaign, amount and display area for generated message in frontend/src/components/
- [ ] T067 Create cliente_agente.ts service with AG-UI protocol client connecting to agent endpoint in frontend/src/servicios/
- [ ] T068 Create CopilotKit provider configuration with agent endpoint from environment variables in frontend/src/App.tsx
- [ ] T069 Create index.tsx home page with campaign list and navigation in frontend/src/pages/
- [ ] T070 Create campaña/[id].tsx dynamic page with Kanban board for selected campaign in frontend/src/pages/
- [ ] T071 Create Spanish translations file (es.json) with UI labels and messages in frontend/public/locales/
- [ ] T072 Configure environment variables in .env.local with NEXT_PUBLIC_AGENT_ENDPOINT in frontend/

---

## Phase 10: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories and deployment readiness

- [x] T073 [P] Create comprehensive quickstart validation script to verify all function tools in quickstart.md
- [x] T074 [P] Add OpenTelemetry instrumentation for agent observability across all Herramientas.cs methods
- [x] T075 [P] Add detailed Spanish XML documentation comments to all public methods in Herramientas.cs
- [x] T076 [P] Create GitHub Actions workflow for .NET build and test (ci-backend.yml) in .github/workflows/
- [x] T077 [P] Create GitHub Actions workflow for Terraform validation and Azure deployment (cd-azure.yml) in .github/workflows/
- [x] T078 Add rate limiting for Azure OpenAI calls in GenerarAgradecimiento method to prevent quota exhaustion
- [x] T079 Add comprehensive error messages in Spanish for all validation failures across ValidadorCampaña, ValidadorTarea, ValidadorComentario
- [x] T080 Create demo preparation checklist in quickstart.md with pre-demo validation steps
- [x] T081 Add demo flow script with exact M365 Copilot Chat commands in quickstart.md
- [x] T082 Performance optimization: cache predefined users in EstadoAgente initialization to avoid repeated lookups

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Story 1 (Phase 3)**: Depends on Foundational completion - No dependencies on other stories
- **User Story 2 (Phase 4)**: Depends on Foundational completion - Independent of US1 but naturally builds on campaigns
- **User Story 3 (Phase 5)**: Depends on Foundational completion - Independent of US1/US2 but requires tasks to exist
- **User Story 4 (Phase 6)**: Depends on Foundational completion - Independent of US1/US2/US3
- **Azure Integration (Phase 7)**: Depends on all desired user stories being complete
- **M365 Integration (Phase 8)**: Depends on Azure Integration (Phase 7)
- **Frontend (Phase 9)**: OPTIONAL - Depends on at least User Stories 1-2 being complete
- **Polish (Phase 10)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories - **MVP**
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - Operates on campaigns from US1 but independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Operates on tasks from US2 but independently testable
- **User Story 4 (P2)**: Can start after Foundational (Phase 2) - References campaigns from US1 but independently testable

### Within Each User Story

- Models and validators (Foundational Phase 2) before all function implementations
- Function tool implementations can proceed in parallel within each story
- Agent registration happens after all function tools for that story are implemented
- Logging added after core functionality is working

### Parallel Opportunities

**Setup Phase (Phase 1)**:

- T003 (launchSettings.json) || T004 (appsettings.json) || T005 (.gitignore) || T006 (README.md)

**Foundational Phase (Phase 2)**:

- T007 (Campaña) || T008 (Tarea) || T009 (Usuario) || T010 (Comentario) || T011 (MensajeAgradecimiento) || T012 (ColumnaKanban) || T013 (RolUsuario)
- T016 (ValidadorCampaña) || T017 (ValidadorTarea) || T018 (ValidadorComentario)

**User Story 1 (Phase 3)**:

- T021 (CrearCampaña) || T022 (ListarCampañas) || T023 (ObtenerCampaña)

**User Story 2 (Phase 4)**:

- T027 (AgregarTarea) || T028 (MoverTarea) || T029 (AsignarTarea) || T030 (ObtenerTableroKanban) || T031 (ListarUsuarios)

**User Story 3 (Phase 5)**:

- T034 (AgregarComentario) || T035 (ListarComentarios)

**Cross-Story Parallelism**:

- After Foundational Phase completes, US1, US2, US3, and US4 can ALL start in parallel with different team members

**Frontend Phase (Phase 9)**:

- T062 (ListaCampañas) || T063 (TableroKanban) || T064 (TarjetaTarea) || T065 (SeccionComentarios) || T066 (GeneradorAgradecimiento)

**Polish Phase (Phase 10)**:

- T073 (quickstart validation) || T074 (OpenTelemetry) || T075 (documentation) || T076 (CI) || T077 (CD)

---

## Parallel Example: User Story 1

```bash
# Launch all function tools for User Story 1 together:
Task T021: "Implement CrearCampaña [AIFunction] method in Herramientas.cs"
Task T022: "Implement ListarCampañas [AIFunction] method in Herramientas.cs"
Task T023: "Implement ObtenerCampaña [AIFunction] method in Herramientas.cs"

# Then proceed sequentially:
Task T024: "Create AgenteRecaudacion class and register US1 function tools"
Task T025: "Configure Azure OpenAI client connection"
Task T026: "Add logging for campaign operations"
```

---

## Parallel Example: All User Stories After Foundation

```bash
# After Phase 2 completes, launch all user stories in parallel:
Team Member A: Phase 3 (User Story 1) - T021 through T026
Team Member B: Phase 4 (User Story 2) - T027 through T033
Team Member C: Phase 5 (User Story 3) - T034 through T038
Team Member D: Phase 6 (User Story 4) - T039 through T043
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (T001-T006)
2. Complete Phase 2: Foundational (T007-T020) - **CRITICAL - blocks all stories**
3. Complete Phase 3: User Story 1 (T021-T026)
4. **STOP and VALIDATE**: Test campaign creation and listing independently via Agents Playground
5. Deploy/demo if ready - this is a functional MVP

### Incremental Delivery

1. Complete Setup (Phase 1) + Foundational (Phase 2) → Foundation ready
2. Add User Story 1 (Phase 3) → Test independently → Deploy/Demo (MVP! ✅)
3. Add User Story 2 (Phase 4) → Test independently → Deploy/Demo
4. Add User Story 4 (Phase 6) → Test independently → Deploy/Demo (high value AI feature)
5. Add User Story 3 (Phase 5) → Test independently → Deploy/Demo (nice-to-have)
6. Add Azure Integration (Phase 7) → Deploy to cloud
7. Add M365 Integration (Phase 8) → Enable Copilot Chat access
8. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup (Phase 1) + Foundational (Phase 2) together
2. Once Foundational is done:
   - Developer A: User Story 1 (T021-T026) - Campaign Management
   - Developer B: User Story 2 (T027-T033) - Task & Kanban Board
   - Developer C: User Story 4 (T039-T043) - AI Thank-You Messages
   - Developer D: User Story 3 (T034-T038) - Commenting
3. Stories complete and integrate independently

### Demo Preparation Strategy

1. Complete User Stories 1, 2, 4 (minimum for impressive demo)
2. Complete Phase 7 (Azure Integration)
3. Complete Phase 8 (M365 Copilot Chat Integration)
4. Skip Phase 9 (Frontend) if demoing directly in M365 Copilot Chat
5. Complete selected Phase 10 tasks: T080 (demo checklist), T081 (demo script)

---

## Task Summary

### Total Tasks: 82

**By Phase**:

- Phase 1 (Setup): 6 tasks
- Phase 2 (Foundational): 14 tasks (BLOCKS all user stories)
- Phase 3 (User Story 1 - P1): 6 tasks 🎯 MVP
- Phase 4 (User Story 2 - P2): 7 tasks
- Phase 5 (User Story 3 - P3): 5 tasks
- Phase 6 (User Story 4 - P2): 5 tasks
- Phase 7 (Azure Integration): 10 tasks
- Phase 8 (M365 Integration): 7 tasks
- Phase 9 (Frontend - OPTIONAL): 12 tasks
- Phase 10 (Polish): 10 tasks

**By User Story**:

- User Story 1 (Campaign Creation): 6 tasks
- User Story 2 (Task & Kanban Board): 7 tasks
- User Story 3 (Collaborative Comments): 5 tasks
- User Story 4 (AI Thank-You Messages): 5 tasks
- Infrastructure & Polish: 59 tasks

**Parallel Opportunities**: 35 tasks marked [P] can run in parallel with others

**Independent Test Criteria**:

- User Story 1: Create campaign, list campaigns, view details independently
- User Story 2: Add tasks, move through Kanban columns, assign users independently
- User Story 3: Add comments, view comments chronologically independently
- User Story 4: Generate personalized thank-you messages with AI independently

**Suggested MVP Scope**: Phase 1 + Phase 2 + Phase 3 (User Story 1 only) = 26 tasks

---

## Format Validation

✅ All tasks follow the checklist format: `- [ ] [TaskID] [P?] [Story?] Description with file path`  
✅ Sequential Task IDs: T001 through T082  
✅ [P] markers: 35 tasks marked as parallelizable  
✅ [Story] labels: US1, US2, US3, US4 labels applied to user story tasks  
✅ File paths: Included in all implementation task descriptions  
✅ Organized by user story: Clear phases for each user story with independent test criteria

---

**Version**: 1.0.0  
**Generated**: 2025-12-01  
**Branch**: `001-fundraising-copilot-agent`
