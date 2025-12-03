# Input variables for Terraform configuration

variable "subscription_id" {
  description = "Required. Azure subscription ID where resources will be deployed."
  type        = string
  nullable    = false
}

variable "project_name" {
  description = "Required. Name of the project (used for resource naming). Must be lowercase alphanumeric."
  type        = string
  default     = "fundraising-agent"
  nullable    = false

  validation {
    condition     = can(regex("^[a-z0-9-]+$", var.project_name))
    error_message = "Project name must contain only lowercase letters, numbers, and hyphens."
  }
}

variable "environment" {
  description = "Required. Environment name (dev, staging, prod)."
  type        = string
  default     = "dev"
  nullable    = false

  validation {
    condition     = contains(["dev", "staging", "prod"], var.environment)
    error_message = "Environment must be one of: dev, staging, prod."
  }
}

variable "location" {
  description = "Required. Azure region for resources. See: https://azure.microsoft.com/en-us/explore/global-infrastructure/geographies/"
  type        = string
  default     = "westeurope"
  nullable    = false
}

variable "app_service_sku" {
  description = "Required. SKU for the App Service Plan. Common values: B1 (Basic), S1 (Standard), P1v2 (Premium v2)."
  type        = string
  default     = "B1"
  nullable    = false

  validation {
    condition     = can(regex("^(B[1-3]|S[1-3]|P[1-3]v[2-3]|I[1-3]v2)$", var.app_service_sku))
    error_message = "App Service SKU must be valid Azure SKU (e.g., B1, S1, P1v2)."
  }
}

variable "use_random_suffix" {
  description = "Optional. Use random suffix for resource names (true) or fixed suffix (false)."
  type        = bool
  default     = true
  nullable    = false
}

variable "suffix" {
  description = "Optional. Fixed suffix for resource names when use_random_suffix is false. Must be 6 characters max."
  type        = string
  default     = "001"
  nullable    = false

  validation {
    condition     = can(regex("^[a-z0-9]{1,6}$", var.suffix))
    error_message = "Suffix must be 1-6 lowercase alphanumeric characters."
  }
}

variable "owner" {
  description = "Required. Owner of the resources (email or name) for tagging purposes."
  type        = string
  default     = "DevOps Team"
  nullable    = false
}

variable "tags" {
  description = "Optional. Additional tags to apply to all resources. Merged with default tags."
  type        = map(string)
  default     = {}
  nullable    = false
}
