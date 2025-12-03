# Feature Specification: M365 Copilot Agent for Non-Profit Fundraising Management

**Feature Branch**: `001-fundraising-copilot-agent`  
**Created**: 2025-12-01  
**Status**: Ready for Planning  
**Input**: M365 Copilot agent for non-profit fundraising campaign management with Kanban board, donor tracking, and automated thank-you messages

## User Scenarios & Testing _(mandatory)_

### User Story 1 - Campaign Creation and Basic Tracking (Priority: P1)

A campaign coordinator needs to quickly set up a new fundraising campaign with a financial goal and track its progress. This is the foundation of the fundraising management system.

**Why this priority**: Without the ability to create and view campaigns, no other fundraising activities can be managed. This is the minimum viable feature that delivers immediate organizational value.

**Independent Test**: Can be fully tested by creating a campaign with name, goal amount, and viewing it in a campaign list. Delivers immediate value by providing a structured way to organize fundraising initiatives.

**Acceptance Scenarios**:

1. **Given** a coordinator opens the Copilot agent, **When** they request "Create a campaign called 'Navidad Solidaria' with a goal of 5,000 euros", **Then** a new campaign is created with that name and goal, and appears in the campaign list
2. **Given** multiple campaigns exist, **When** the coordinator views the campaign list, **Then** all campaigns are displayed with their names and goals visible
3. **Given** a campaign exists, **When** the coordinator selects it, **Then** the campaign details and associated task board are displayed

---

### User Story 2 - Task Organization with Kanban Board (Priority: P2)

Volunteers and coordinators need to organize fundraising activities into actionable tasks that can be tracked through completion stages. This helps the team visualize work in progress and ensures nothing falls through the cracks.

**Why this priority**: Once campaigns exist, teams need a way to break down work and track progress. This is essential for coordinating multi-person efforts but depends on campaigns being created first.

**Independent Test**: Can be tested by adding tasks to an existing campaign, moving them between workflow stages (To Do, In Progress, Completed), and verifying their positions are preserved. Delivers value by providing visual work management.

**Acceptance Scenarios**:

1. **Given** a campaign is selected, **When** the coordinator requests to add a task (e.g., "contact donors", "organize event", "send thank-you messages"), **Then** the task appears in the "To Do" column of the Kanban board
2. **Given** a task exists in "To Do", **When** a user moves it to "In Progress", **Then** the task appears in the "In Progress" column and is removed from "To Do"
3. **Given** a task exists in any column, **When** a user moves it to "Completed", **Then** the task appears in the "Completed" column
4. **Given** a task exists, **When** a user assigns it to a team member (coordinator or volunteer), **Then** the assignee name is displayed on the task card

---

### User Story 3 - Collaborative Task Notes (Priority: P3)

Team members need to add context, updates, or questions to specific tasks so everyone stays informed about progress and blockers.

**Why this priority**: While useful for team coordination, commenting is not essential for the basic workflow to function. Teams can coordinate externally if needed.

**Independent Test**: Can be tested by adding comments to existing tasks and verifying they are visible to all users viewing that task. Delivers value through enhanced team communication.

**Acceptance Scenarios**:

1. **Given** a task exists on the board, **When** a user adds a comment (e.g., "Called 10 donors today, 3 committed"), **Then** the comment appears on the task card
2. **Given** a task has comments, **When** another user views the task, **Then** all comments are visible in chronological order
3. **Given** a task has comments, **When** a user adds a new comment, **Then** it appears after existing comments

---

### User Story 4 - Automated Thank-You Message Generation (Priority: P2)

Coordinators need to send personalized thank-you messages to donors quickly after receiving contributions, but writing individual messages is time-consuming. AI-generated drafts save significant time while maintaining a personal touch.

**Why this priority**: This is a high-value AI capability that demonstrates clear time savings for non-profits. It depends on having campaigns defined but not on the Kanban board.

