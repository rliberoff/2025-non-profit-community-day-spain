# Application Insights Outputs

output "id" {
  description = "The ID of the Application Insights component."
  value       = azurerm_application_insights.this.id
}

output "name" {
  description = "The name of the Application Insights component."
  value       = azurerm_application_insights.this.name
}

output "instrumentation_key" {
  description = "The instrumentation key for the Application Insights component."
  value       = azurerm_application_insights.this.instrumentation_key
  sensitive   = true
}

output "connection_string" {
  description = "The connection string for the Application Insights component."
  value       = azurerm_application_insights.this.connection_string
  sensitive   = true
}

output "app_id" {
  description = "The App ID associated with this Application Insights component."
  value       = azurerm_application_insights.this.app_id
}
