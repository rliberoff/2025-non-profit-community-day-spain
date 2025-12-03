# Azure Container Registry
# Reference: https://learn.microsoft.com/en-us/azure/container-registry/

variable "name" {
  description = "Required. The name of the Container Registry. Must be globally unique and alphanumeric only (no hyphens)."
  type        = string
  nullable    = false

  validation {
    condition     = can(regex("^[a-zA-Z0-9]+$", var.name))
    error_message = "Container Registry name must be alphanumeric only (no hyphens or special characters)."
  }
}

variable "location" {
  description = "Required. The Azure region where the Container Registry should be created."
  type        = string
  nullable    = false
}

variable "resource_group_name" {
  description = "Required. The name of the resource group in which to create the Container Registry."
  type        = string
  nullable    = false
}

variable "sku" {
  description = "Optional. The SKU name of the Container Registry. Possible values: Basic, Standard, Premium. Default is Basic."
  type        = string
  default     = "Basic"
  nullable    = false

  validation {
    condition     = contains(["Basic", "Standard", "Premium"], var.sku)
    error_message = "SKU must be one of: Basic, Standard, Premium."
  }
}

variable "admin_enabled" {
  description = "Optional. Specifies whether the admin user is enabled. Default is true."
  type        = bool
  default     = true
  nullable    = false
}

variable "tags" {
  description = "Optional. A mapping of tags to assign to the resource."
  type        = map(string)
  default     = {}
}