**Independent Test**: Can be tested by requesting a thank-you message for a donor and verifying it includes personalized elements (donor name, campaign name, contribution amount if provided). Delivers immediate value by automating a repetitive but important task.

**Acceptance Scenarios**:

1. **Given** a campaign exists, **When** the coordinator requests "Generate a thank-you email for donor María García who contributed 100 euros to Navidad Solidaria", **Then** a draft email is generated with:
    - Personalized greeting using donor name
    - Reference to specific campaign
    - Mention of contribution amount
    - Warm, professional tone appropriate for non-profit communication
2. **Given** a thank-you message is generated, **When** the coordinator reviews it, **Then** the message is in Spanish and suitable for direct use or minor editing
3. **Given** a campaign exists, **When** the coordinator requests a thank-you message without specifying contribution amount, **Then** a general thank-you message is generated referencing the campaign without mentioning specific amounts

---

### Edge Cases

-   What happens when a user tries to create a campaign with a zero or negative goal amount?
-   How does the system handle tasks assigned to users who are not in the predefined team (coordinator, volunteer 1, volunteer 2)?
-   What happens if a user tries to move a task to a non-existent column?
-   How does the system respond when asked to generate a thank-you message for a campaign that doesn't exist?
-   What happens when multiple users try to move the same task simultaneously?
-   How are extremely long campaign names, task names, or comments displayed in the interface?
-   What happens when a user requests to create a duplicate campaign name?

## Requirements _(mandatory)_

### Functional Requirements

#### Campaign Management

-   **FR-001**: System MUST allow users to create fundraising campaigns with a name and financial goal amount
-   **FR-002**: System MUST display a list of all created campaigns with their names and goals
-   **FR-003**: System MUST allow users to select a campaign to view its details and associated tasks
-   **FR-004**: System MUST support three predefined sample campaigns ("Navidad Solidaria", "Educación Rural", "Salud Comunitaria") pre-populated with 3-5 example tasks each

#### Task Management & Kanban Board

-   **FR-005**: System MUST provide a Kanban board view for each campaign with three columns: "To Do" (Por hacer), "In Progress" (En progreso), and "Completed" (Completado)
-   **FR-006**: System MUST allow users to add tasks to a campaign
-   **FR-007**: System MUST allow users to move tasks between workflow columns (To Do, In Progress, Completed) via menu selection or conversational commands
-   **FR-008**: System MUST display tasks within their current workflow column
-   **FR-009**: System MUST allow users to assign tasks to team members from a predefined list of three users: 1 campaign coordinator and 2 volunteers
-   **FR-010**: System MUST display the assignee name on task cards

#### Collaboration

-   **FR-011**: System MUST allow users to add text comments to tasks
-   **FR-012**: System MUST display all comments on a task in chronological order
-   **FR-013**: System MUST make comments visible to all users who can view the task

#### AI-Powered Message Generation

-   **FR-014**: System MUST generate personalized thank-you messages for donors that include:
    -   Donor name
    -   Campaign name
    -   Contribution amount (when provided)
    -   Warm, professional tone appropriate for Spanish-speaking non-profit communication, defined as:
        -   **Greeting formality**: Use "Estimado/a" or "Querido/a" (formal-friendly), avoid overly casual "Hola" or stiff "Distinguido/a"
        -   **Sentence structure**: 2-4 sentences total; first sentence expresses gratitude, middle sentence(s) explain impact, final sentence reinforces appreciation
        -   **Emotional words**: Include at least 2 of: agradecimiento, generoso/a, apoyo, impacto, esperanza, comunidad, transformar, posible
        -   **Tone examples**: "Queremos expresar nuestro más sincero agradecimiento", "Tu apoyo hace posible", "Gracias a personas como tú"
