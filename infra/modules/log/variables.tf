# Azure Log Analytics Workspace
# Reference: https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-overview

variable "name" {
  description = "Required. The name of the Log Analytics Workspace."
  type        = string
  nullable    = false
}

variable "location" {
  description = "Required. The Azure region where the Log Analytics Workspace should be created."
  type        = string
  nullable    = false
}

variable "resource_group_name" {
  description = "Required. The name of the resource group in which to create the Log Analytics Workspace."
  type        = string
  nullable    = false
}

variable "sku" {
  description = "Optional. Specifies the SKU of the Log Analytics Workspace. Default is PerGB2018."
  type        = string
  default     = "PerGB2018"
  nullable    = false

  validation {
    condition     = contains(["Free", "PerNode", "Premium", "Standard", "Standalone", "Unlimited", "CapacityReservation", "PerGB2018"], var.sku)
    error_message = "SKU must be one of: Free, PerNode, Premium, Standard, Standalone, Unlimited, CapacityReservation, PerGB2018."
  }
}

variable "retention_in_days" {
  description = "Optional. The workspace data retention in days. Possible values range between 30 and 730. Default is 30."
  type        = number
  default     = 30
  nullable    = false

  validation {
    condition     = var.retention_in_days >= 30 && var.retention_in_days <= 730
    error_message = "Retention in days must be between 30 and 730."
  }
}

variable "tags" {
  description = "Optional. A mapping of tags to assign to the resource."
  type        = map(string)
  default     = {}
}
