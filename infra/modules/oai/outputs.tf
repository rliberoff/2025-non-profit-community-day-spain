# # Azure AI Foundry and Project Outputs

# # Foundry Outputs
# output "foundry_id" {
#   description = "The ID of the Azure AI Foundry workspace."
#   value       = azurerm_ai_foundry.foundry.id
# }

# output "foundry_name" {
#   description = "The name of the Azure AI Foundry workspace."
#   value       = azurerm_ai_foundry.foundry.name
# }

# output "foundry_workspace_id" {
#   description = "The immutable workspace ID of the Azure AI Foundry."
#   value       = azurerm_ai_foundry.foundry.workspace_id
# }

# output "foundry_discovery_url" {
#   description = "The discovery URL for the Azure AI Foundry."
#   value       = azurerm_ai_foundry.foundry.discovery_url
# }

# output "foundry_identity_principal_id" {
#   description = "The Principal ID of the Foundry's system assigned identity."
#   value       = azurerm_ai_foundry.foundry.identity[0].principal_id
# }

# output "foundry_identity_tenant_id" {
#   description = "The Tenant ID of the Foundry's system assigned identity."
#   value       = azurerm_ai_foundry.foundry.identity[0].tenant_id
# }

# # Project Outputs
# output "project_id" {
#   description = "The ID of the Azure AI Foundry Project."
#   value       = azurerm_ai_foundry_project.project.id
# }

# output "project_name" {
#   description = "The name of the Azure AI Foundry Project."
#   value       = azurerm_ai_foundry_project.project.name
# }

# output "project_project_id" {
#   description = "The immutable project ID of the Azure AI Foundry Project."
#   value       = azurerm_ai_foundry_project.project.project_id
# }

# output "project_identity_principal_id" {
#   description = "The Principal ID of the Project's system assigned identity."
#   value       = azurerm_ai_foundry_project.project.identity[0].principal_id
# }

# output "project_identity_tenant_id" {
#   description = "The Tenant ID of the Project's system assigned identity."
#   value       = azurerm_ai_foundry_project.project.identity[0].tenant_id
# }

# # AI Services Outputs
# output "ai_services_id" {
#   description = "The ID of the Azure AI Services account."
#   value       = azurerm_cognitive_account.ai_services.id
# }

# output "ai_services_endpoint" {
#   description = "The endpoint of the Azure AI Services account."
#   value       = azurerm_cognitive_account.ai_services.endpoint
# }

# output "ai_services_primary_access_key" {
#   description = "The primary access key for the Azure AI Services account."
#   value       = azurerm_cognitive_account.ai_services.primary_access_key
#   sensitive   = true
# }

# # Supporting Resources Outputs
# output "storage_account_id" {
#   description = "The ID of the Storage Account used by Azure AI Foundry."
#   value       = azurerm_storage_account.foundry.id
# }

# output "key_vault_id" {
#   description = "The ID of the Key Vault used by Azure AI Foundry."
#   value       = azurerm_key_vault.foundry.id
# }
