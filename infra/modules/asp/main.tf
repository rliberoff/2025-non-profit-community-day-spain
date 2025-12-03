# Azure App Service Plan
# Defines the compute resources for Azure App Service
# Reference: https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/service_plan

resource "azurerm_service_plan" "this" {
  name                = var.name
  location            = var.location
  resource_group_name = var.resource_group_name
  os_type             = var.os_type
  sku_name            = var.sku_name

  tags = var.tags

  lifecycle {
    ignore_changes = [tags]
  }
}
