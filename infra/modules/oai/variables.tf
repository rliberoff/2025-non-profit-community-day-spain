# Azure AI Foundry and Project Configuration
# Reference: https://learn.microsoft.com/en-us/azure/ai-foundry/how-to/create-hub-terraform

variable "foundry_name" {
  description = "Required. The name of the Azure AI Foundry workspace."
  type        = string
  nullable    = false
}

variable "location" {
  description = "Required. The Azure region where the resources should be created."
  type        = string
  nullable    = false
}

variable "resource_group_name" {
  description = "Required. The name of the resource group in which to create the resources."
  type        = string
  nullable    = false
}

variable "sku_name" {
  description = "Optional. Specifies the SKU name of the AI Services account. Typically S0 for standard tier."
  type        = string
  nullable    = false
  default     = "S0"

  validation {
    condition     = contains(["S0"], var.sku_name)
    error_message = "The SKU name for Azure AI Services must be S0."
  }
}

variable "project_name" {
  description = "Required. The name of the Azure AI Foundry Project."
  type        = string
  nullable    = false
}

# variable "ai_services_name" {
#   description = "Required. The name of the Azure AI Services account (required for Azure AI Foundry)."
#   type        = string
#   nullable    = false
# }





# variable "application_insights_id" {
#   description = "Optional. The ID of the Application Insights instance to use for monitoring."
#   type        = string
#   default     = null
# }



# variable "container_registry_id" {
#   description = "Optional. The ID of the Container Registry to use for custom images."
#   type        = string
#   default     = null
# }

# variable "foundry_friendly_name" {
#   description = "Optional. The friendly display name for the Azure AI Foundry workspace."
#   type        = string
#   default     = null
# }

# variable "foundry_description" {
#   description = "Optional. The description for the Azure AI Foundry workspace."
#   type        = string
#   default     = null
# }

# variable "project_friendly_name" {
#   description = "Optional. The friendly display name for the Azure AI Foundry Project."
#   type        = string
#   default     = null
# }

# variable "project_description" {
#   description = "Optional. The description for the Azure AI Foundry Project."
#   type        = string
#   default     = null
# }

variable "tags" {
  description = "Optional. A mapping of tags to assign to the resources."
  type        = map(string)
  default     = {}
}
