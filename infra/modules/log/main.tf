# Azure Log Analytics Workspace
# Centralized logging and analytics for Azure resources
# Reference: https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/log_analytics_workspace

resource "azurerm_log_analytics_workspace" "this" {
  name                = var.name
  location            = var.location
  resource_group_name = var.resource_group_name
  sku                 = var.sku
  retention_in_days   = var.retention_in_days

  tags = var.tags

  lifecycle {
    ignore_changes = [tags]
  }
}
