resource "azurerm_ai_services" "this" {
  custom_subdomain_name              = var.foundry_name
  fqdns                              = []
  local_authentication_enabled       = true
  location                           = var.location
  name                               = var.foundry_name
  outbound_network_access_restricted = false
  primary_access_key                 = "" # Masked sensitive attribute
  public_network_access              = "Enabled"
  resource_group_name                = var.resource_group_name
  sku_name                           = var.sku_name
  tags                               = {}
  identity {
    identity_ids = []
    type         = "SystemAssigned"
  }
  network_acls {
    bypass         = ""
    default_action = "Allow"
    ip_rules       = []
  }
}

resource "azapi_resource" "project" {
  body = {
    properties = {
      description = "Default project created with the resource"
      displayName = "ai-proj-relv"
    }
  }
  ignore_casing             = false
  ignore_missing_property   = true
  ignore_null_property      = false
  location                  = "swedencentral"
  name                      = "ai-proj-relv"
  parent_id                 = azurerm_cognitive_account.this.id
  schema_validation_enabled = true
  type                      = "Microsoft.CognitiveServices/accounts/projects@2025-06-01"
  identity {
    identity_ids = []
    type         = "SystemAssigned"
  }
}