-   **FR-015**: System MUST generate all thank-you messages in Spanish
-   **FR-016**: System MUST generate thank-you messages suitable for direct use or minor editing
-   **FR-017**: System MUST handle requests for thank-you messages with or without contribution amounts specified
-   **FR-020**: System MUST accept donor information (name, contribution amount) as input parameters when generating thank-you messages (donors are NOT stored as persistent entities)

#### User Management

-   **FR-018**: System MUST support exactly three predefined users: 1 campaign coordinator and 2 volunteers
-   **FR-019**: System MUST display available team members for task assignment

### Key Entities

-   **Campaign**: Represents a fundraising initiative with a name and financial goal. Multiple campaigns can exist simultaneously. Each campaign has an associated set of tasks.

-   **Task**: Represents an actionable work item within a campaign. Has a title/description, current workflow status (To Do, In Progress, or Completed), an optional assignee (team member), and a collection of comments. Tasks belong to exactly one campaign.

-   **Team Member**: Represents a person who can be assigned tasks. Three predefined members exist: one with "coordinator" role and two with "volunteer" role. Team members are referenced when assigning tasks.

-   **Comment**: Represents a text note added to a task by a team member. Comments are ordered chronologically and associated with a specific task.

-   **Thank-You Message**: Represents an AI-generated draft communication to a donor. Contains personalized content referencing donor information, campaign details, and contribution amounts. Donor information is provided as input parameters at generation time and is not persisted.

## Success Criteria _(mandatory)_

### Measurable Outcomes

-   **SC-001**: Campaign coordinators can create a new fundraising campaign and view it in the campaign list in under 30 seconds
-   **SC-002**: Users can add a task to a campaign and move it through all three workflow stages (To Do → In Progress → Completed) in under 1 minute
-   **SC-003**: The system generates a complete, Spanish-language thank-you message suitable for donor communication in under 15 seconds after receiving the request
-   **SC-004**: ~~90% of generated thank-you messages require no editing or only minor edits before sending, as measured by user feedback during demo rehearsals~~ **OBSOLETE** - Not measurable in live demo context; qualitative assessment during rehearsal sufficient
-   **SC-005**: All three predefined users can be assigned to tasks without system errors
-   **SC-006**: Comments added to tasks are immediately visible to all users viewing that task (within 5 seconds)
-   **SC-007**: The complete demo workflow (create campaign, add tasks, move tasks, generate thank-you message) completes successfully within a 10-minute demonstration window
-   **SC-008**: System reliably handles the three sample campaigns (Navidad Solidaria, Educación Rural, Salud Comunitaria) with at least 5 tasks each without performance degradation
-   **SC-009**: Task assignments correctly display the assigned team member name on 100% of task cards
-   **SC-010**: Kanban board accurately reflects task positions after moves with zero data loss or incorrect placements

## Assumptions

1. **Demo Environment**: This feature is designed for live demonstration purposes during a 45-minute session, not for production deployment at scale
2. **Data Persistence**: Data (campaigns, tasks, comments) needs to persist only for the duration of the demonstration session; no long-term data retention is required
3. **User Authentication**: Since exactly three predefined users exist, a simple user selection mechanism is sufficient (no complex authentication system required)
4. **Concurrent Usage**: The system will be used by one demonstrator during the session, so complex concurrency controls are not required
5. **Language**: All user-facing content and AI-generated messages will be in Spanish to match the audience language
6. **Campaign Financial Tracking**: The system tracks campaign goals but does not need to track actual donations received or calculate progress percentages (that complexity exceeds demo scope)
7. **Task Complexity**: Tasks are simple text descriptions without subtasks, due dates, priorities, or other advanced project management features
8. **Comment Authorship**: Comments show content but do not need to track which specific user wrote them (reduces demo complexity)
9. **Donor Management**: Donors are not stored as persistent entities. Donor information (name, amount) is provided as input when generating thank-you messages
10. **Network Connectivity**: The demo environment has reliable internet access to Azure services
11. **Microsoft 365 Environment**: Attendees are assumed to have basic familiarity with Microsoft 365 interface conventions
12. **Sample Data**: Three campaigns (Navidad Solidaria, Educación Rural, Salud Comunitaria) are pre-loaded with example tasks to provide immediate visual content for demonstration
13. **Interaction Model**: Task management uses menu selection or conversational commands rather than drag-and-drop, aligning with Copilot Studio's strengths in conversational interfaces

