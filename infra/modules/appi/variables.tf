# Azure Application Insights
# Reference: https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview

variable "name" {
  description = "Required. The name of the Application Insights component."
  type        = string
  nullable    = false
}

variable "location" {
  description = "Required. The Azure region where the Application Insights should be created."
  type        = string
  nullable    = false
}

variable "resource_group_name" {
  description = "Required. The name of the resource group in which to create Application Insights."
  type        = string
  nullable    = false
}

variable "application_type" {
  description = "Required. Specifies the type of Application Insights to create. Common values: web, other."
  type        = string
  nullable    = false
  default     = "web"

  validation {
    condition     = contains(["web", "other", "java", "ios", "phone", "store", "Node.JS"], var.application_type)
    error_message = "Application type must be one of: web, other, java, ios, phone, store, Node.JS."
  }
}

variable "workspace_id" {
  description = "Optional. The ID of the Log Analytics Workspace to associate with Application Insights."
  type        = string
  default     = null
}

variable "tags" {
  description = "Optional. A mapping of tags to assign to the resource."
  type        = map(string)
  default     = {}
}
