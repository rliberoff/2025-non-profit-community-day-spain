# Outputs from the main Terraform configuration

# Resource Group
output "resource_group_name" {
  description = "Name of the Azure resource group"
  value       = azurerm_resource_group.rg.name
}

output "resource_group_location" {
  description = "Location of the Azure resource group"
  value       = azurerm_resource_group.rg.location
}

# Azure AI Foundry
output "ai_foundry_id" {
  description = "ID of the Azure AI Foundry workspace"
  value       = module.ai_foundry.foundry_id
}

output "ai_foundry_name" {
  description = "Name of the Azure AI Foundry workspace"
  value       = module.ai_foundry.foundry_name
}

output "ai_foundry_workspace_id" {
  description = "Immutable workspace ID of the Azure AI Foundry"
  value       = module.ai_foundry.foundry_workspace_id
}

output "ai_foundry_discovery_url" {
  description = "Discovery URL for the Azure AI Foundry"
  value       = module.ai_foundry.foundry_discovery_url
}

# Azure AI Foundry Project
output "ai_foundry_project_id" {
  description = "ID of the Azure AI Foundry Project"
  value       = module.ai_foundry.project_id
}

output "ai_foundry_project_name" {
  description = "Name of the Azure AI Foundry Project"
  value       = module.ai_foundry.project_name
}

output "ai_foundry_project_project_id" {
  description = "Immutable project ID of the Azure AI Foundry Project"
  value       = module.ai_foundry.project_project_id
}

# Azure AI Services
output "ai_services_id" {
  description = "ID of the Azure AI Services account"
  value       = module.ai_foundry.ai_services_id
}

output "ai_services_endpoint" {
  description = "Endpoint URL for Azure AI Services"
  value       = module.ai_foundry.ai_services_endpoint
}

# Application Insights
output "application_insights_connection_string" {
  description = "Connection string for Application Insights"
  value       = module.application_insights.connection_string
  sensitive   = true
}

output "application_insights_instrumentation_key" {
  description = "Instrumentation key for Application Insights"
  value       = module.application_insights.instrumentation_key
  sensitive   = true
}

# Log Analytics Workspace
output "log_analytics_workspace_id" {
  description = "ID of the Log Analytics Workspace"
  value       = module.log_analytics_workspace.workspace_id
}

# Container Registry
output "container_registry_name" {
  description = "Name of the Azure Container Registry"
  value       = module.container_registry.name
}

output "container_registry_login_server" {
  description = "Login server for the Azure Container Registry"
  value       = module.container_registry.login_server
}

# App Service
output "app_service_plan_id" {
  description = "ID of the App Service Plan"
  value       = module.app_service_plan.id
}

output "app_service_name" {
  description = "Name of the Linux Web App"
  value       = module.linux_web_app.name
}

output "app_service_url" {
  description = "URL of the deployed Fundraising Agent API"
  value       = "https://${module.linux_web_app.default_hostname}"
}

output "app_service_default_hostname" {
  description = "Default hostname of the Linux Web App"
  value       = module.linux_web_app.default_hostname
}

# Deployment instructions
output "deployment_instructions" {
  description = "Next steps for deploying the agent"
  value       = <<-EOT
    
    ✅ Azure infrastructure deployed successfully!
    
    Next steps:
    
    1. Build and push Docker image:
       cd src/AgentFundraising
       docker build -t ${module.container_registry.login_server}/fundraising-agent:latest .
       az acr login --name ${module.container_registry.name}
       docker push ${module.container_registry.login_server}/fundraising-agent:latest
    
    2. Update App Service to use the image:
       az webapp config container set \
         --name ${module.linux_web_app.name} \
         --resource-group ${azurerm_resource_group.rg.name} \
         --docker-custom-image-name ${module.container_registry.login_server}/fundraising-agent:latest \
         --docker-registry-server-url https://${module.container_registry.login_server}
    
    3. Test the agent:
       curl https://${module.linux_web_app.default_hostname}/health
    
    4. Access the API:
       https://${module.linux_web_app.default_hostname}/agent/chat
    
  EOT
}
