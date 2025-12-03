# Linux Web App Outputs

output "id" {
  description = "The ID of the Linux Web App."
  value       = azurerm_linux_web_app.this.id
}

output "name" {
  description = "The name of the Linux Web App."
  value       = azurerm_linux_web_app.this.name
}

output "default_hostname" {
  description = "The default hostname of the Linux Web App."
  value       = azurerm_linux_web_app.this.default_hostname
}

output "outbound_ip_addresses" {
  description = "A comma separated list of outbound IP addresses."
  value       = azurerm_linux_web_app.this.outbound_ip_addresses
}

output "possible_outbound_ip_addresses" {
  description = "A comma separated list of possible outbound IP addresses."
  value       = azurerm_linux_web_app.this.possible_outbound_ip_addresses
}

output "identity_principal_id" {
  description = "The Principal ID of the system assigned identity."
  value       = azurerm_linux_web_app.this.identity[0].principal_id
}

output "identity_tenant_id" {
  description = "The Tenant ID of the system assigned identity."
  value       = azurerm_linux_web_app.this.identity[0].tenant_id
}
