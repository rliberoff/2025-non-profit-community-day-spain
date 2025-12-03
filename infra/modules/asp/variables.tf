# Azure App Service Plan (Service Plan)
# Reference: https://learn.microsoft.com/en-us/azure/app-service/overview-hosting-plans

variable "name" {
  description = "Required. The name of the App Service Plan."
  type        = string
  nullable    = false
}

variable "location" {
  description = "Required. The Azure region where the App Service Plan should be created."
  type        = string
  nullable    = false
}

variable "resource_group_name" {
  description = "Required. The name of the resource group in which to create the App Service Plan."
  type        = string
  nullable    = false
}

variable "os_type" {
  description = "Required. The OS type for the App Service Plan. Possible values: Linux, Windows."
  type        = string
  nullable    = false

  validation {
    condition     = contains(["Linux", "Windows"], var.os_type)
    error_message = "OS type must be either Linux or Windows."
  }
}

variable "sku_name" {
  description = "Required. The SKU for the plan. Common values: B1 (Basic), S1 (Standard), P1V2 (Premium V2), P1V3 (Premium V3)."
  type        = string
  nullable    = false

  validation {
    condition     = can(regex("^(B[1-3]|S[1-3]|P[1-3]V[2-3]|F1|D1|Y1)$", var.sku_name))
    error_message = "SKU name must be a valid App Service Plan SKU (e.g., B1, S1, P1V2, P1V3)."
  }
}

variable "tags" {
  description = "Optional. A mapping of tags to assign to the resource."
  type        = map(string)
  default     = {}
}
