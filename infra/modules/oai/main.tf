# Current Azure client configuration
data "azurerm_client_config" "current" {}

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
