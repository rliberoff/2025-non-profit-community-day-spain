# Container Registry Outputs

output "id" {
  description = "The ID of the Container Registry."
  value       = azurerm_container_registry.this.id
}

output "name" {
  description = "The name of the Container Registry."
  value       = azurerm_container_registry.this.name
}

output "login_server" {
  description = "The URL that can be used to log into the Container Registry."
  value       = azurerm_container_registry.this.login_server
}

output "admin_username" {
  description = "The Username associated with the Container Registry Admin account."
  value       = azurerm_container_registry.this.admin_username
  sensitive   = true
}

output "admin_password" {
  description = "The Password associated with the Container Registry Admin account."
  value       = azurerm_container_registry.this.admin_password
  sensitive   = true
}

output "identity_principal_id" {
  description = "The Principal ID of the system assigned identity."
  value       = azurerm_container_registry.this.identity[0].principal_id
}

output "identity_tenant_id" {
  description = "The Tenant ID of the system assigned identity."
  value       = azurerm_container_registry.this.identity[0].tenant_id
}
