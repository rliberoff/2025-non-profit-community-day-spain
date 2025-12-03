# Cognitive Services Account Outputs

output "id" {
  description = "The ID of the Cognitive Services account."
  value       = azurerm_cognitive_account.this.id
}

output "name" {
  description = "The name of the Cognitive Services account."
  value       = azurerm_cognitive_account.this.name
}

output "endpoint" {
  description = "The endpoint used to connect to the Cognitive Services account."
  value       = azurerm_cognitive_account.this.endpoint
}

output "primary_access_key" {
  description = "The primary access key for the Cognitive Services account."
  value       = azurerm_cognitive_account.this.primary_access_key
  sensitive   = true
}

output "secondary_access_key" {
  description = "The secondary access key for the Cognitive Services account."
  value       = azurerm_cognitive_account.this.secondary_access_key
  sensitive   = true
}

output "identity_principal_id" {
  description = "The Principal ID of the system assigned identity."
  value       = azurerm_cognitive_account.this.identity[0].principal_id
}

output "identity_tenant_id" {
  description = "The Tenant ID of the system assigned identity."
  value       = azurerm_cognitive_account.this.identity[0].tenant_id
}
