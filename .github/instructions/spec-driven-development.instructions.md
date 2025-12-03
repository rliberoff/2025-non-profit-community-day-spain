---
applyTo: '**'
description: 'Enforces specification-first development: all implementation, tests, automation, and documentation must derive from clear, complete, and testable specifications authored before any code is produced.'
---

# Spec-Driven Development (SDD): Specifications Before Code Instructions

All work in this repository **must begin with a clear, comprehensive, and testable specification** before any implementation, automation, testing, or documentation is created.  

The specification acts as the **single source of truth** for requirements, behavior, interfaces, constraints, and acceptance criteria, and all code or AI-generated artifacts must follow it.

## 1. Context

Spec-Driven Development (SDD) ensures alignment between requirements, design, AI-generated code, tests, and documentation by mandating that **specifications precede implementation** and remain the authoritative reference.

This process applies to:

- All programming languages, frameworks, platforms, and tools  
- All workflows (features, bug fixes, refactors, design changes, APIs, data modeling, infrastructure, automation, CI/CD, etc.)  
- Both manually written code and AI-generated code (e.g., GitHub Copilot)  

Specifications guide decisions, prevent ambiguity, and maintain traceability across the lifecycle.

## 2. Objective

Ensure that all implementation artifacts (code, tests, configs, docs, pipelines) are:

- Derived from an explicit, approved, and maintained specification  
- Consistent with the documented intended behavior  
- Traceable back to the specification sections or identifiers  
- Updated only when the specification changes  

## 3. Core Instructions

### 3.1. Specification Before Implementation

No implementation—code, tests, automation, or docs—should begin without a corresponding **approved specification** describing intended behavior, scope, constraints, and acceptance criteria.

### 3.2. Specification as the Source of Truth

The spec must govern:

- System behavior  
- Interfaces and data contracts  
- Edge cases and constraints  
- Acceptance criteria  
- Tests and automation  

All implementation must remain consistent with the spec.

### 3.3. Include Concrete Acceptance Criteria

Every specification must include **measurable, testable criteria**.  
These criteria drive test creation, validation, and automation.

### 3.4. Keep Specs Technology-Agnostic

The spec must describe **what** the system does, not **how** it is implemented.  

Avoid references to frameworks, libraries, or languages unless necessary.

### 3.5. Decompose Work From the Spec

Break specifications into traceable tasks, test cases, and implementation steps.  

Reference the spec ID and relevant sections in commits, PRs, tests, and automation.

### 3.6. Implement and Test Against the Spec

All implementation and test work must follow the specification and map back to acceptance criteria.

When implementation learning reveals new insights, update the specification first, then the code.

### 3.7. Iterate Specification and Implementation Together

Specs are living documents. They evolve with the system, but **changes must be made to the spec before altering the implementation**.

### 3.8. Ensure Traceability and Reviewability

All code, commits, PRs, and automated test suites must reference their originating specification and acceptance criteria.

### 3.9. Use Specifications to Drive AI and Tooling

When generating code or tests via AI tools (e.g., GitHub Copilot), provide the specification as the primary input to ensure faithful adherence.

### 3.10. Reject Unspecced Changes

No new behavior, significant change, or breaking modification may be accepted without an accompanying specification update.

## 4. Specification Structure

Each specification must include:

### 4.1. Overview

- Purpose  
- Scope (included/excluded)  
- Context and system fit  

### 4.2. Requirements

- Functional Requirements (FR-###)  
- Non-Functional Requirements (NFR-###)  
- Constraints (technical, regulatory, business)  

### 4.3. Interface Definitions

- Inputs & outputs  
- Contracts (pre-conditions, post-conditions, invariants)  
- Error handling and failure modes  

### 4.4. Behavior Specifications

- Scenarios  
- Edge cases  
- State transitions  
- Dependencies  

### 4.5. Acceptance Criteria

- Precise, testable, measurable conditions  
- Success metrics  
- Validation methods  

### 4.6. Examples and Illustrations

- Use cases  
- Sample data  
- Activity/sequence/state diagrams  

### 4.7. Metadata

- Spec ID  
- Version  
- Authors  
- Status (Draft, Approved, Implemented, Deprecated)  
- Related specifications  

## 5. Technology-Specific Guidance

### 5.1. General Purpose Languages (C#, JS/TS, Java, Python, Go, Rust, etc.)

- Define module/class/function behavior  
- Document input/output types and constraints
- Specs stored in markdown near source code  
- Reference spec IDs in code comments and tests  
- Derive test cases from scenario definitions  

### 5.2. API Development (REST, GraphQL, gRPC)

- Specs may include OpenAPI, GraphQL SDL, or protobuf  
- Behavior and edge cases documented in spec  
- Validate request/response conformance during tests  

### 5.3. Databases & Data Modeling

- Define schemas, constraints, and integrity rules  
- Document expected query performance requirements  
- Specify migration behavior and backward compatibility  

### 5.4. Frontend

- Document UX flows, component behavior, accessibility rules  
- Include loading/error/edge UI states  
- Reference design system or visual spec as needed  

### 5.5. Infrastructure, DevOps & CI/CD

- Document topology, resource constraints, env-specific behaviors  
- Specify security, observability, DR, and backup rules  
- Describe deployment and rollback strategies  

### 5.6. Testing & QA

- Derive test cases from acceptance criteria  
- Map tests to specific spec requirements  
- Use spec scenarios for integration, performance, and security tests  

## 6. Workflow Integration

### 6.1. Before Starting Work

1. Verify if a spec exists  
2. Create or update the spec if not  
3. Ensure acceptance criteria are clear  
4. Have spec reviewed and approved  

### 6.2. During Implementation

1. Reference spec ID in commits and PRs  
2. Implement exactly what is defined  
3. Write tests mapping directly to acceptance criteria  
4. Document assumptions and discoveries in the spec  

### 6.3. After Implementation

1. Validate implementation against all acceptance criteria  
2. Update spec status to “Implemented”  
3. Link implementation artifacts to the spec  
4. Conduct review explicitly checking spec conformance  

### 6.7. When Requirements Change

1. Update the specification first  
2. Review the updated spec  
3. Update related specs  
4. Adjust implementation accordingly  

## 7. Specification Formats

- Markdown narrative specifications  
- Formal specifications (mathematical/rigorous)  
- Gherkin (Given/When/Then) for behavior  
- OpenAPI / JSON Schema / GraphQL SDL / Protobuf  
- Architecture Decision Records (ADRs)  
- RFC-style documents for major proposals  

## 8. Expected Results

- Every feature or change has a pre-existing specification  
- Specs are clear, testable, complete, and up-to-date  
- Implementation faithfully mirrors documented behavior  
- Full traceability between specs, implementation, tests, and automation  
- Code reviews have objective criteria  
- AI-generated outputs align with the specification  
- The specification becomes durable, reliable project documentation  

## 9. Enforcement

Compliance is enforced via:

- Pre-implementation specification reviews  
- Code reviews validating spec conformance  
- Automated tests derived from acceptance criteria  
- Documentation audits  
- Traceability checks  

Common anti-patterns to reject:

- Implementation without specification  
- Vague or untestable specifications  
- Code changes without spec updates  
- Specs prescribing implementation details  
- Missing or ambiguous acceptance criteria  
- Broken traceability  

## 10. Applicability

**IMPORTANT**: These rules apply to **all contributors**, **all changes** (features, fixes, refactors), and **all generation workflows**—manual or AI. No merges or releases may occur without a corresponding specification and traceability.

**NOTE**: If a change cannot be fully specified initially, document the current understanding, mark open questions, and refine iteratively—**but do not allow code to drift from the spec**.
