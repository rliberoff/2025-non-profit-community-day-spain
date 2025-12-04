# Main Terraform configuration for Fundraising Copilot Agent
# Orchestrates Azure AI Foundry and App Service deployment

# Random suffix for unique resource names
resource "random_id" "suffix" {
  byte_length = 4
}

# Locals for naming and tagging
locals {
  suffix = lower(trimspace(var.use_random_suffix ? substr(lower(random_id.suffix.hex), 0, 6) : var.suffix))

  # Centralized tags
  tags = merge(var.tags, {
    Environment = var.environment
    Project     = var.project_name
    Owner       = var.owner
    CreatedAt   = formatdate("YYYY-MM-DD hh:mm:ss", timestamp())
    CreatedWith = "Terraform"
    Suffix      = local.suffix
  })

  # Resource names following Azure naming conventions
  resource_group_name = "rg-${var.project_name}-${var.environment}-${local.suffix}"
}

# Resource Group for AI resources
resource "azurerm_resource_group" "rg" {
  name     = local.resource_group_name
  location = var.location

  tags = var.tags

  lifecycle {
    ignore_changes = [tags]
  }
}

# Log Analytics Workspace for centralized logging
module "log_analytics_workspace" {
  source = "./modules/log"

  name                = "log-${var.project_name}-${var.environment}-${local.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
  tags                = local.tags
}

# Application Insights for monitoring
module "application_insights" {
  source = "./modules/appi"

  name                = "appi-${var.project_name}-${var.environment}-${local.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
  workspace_id        = module.log_analytics_workspace.id
  tags                = local.tags

  depends_on = [module.log_analytics_workspace]
}

# Azure AI Foundry with Project for OpenAI models
module "ai_foundry" {
  source = "./modules/oai"

  foundry_name         = "aifndry-${var.project_name}-${var.environment}-${local.suffix}"
  storage_account_name = "${var.project_name}${var.environment}"
  key_vault_name       = "${var.project_name}-${var.environment}"
  suffix               = local.suffix
  project_name         = "aiproj-${var.project_name}-${var.environment}-${local.suffix}"
  ai_services_name     = "oai-${var.project_name}-${var.environment}-${local.suffix}"
  location             = var.location
  resource_group_name  = azurerm_resource_group.rg.name
  sku_name             = "S0"

  # Optional: Application Insights for monitoring
  application_insights_id = module.application_insights.id

  # Optional: Container Registry for custom images
  container_registry_id = module.container_registry.id

  # Optional: Friendly names and descriptions
  foundry_friendly_name = "Fundraising Agent AI Foundry - ${var.environment}"
  foundry_description   = "Azure AI Foundry workspace for Fundraising Copilot Agent with OpenAI models"
  project_friendly_name = "Fundraising Agent Project - ${var.environment}"
  project_description   = "Azure AI Foundry Project for OpenAI model deployment"

  tags = local.tags

  depends_on = [
    module.application_insights,
    module.container_registry
  ]
}

# Azure Container Registry for Docker images
module "container_registry" {
  source = "./modules/acr"

  name                = lower(replace("acr${var.project_name}${var.environment}${local.suffix}", "-", ""))
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Basic"
  admin_enabled       = true
  tags                = local.tags
}

# App Service Plan for hosting
module "app_service_plan" {
  source = "./modules/asp"

  name                = "plan-${var.project_name}-${var.environment}-${local.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = var.app_service_sku
  tags                = local.tags
}

# Linux Web App for the Fundraising Agent
module "linux_web_app" {
  source = "./modules/app"

  name                = "app-${var.project_name}-${var.environment}-${local.suffix}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = module.app_service_plan.id
  https_only          = true

  # Site configuration
  always_on           = true
  http2_enabled       = true
  minimum_tls_version = "1.2"
  ftps_state          = "Disabled"

  # Docker configuration
  docker_image_name   = "mcr.microsoft.com/dotnet/aspnet:10.0"
  docker_registry_url = "https://mcr.microsoft.com"

  # Health check
  health_check_path                 = "/health"
  health_check_eviction_time_in_min = 10

  # Application settings
  app_settings = {
    # Azure AI Foundry configuration
    "AzureAI__FoundryId"    = module.ai_foundry.foundry_id
    "AzureAI__ProjectId"    = module.ai_foundry.project_id
    "AzureAI__Endpoint"     = module.ai_foundry.ai_services_endpoint
    "AzureAI__DiscoveryUrl" = module.ai_foundry.foundry_discovery_url

    # Application Insights
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = module.application_insights.connection_string

    # Container configuration
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "WEBSITES_PORT"                       = "8080"

    # Logging
    "Logging__LogLevel__Default"              = "Information"
    "Logging__LogLevel__Microsoft.AspNetCore" = "Warning"
    "Logging__LogLevel__Demo.Agent.Fundraising" = "Information"

    # CORS (if needed)
    "CORS__AllowedOrigins" = "*"
  }

  # Logging configuration
  enable_detailed_logging = true
  log_retention_in_days   = 7
  log_retention_in_mb     = 35
  application_log_level   = "Information"

  tags = local.tags

  depends_on = [
    module.app_service_plan,
    module.application_insights,
    module.ai_foundry
  ]
}

# Role assignment: Web App to Azure AI Services
resource "azurerm_role_assignment" "app_to_ai_services" {
  scope                = module.ai_foundry.ai_services_id
  role_definition_name = "Cognitive Services User"
  principal_id         = module.linux_web_app.identity_principal_id
}

# Role assignment: Web App to Azure AI Foundry
resource "azurerm_role_assignment" "app_to_ai_foundry" {
  scope                = module.ai_foundry.foundry_id
  role_definition_name = "AzureML Data Scientist"
  principal_id         = module.linux_web_app.identity_principal_id
}

# Role assignment: Web App to Azure AI Foundry Project
resource "azurerm_role_assignment" "app_to_ai_project" {
  scope                = module.ai_foundry.project_id
  role_definition_name = "AzureML Data Scientist"
  principal_id         = module.linux_web_app.identity_principal_id
}
