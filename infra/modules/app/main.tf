# Azure Linux Web App (App Service)
# Hosts containerized applications on Linux
# Reference: https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/linux_web_app

resource "azurerm_linux_web_app" "this" {
  name                = var.name
  location            = var.location
  resource_group_name = var.resource_group_name
  service_plan_id     = var.service_plan_id
  https_only          = var.https_only

  identity {
    type = "SystemAssigned"
  }

  site_config {
    always_on           = var.always_on
    http2_enabled       = var.http2_enabled
    minimum_tls_version = var.minimum_tls_version
    ftps_state          = var.ftps_state

    dynamic "application_stack" {
      for_each = var.docker_image_name != null ? [1] : []
      content {
        docker_image_name   = var.docker_image_name
        docker_registry_url = var.docker_registry_url
      }
    }

    health_check_path                 = var.health_check_path
    health_check_eviction_time_in_min = var.health_check_eviction_time_in_min
  }

  app_settings = var.app_settings

  logs {
    detailed_error_messages = var.enable_detailed_logging
    failed_request_tracing  = var.enable_detailed_logging

    http_logs {
      file_system {
        retention_in_days = var.log_retention_in_days
        retention_in_mb   = var.log_retention_in_mb
      }
    }

    application_logs {
      file_system_level = var.application_log_level
    }
  }

  tags = var.tags

  lifecycle {
    ignore_changes = [
      tags,
      site_config[0].application_stack[0].docker_image_name,
    ]
  }
}
