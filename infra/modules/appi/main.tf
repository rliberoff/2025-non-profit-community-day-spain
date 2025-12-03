# Azure Application Insights
# Application Performance Management (APM) service for monitoring and diagnostics
# Reference: https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/application_insights

resource "azurerm_application_insights" "this" {
  name                = var.name
  location            = var.location
  resource_group_name = var.resource_group_name
  application_type    = var.application_type
  workspace_id        = var.workspace_id

  tags = var.tags

  lifecycle {
    ignore_changes = [tags]
  }
}