## Dependencies

### External Dependencies

-   **Microsoft 365 Copilot Platform**: The agent must be built using M365 Copilot or Copilot Studio as mandated by constitution principle II
-   **Azure AI Services**: Required for AI-powered message generation capabilities
-   **Microsoft 365 Environment**: Users must have access to a Microsoft 365 environment to interact with the Copilot agent

### Internal Dependencies

-   None (this is the first feature in the repository)

## Constraints

### Technology Constraints (per Constitution)

-   **MUST** use Copilot Studio or Microsoft 365 Agents SDK (no other agent frameworks permitted)
-   **MUST** use Azure for any cloud services required
-   **MUST NOT** use third-party agent frameworks (LangChain, LlamaIndex, etc.)
-   **MUST NOT** use non-Microsoft LLM APIs directly

### Session Constraints

-   **Total demo time**: Maximum 10 minutes allocated within the 45-minute session (reserve time for two other scenarios)
-   **Setup time**: All configuration must be completed before the session; no live provisioning
-   **Reliability**: Demo must run successfully without depending on complex external services that could fail during live presentation

### Scope Constraints

-   **Fixed user count**: Exactly 3 users (1 coordinator, 2 volunteers) - no user management features
-   **Fixed workflow**: Exactly 3 Kanban columns - no customization
-   **Sample campaigns**: Focus on the three predefined campaigns (Navidad Solidaria, Educación Rural, Salud Comunitaria)
-   **No financial tracking**: Campaign goals are displayed but actual donation amounts received are not tracked
-   **No reporting**: No analytics, dashboards, or financial reports beyond the basic Kanban board view

## Design Decisions

### DD-001: Donor Management Approach

-   **Decision**: Donors are NOT stored as persistent entities. Thank-you message generation accepts donor information (name, contribution amount) as input parameters.
-   **Rationale**: Simplifies demo scope, reduces development time, and focuses on the AI message generation capability rather than donor database management. Aligns with demo-first development principle.

### DD-002: Sample Campaign Pre-population

-   **Decision**: Three sample campaigns (Navidad Solidaria, Educación Rural, Salud Comunitaria) are pre-populated with 3-5 example tasks each.
-   **Rationale**: Provides immediate visual content for demonstration, reduces live demo time and risk, allows presenter to focus on task manipulation and AI features rather than setup.

### DD-003: Kanban Board Interaction Model

-   **Decision**: Task movement uses menu selection or conversational commands (e.g., "Move task to In Progress") rather than drag-and-drop.
-   **Rationale**: Aligns with Copilot Studio's conversational interface strengths, more reliable for live demo, reduces technical complexity while maintaining full functionality.

## Constitution Compliance

This specification adheres to the following constitutional principles:

-   **Principle I (Demo-First Development)**: All features are designed for demonstration within the 10-minute allocated segment of the 45-minute session
-   **Principle II (Microsoft Technology Stack)**: Specifies use of Copilot Studio or M365 Agents SDK; no prohibited technologies
-   **Principle III (Bilingual Separation)**: This specification is written in English; implementation code and user-facing content will be in Spanish
-   **Principle IV (Three-Scenario Coverage)**: This specification addresses scenario 1 of 3 (Fundraising / Captación de fondos)
-   **Principle V (Progressive Complexity)**: Targets Copilot Studio as primary implementation (low-code approach suitable for this scenario's complexity)
-   **Principle VI (Spec-Driven Development)**: This specification is authored before implementation and defines testable acceptance criteria

**Version**: 1.0.0  
**Last Updated**: 2025-12-01
