# Azure Cognitive Services Account for AI Hub
# Reference: https://learn.microsoft.com/en-us/azure/cognitive-services/

variable "name" {
  description = "Required. The name of the Cognitive Services account. Must be unique within Azure."
  type        = string
  nullable    = false
}

variable "location" {
  description = "Required. The Azure region where the Cognitive Services account should be created."
  type        = string
  nullable    = false
}

variable "resource_group_name" {
  description = "Required. The name of the resource group in which to create the Cognitive Services account."
  type        = string
  nullable    = false
}

variable "kind" {
  description = "Required. Specifies the type of Cognitive Service account. For AI Hub, use 'CognitiveServices'."
  type        = string
  nullable    = false

  validation {
    condition     = contains(["CognitiveServices", "OpenAI", "ComputerVision", "TextAnalytics", "Speech"], var.kind)
    error_message = "The Cognitive Services kind must be one of: CognitiveServices, OpenAI, ComputerVision, TextAnalytics, Speech."
  }
}

variable "sku_name" {
  description = "Required. Specifies the SKU name of the Cognitive Services account. Common values: S0 (standard), F0 (free)."
  type        = string
  nullable    = false
  default     = "S0"

  validation {
    condition     = contains(["S0", "S1", "F0"], var.sku_name)
    error_message = "The SKU name must be one of: S0, S1, F0."
  }
}

variable "tags" {
  description = "Optional. A mapping of tags to assign to the resource."
  type        = map(string)
  default     = {}
}
