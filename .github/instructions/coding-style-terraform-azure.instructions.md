---
applyTo: "**/*.tf,**/*.tfvars,**/*.tf.json"
description: "Coding style conventions for Terraform projects, specifically for Azure infrastructure. When generating Azure Terraform code, always follow these guidelines."
---

# Terraform for Azure Instructions

When generating Terraform code for Azure, always follow best practices and conventions specific to Azure infrastructure and Terraform (1.14+).

## General Guidelines

- Use the Azure knowledge MCP tools to ensure accuracy and best practices in Azure services.
- Use the Terraform MCP Server tool to generate the Terraform code to meet the refined requirements for Azure infrastructure.
- Refine all infrastructure requirements to be Azure-specific and aligned with best practices, security, and compliance standards. Be thorough and detailed in your analysis.

## General Rules

- Terraform should be written using HCL (HashiCorp Configuration Language) syntax.
- Use the latest Azure provider version compatible with the required resources.
  - Prefer `azurerm` over `azapi`.
  - Use `azapi` when what is required is not supported by `azurerm`.
  - Only use official or certified providers.
- Follow best practices for Terraform code structure, including the use of variables, outputs, and modules (more about this below).
- Ensure that the generated code is well-documented with comments explaining the purpose of each resource and configuration.
- Always try to use implicit dependencies over explicit dependencies where possible in Terraform.
- When generating Terraform resource names, ensure they are unique and descriptive, lower-case, and snake_case.
- Be sure to include any necessary provider configurations, backend settings, and required variables in the generated code.
- Ensure the generated terraform code always includes a top level `tags` variable map that is used on all taggable resources, with at least the following tags: `Environment`, `Project`, and `Owner`.
- Ensure that sensitive information such as passwords, API keys, and secrets are not hardcoded in the Terraform code. Use variables and values in `.tfvars` files instead.
- Do not assume any prior knowledge about the user's Azure environment; always seek clarification when in doubt.
- Do not ask for Azure specific information like instance types, instead focus on high level requirements and attempt to map them to Azure services for the user.
- Before finalizing the Terraform code, always confirm with the user that all requirements have been accurately captured and addressed.
- Never use `null_resource`, instead prefer `terraform_data`.
- Any sensitive information in output must be marked always as `sensitive = true`.
- Always use the latest version of Terraform unless a specific version is defined or requested.
- Always search in `https://registry.terraform.io/` for the latest version for providers.

## 1. Project Structure & Organization

### Modular Architecture

- **Separate modules by resource type**: Each Azure resource type has its own module directory (e.g., `modules/aca/`, `modules/kv/`, `modules/oai/`, `modules/cosmos/`, `modules/acr/`, etc.)
- **Consistent module structure**: Each module contains:
  - `main.tf` - Resource definitions
  - `variables.tf` - Input variables
  - `outputs.tf` - Output values, including sensitive information when required.
  - `providers.tf` - Provider configuration (when needed)

### Backend Configuration

- **Separate backend infrastructure**: Backend state storage (`backend/`) is isolated from main resources (`resources/`)
- **Remote state management**: Use Azure Storage Account for Terraform state with proper configuration

## 2. Naming Conventions

### Resource Naming

- **Consistent prefix pattern**: Use Azure resource abbreviations (e.g., `rg-`, `acr`, `appcs-`, `kv-`, `appi-`)
  - Reference: [Abbreviation recommendations for Azure resources](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-abbreviations)
- **Suffix strategy with locals**: Compute resource names with suffixes in locals block
- **Module folders or directory names**: Use kebab-case for module folder names using the same abbreviation strategy

  ```terraform
  locals {
    suffix = lower(trimspace(var.use_random_suffix ? substr(lower(random_id.random.hex), 1, 5) : var.suffix))
    name_resource_group = "${var.resource_group_name}-${local.suffix}"
    name_acr = "${var.acr_name}${local.suffix}"
  }
  ```

- **Random suffix option**: Provide flexibility between random and fixed suffixes via variables

### Variable Naming

- **Descriptive prefixes**: Group related variables by resource type (e.g., `aca_bot_*`, `appcs_*`, `openai_*`)
- **Underscores for readability**: Use snake_case for all identifiers

## 3. Variable Management

### Variable Documentation

- **Comprehensive descriptions**: Every variable includes:
  - Required/Optional status
  - Purpose and impact
  - Default values
  - External references (e.g., Azure documentation links)

### Variable Validation

- **Built-in validation blocks**: Validate input values at the variable level:

  ```terraform
  validation {
    condition     = contains(["free", "standard"], var.appcs_sku)
    error_message = "The Azure App Configuration SKU is incorrect. Possible values are `free` and `standard`."
  }
  ```

- **Regex validation**: Use `can(regex())` for pattern matching (e.g., suffix format validation)
- **Range validation**: Enforce numeric boundaries for values like retention days

### Variable Defaults

- **Sensible defaults**: Provide production-ready defaults (e.g., `location = "swedencentral"`)
- **Explicit nullability**: Use `nullable = false` to enforce required values

## 4. Tagging Strategy

### Consistent Tagging

- **Centralized tag management**: Define common tags in locals:

  ```terraform
  locals {
    tags = merge(var.tags, {
      environment = var.environment
      project     = var.project
      owner       = var.owner
      createdAt   = "${formatdate("YYYY-MM-DD hh:mm:ss", timestamp())} UTC"
      createdWith = "Terraform"
      suffix      = local.suffix
    })
  }
  ```

