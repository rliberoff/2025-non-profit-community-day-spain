# Azure Infrastructure for Fundraising Copilot Agent

This directory contains the Terraform configuration for deploying the Fundraising Copilot Agent infrastructure on Azure.

## Architecture

The infrastructure follows a **modular architecture** where each Azure resource type has its own dedicated module. This approach ensures:

- **Separation of concerns**: Each resource is independently managed
- **Reusability**: Modules can be reused across different projects
- **Maintainability**: Changes to one resource don't affect others
- **Best practices compliance**: Aligns with Terraform and Azure coding standards

### Module Structure

#### AI & Monitoring Modules

1. **Cognitive Account** (`modules/ais/`)

   - Generic Azure Cognitive Services account
   - Used for AI Hub (multi-service cognitive services)
   - Abbreviation: AIS (AI Services)

2. **Azure OpenAI** (`modules/oai/`)

   - Azure OpenAI Service with GPT-4o-mini deployment
   - Manages both the cognitive account and model deployment
   - Abbreviation: OAI (OpenAI)

3. **Application Insights** (`modules/appi/`)

   - Application Performance Management (APM)
   - Monitoring and telemetry collection
   - Abbreviation: APPI (Application Insights)

4. **Log Analytics Workspace** (`modules/log/`)
   - Centralized logging and analytics
   - Supports Application Insights integration
   - Abbreviation: LOG (Log Analytics)

#### Hosting & Container Modules

1. **App Service Plan** (`modules/asp/`)

   - Linux-based hosting plan for containers
   - Defines compute resources for web apps
   - Abbreviation: ASP (App Service Plan)

2. **Container Registry** (`modules/acr/`)

   - Private Docker registry for agent images
   - System-assigned managed identity enabled
   - Abbreviation: ACR (Azure Container Registry)

3. **Linux Web App** (`modules/app/`)
   - Container-based web application hosting
   - Health checks and detailed logging enabled
   - Managed identity for secure service access
   - Abbreviation: APP (App Service)

## Prerequisites

- [Terraform](https://www.terraform.io/downloads.html) >= 1.14
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) >= 2.50
- Azure subscription with appropriate permissions

## Quick Start

1. **Login to Azure**:

   ```bash
   az login
   az account set --subscription <your-subscription-id>
   ```

2. **Initialize Terraform**:

   ```bash
   cd infra
   terraform init
   ```

3. **Review and customize variables** in `terraform.tfvars`:

   ```hcl
   project_name = "fundraising"
   environment  = "dev"
   location     = "swedencentral"
   # ... other variables
   ```

4. **Plan the deployment**:

   ```bash
   terraform plan
   ```

5. **Deploy the infrastructure**:

   ```bash
   terraform apply
   ```

6. **Follow the deployment instructions** from the Terraform output to build and deploy your agent container.

## Variables

Key variables you can configure in `terraform.tfvars`:

| Variable                | Description                          | Default           |
| ----------------------- | ------------------------------------ | ----------------- |
| `project_name`          | Project name prefix for resources    | `"fundraising"`   |
| `environment`           | Environment (dev, staging, prod)     | `"dev"`           |
| `location`              | Azure region                         | `"swedencentral"` |
| `use_random_suffix`     | Use random suffix for resource names | `true`            |
| `app_service_sku`       | App Service Plan SKU                 | `"B1"`            |
| `model_deployment_name` | Name for GPT-4o-mini deployment      | `"gpt-4o-mini"`   |
| `tags`                  | Additional tags for all resources    | `{}`              |

## Outputs

After successful deployment, Terraform provides:

- Resource names and IDs
- Service endpoints (AI Hub, OpenAI, App Service)
- Deployment instructions for the agent container
- Connection strings and access keys (marked as sensitive)

## Resource Naming Convention

Resources follow Azure's recommended naming conventions:

- Resource Group: `rg-{project}-{env}-{suffix}`
- AI Hub: `cog-{project}-aihub-{env}-{suffix}`
- OpenAI: `cog-{project}-oai-{env}-{suffix}`
- App Insights: `appi-{project}-{env}-{suffix}`
- Log Analytics: `log-{project}-{env}-{suffix}`
- App Service Plan: `plan-{project}-{env}-{suffix}`
- Container Registry: `acr{project}{env}{suffix}` (no hyphens)
- Web App: `app-{project}-{env}-{suffix}`

