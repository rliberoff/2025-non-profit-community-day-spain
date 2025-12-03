<!--
SYNC IMPACT REPORT
==================
Version Change: 0.0.0 → 1.0.0
Ratification: 2025-12-01 (Initial constitution)

Modified Principles:
- NEW: All 6 core principles established for first time

Added Sections:
- Core Principles (6 principles)
- Technology Constraints
- Session Requirements
- Governance

Templates Status:
✅ plan-template.md - Constitution Check section aligned
✅ spec-template.md - User Stories & Requirements aligned
✅ tasks-template.md - Phased approach aligned with demonstration principles

Follow-up TODOs:
- None - all principles defined and validated
-->

# Non-Profit Community Day Spain 2025 Constitution

## Core Principles

### I. Demo-First Development

All code must serve as live demonstration material for a 45-minute session. Every component must be:

- Executable and demonstrable within the session timeframe
- Clear enough for live coding or explanation
- Focused on showing AI transformation of non-profit challenges
- Self-contained to avoid complex setup during presentation

**Rationale**: The primary purpose is education and demonstration, not production deployment. Code complexity must be balanced against teaching clarity.

### II. Microsoft Technology Stack (NON-NEGOTIABLE)

Only Microsoft technologies are permitted:

- **Agents**: Copilot Studio, Microsoft 365 Agents SDK and Microsoft Agent Framework
- **Cloud Platform**: Azure (Azure AI Foundry for advanced scenarios)
- **Integration**: Microsoft 365 services and APIs
- **NO** third-party agent frameworks (no LangChain, no OpenAI direct APIs, etc.)

**Rationale**: Session focus is on Microsoft's AI ecosystem. Mixing vendor solutions dilutes the message and creates integration complexity.

### III. Bilingual Specification-Code Separation

**Specifications**: MUST be written in English (international standard, tooling compatibility)  
**Code & Comments**: MUST be written in Spanish (audience language, teaching clarity)  
**Documentation for attendees**: Spanish

**Rationale**: Specifications serve as technical contracts and may be reviewed by international teams or AI systems expecting English. Code is the teaching material for Spanish-speaking attendees.

### IV. Three-Scenario Coverage

Demonstrations must address exactly three non-profit challenges:

1. **Captación de fondos** (Fundraising)
2. **Gestión de voluntariado** (Volunteer management)
3. **Atención a beneficiarios** (Beneficiary support)

Each scenario must show actionable AI solutions using agents.

**Rationale**: These three areas are universal pain points for non-profits. Focusing on them ensures relevance and avoids scope creep in a 45-minute session.

### V. Progressive Complexity (Copilot Studio → Azure AI Foundry)

Start simple, scale when justified:

- **Basic scenarios**: Copilot Studio agents (low-code/no-code approach)
- **Advanced needs**: Azure AI Foundry (custom models, advanced prompts)
- Each complexity jump **MUST** be justified by a clear scenario requirement

**Rationale**: Demonstrate the natural escalation path from citizen developer tools to professional AI engineering. Show when and why to adopt each approach.

### VI. Spec-Driven Development (Inherited from Repository Standards)

As mandated by `.github/instructions/spec-driven-development.instructions.md`:

- Specifications MUST be authored before implementation
- All code must trace back to specification requirements
- Acceptance criteria must be testable and measurable
- Changes to behavior require specification updates first

**Rationale**: Ensures consistency, reviewability, and AI-assisted code generation fidelity. Critical for demonstration preparation and reproducibility.

## Technology Constraints

### Permitted Technologies

- **Agent Platforms**: Copilot Studio, Microsoft 365 Agents SDK, Microsoft Agent Framework
- **Cloud Services**: Azure AI Foundry, Azure Functions, Azure OpenAI Service, Azure Storage
- **Integrations**: Microsoft Graph API, Microsoft 365 services (Teams, SharePoint, Outlook)
- **Languages**: C# (.NET) for any backend code
- **Documentation**: Markdown, Mermaid diagrams

### Prohibited Technologies

- Non-Microsoft agent frameworks (LangChain, LlamaIndex, Haystack, etc.)
- Non-Microsoft LLM APIs (direct OpenAI, Anthropic, Google, etc.)
- Non-Microsoft cloud platforms (AWS, GCP)

### Deployment Requirements

- Demos must run reliably in a live session environment
- Dependency on external services must have fallback plans
- Azure resources must be provisionable via Terraform templates (do not use ARM or Bicep).

## Session Requirements

### Time Constraints

- **Total Duration**: Maximum 45 minutes
- **Live Demo Time**: Reserve at least 20 minutes for live demonstrations
- **Setup Time**: Pre-configure all Azure resources; no live provisioning

### Audience Expectations

- **Language**: Session delivered in Spanish
- **Technical Level**: Mixed (some non-technical non-profit staff, some developers)
- **Takeaway**: Attendees should understand when to use Copilot Studio vs. Azure AI Foundry

### Demonstration Success Criteria

Each of the three scenarios must show:

1. The non-profit challenge (problem statement)
2. The AI agent solution (working demo)
3. The value delivered (metrics or outcomes)
4. Optional: How to scale the solution with Azure AI Foundry

## Governance

### Amendment Process

1. Proposed changes must be documented with rationale
2. Impact analysis on existing specifications and code required
3. Version bump follows semantic versioning:
    - **MAJOR**: Principle removal, technology stack change, scenario redefinition
    - **MINOR**: New principle added, scope expansion
    - **PATCH**: Clarifications, wording improvements

### Compliance Verification

- All specifications must reference applicable principles
- Code reviews verify adherence to technology constraints
- Bilingual separation enforced via PR checks (if automated)
- Demo rehearsals validate 45-minute constraint

### Priority Conflicts

When principles conflict, priority order:

1. Session time constraints (45 minutes)
2. Microsoft technology stack
3. Three-scenario coverage
4. Demo-first development
5. Progressive complexity
6. Bilingual separation

**Version**: 1.0.0 | **Ratified**: 2025-12-01 | **Last Amended**: 2025-12-01