- **Automatic metadata**: Include creation timestamp and tool information
- **Tag inheritance**: Pass tags to all modules and resources

### Lifecycle Management

- **Ignore tag changes**: Prevent drift from external tag modifications:

  ```terraform
  lifecycle {
    ignore_changes = [tags]
  }
  ```

## 5. Resource Configuration

### Identity Management

- **User-assigned managed identities**: Prefer user-assigned over system-assigned for flexibility:

  ```terraform
  identity {
    type         = "UserAssigned"
    identity_ids = [var.identity_id]
  }
  ```

- **Consistent identity passing**: Pass managed identity to all resources requiring authentication

### Data Sources

- **Current configuration retrieval**: Use data sources for runtime information:

  ```terraform
  data "azurerm_client_config" "current" {}
  data "azurerm_subscription" "current" {}
  ```

### Dynamic Blocks

- **Conditional resource creation**: Use `dynamic` blocks for optional configurations:

  ```terraform
  dynamic "geo_location" {
    for_each = var.geo_locations
    content {
      location          = geo_location.value.location
      failover_priority = geo_location.value.failover_priority
    }
  }
  ```

## 6. Lifecycle Management

### Prevent Unwanted Updates

- **Selective ignore_changes**: Ignore attributes managed externally:

  ```terraform
  lifecycle {
    ignore_changes = [
      tags,
      template[0].container[0].image,
    ]
  }
  ```

### Trigger-Based Replacement

- **Content-based triggers**: Use `terraform_data` with SHA1 hashing to detect actual file changes:

  ```terraform
  resource "terraform_data" "trigger_update_bot_icon" {
    input = sha1(file_content)
  }

  lifecycle {
    replace_triggered_by = [terraform_data.trigger_update_bot_icon]
  }
  ```

## 7. Security Practices

### Access Control

- **Role-based access**: Define role assignments for managed identities:

  ```terraform
  resource "azurerm_role_assignment" "service_principals_role_assignment" {
    scope                = azurerm_cognitive_account.openai.id
    role_definition_name = "Cognitive Services OpenAI User"
    principal_id         = each.value
  }
  ```

### Secrets Management

- **No hardcoded secrets**: Use variables for sensitive values
- **When sensitive data or secrets are provided** as part of the development, put them in a `terraform.tfvars` file linked to their corresponding variable.
- **Key Vault integration**: Store secrets in Azure Key Vault with proper access policies
- **Name sanitization**: Replace unsupported characters (e.g., `:` to `--` for Key Vault secret names)

### Content Filtering (Azure OpenAI)

- **Responsible AI policies**: Configure content filters with appropriate severity thresholds
- **Ordered configuration**: Maintain consistent order in arrays to prevent unnecessary updates

## 8. Development Modes

### Environment-Specific Configuration

- **Development mode flag**: Use boolean flag for dev-specific configurations:

  ```terraform
  variable "development_mode" {
    description = "Specifies whether this resource should be created with configurations suitable for development purposes."
    type        = bool
    default     = false
  }
  ```

- **Conditional resource creation**: Use `count` with development mode for dev-only resources:

  ```terraform
  data "azurerm_client_config" "current" {
    count = var.development_mode ? 1 : 0
  }
  ```

## 9. Provider Configuration

### Version Pinning

- **Pessimistic version constraints**: Use `~>` for patch-level flexibility:

  ```terraform
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.2.0"
    }
  }
  ```

### Provider Features

- **Bug mitigation flags**: Document workarounds with references:

  ```terraform
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
  ```

## 10. Module Usage

### Dependency Management

- **Implicit dependencies**: Rely on and prefer Terraform's built-in dependency graph where possible.
- **Explicit dependencies**: Use `depends_on` when implicit dependencies aren't sufficient:

  ```terraform
  module "aca_bot_backend" {
    depends_on = [module.acr]
    # ... configuration
  }
  ```

### Module Reusability

- **Use `for_each` for multiple instances**: Create multiple similar resources efficiently:

  ```terraform
  resource "azurerm_cognitive_deployment" "openai" {
    for_each = { for model in var.models : model.id => model }
    # ... configuration
  }
  ```

## 11. Output Management

### Informative Outputs

- **Helper messages**: Include example commands in outputs:

  ```terraform
  output "terraform_init_backend" {
    description = "Shows an example of the Terraform command to initialize a deployment with this backend."
    value       = local.terraform_message
  }
  ```

- **Clear descriptions**: Every output has a descriptive explanation

## 12. Documentation & Comments

### Inline Documentation

- **Complex logic explanations**: Add detailed comments for non-obvious implementations
- **Bug workaround references**: Document known issues with GitHub/Azure issue links
- **Multi-line comment blocks**: Use for detailed explanations of lifecycle rules and special cases

### Code Comments

- **Purpose explanation**: Clarify why certain approaches are taken
- **External references**: Link to official documentation for validation

## 13. File Organization

### Separation of Concerns

- **Split variables by resource**: Use separate variable files (e.g., `variables-aca-ai-agent-chitchat.tf`)
- **Logical grouping**: Group related configurations together

### Regarding `tfvars` Files

- **Environment-specific values**: Use `.tfvars` files for environment configuration
- **Comments in tfvars**: Document non-obvious values directly in variable files

## 14. Testing & Validation

- **Terraform validate**: Always ensure generated code passes `terraform validate`
- **Plan review**: Generate and review `terraform plan` output for correctness before applying changes.