Reference: [Azure Resource Abbreviations](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-abbreviations)

## Clean Up

To destroy all resources:

```bash
terraform destroy
```

**Warning**: This will delete all resources including data. Ensure you have backups if needed.

## Module Structure

Infraestructure is organized as follows:

```text
infra/
├── main.tf                       # Main configuration and module orchestration
├── outputs.tf                    # Output definitions
├── providers.tf                  # Provider configuration (Azure, Random)
├── terraform.tfvars              # Variable values (not in git)
├── variables.tf                  # Input variable definitions
└── modules/                      # Modular resource definitions
    ├── acr/                      # Azure Container Registry (Docker image registry)
    │   ├── main.tf
    │   ├── outputs.tf
    │   └── variables.tf
    ├── ais/                      # Azure AI Services (Cognitive Account for AI Hub)
    │   ├── main.tf
    │   ├── outputs.tf
    │   └── variables.tf
    ├── app/                      # Linux Web App (web application hosting)
    │   ├── main.tf
    │   ├── outputs.tf
    │   └── variables.tf
    ├── appi/                     # Application Insights (APM and monitoring)
    │   ├── main.tf
    │   ├── outputs.tf
    │   └── variables.tf
    ├── asp/                      # App Service Plan (compute hosting plan)
    │   ├── main.tf
    │   ├── outputs.tf
    │   └── variables.tf
    ├── log/                      # Log Analytics Workspace (centralized logging)
    │   ├── main.tf
    │   ├── outputs.tf
    │   └── variables.tf
    └── oai/                      # Azure OpenAI with model deployment
        ├── main.tf
        ├── outputs.tf
        └── variables.tf
```

## Modular Design Principles

Each module follows these principles:

1. **Single Responsibility**: One module = one Azure resource type
2. **Consistent Structure**: All modules have `main.tf`, `variables.tf`, and `outputs.tf`
3. **Comprehensive Documentation**: Variables include descriptions and validation rules
4. **Sensitive Data Protection**: Outputs marked as sensitive where appropriate
5. **Lifecycle Management**: Tags ignored to prevent external drift
6. **Identity Management**: System-assigned managed identities by default

## Security Considerations

- All resources use system-assigned managed identities
- Web app enforces HTTPS only
- Minimum TLS version set to 1.2
- FTPS disabled
- Container registry uses admin authentication (consider using managed identity in production)
- Sensitive outputs (keys, connection strings) marked as sensitive
- Tags lifecycle ignored to prevent drift from external modifications
- Role-based access control (RBAC) for service-to-service communication

## Cost Optimization

Default configuration uses cost-effective SKUs:

- **App Service**: B1 (Basic) - ~$13/month
- **Azure OpenAI**: S0 with minimal capacity (10 TPM)
- **Container Registry**: Basic tier
- **Cognitive Services**: S0
- **Log Analytics**: PerGB2018 with 30-day retention

For production, consider:

- App Service: P1V2 or higher for better performance and SLA
- Container Registry: Standard or Premium for geo-replication and webhooks
- Increased OpenAI capacity based on usage patterns
- Extended log retention for compliance requirements

## Troubleshooting

### Common Issues

1. **Terraform state locked**: Wait for concurrent operations to complete or use `terraform force-unlock`
2. **Resource name conflicts**: Adjust `suffix` or enable `use_random_suffix`
3. **Quota exceeded**: Check Azure subscription quotas for the region
4. **Permission denied**: Ensure your Azure account has appropriate RBAC roles
5. **Module not found**: Run `terraform init` after adding or modifying modules

### Useful Commands

```bash
# Validate configuration
terraform validate

# Format code
terraform fmt -recursive

# Show current state
terraform show

# List resources
terraform state list

# Refresh state
terraform refresh

# Show module dependency graph
terraform graph | dot -Tpng > graph.png
```

## CI/CD Integration

This configuration can be integrated with GitHub Actions, Azure DevOps, or other CI/CD platforms. Ensure:

1. Service principal with appropriate permissions
2. State storage in Azure Storage Account (recommended)
3. Secret management for sensitive variables
4. Environment-specific `.tfvars` files
5. Automated testing with `terraform validate` and `terraform plan`

## Support

For issues or questions:

- Review the [Terraform Azure Provider documentation](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs)
- Check Azure service-specific documentation
- Review `.github/instructions/coding-style-terraform-azure.instructions.md` for coding standards
- Open an issue in the project repository
