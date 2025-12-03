# Azure Linux Web App (App Service)
# Reference: https://learn.microsoft.com/en-us/azure/app-service/

variable "name" {
  description = "Required. The name of the Linux Web App."
  type        = string
  nullable    = false
}

variable "location" {
  description = "Required. The Azure region where the Linux Web App should be created."
  type        = string
  nullable    = false
}

variable "resource_group_name" {
  description = "Required. The name of the resource group in which to create the Linux Web App."
  type        = string
  nullable    = false
}

variable "service_plan_id" {
  description = "Required. The ID of the App Service Plan within which to create this Linux Web App."
  type        = string
  nullable    = false
}

variable "https_only" {
  description = "Optional. Should the Linux Web App only be accessible via HTTPS? Default is true."
  type        = bool
  default     = true
  nullable    = false
}

variable "always_on" {
  description = "Optional. Should the app be loaded at all times? Default is true."
  type        = bool
  default     = true
  nullable    = false
}

variable "http2_enabled" {
  description = "Optional. Should HTTP2 be enabled? Default is true."
  type        = bool
  default     = true
  nullable    = false
}

variable "minimum_tls_version" {
  description = "Optional. The minimum supported TLS version. Default is 1.2."
  type        = string
  default     = "1.2"
  nullable    = false

  validation {
    condition     = contains(["1.0", "1.1", "1.2"], var.minimum_tls_version)
    error_message = "Minimum TLS version must be one of: 1.0, 1.1, 1.2."
  }
}

variable "ftps_state" {
  description = "Optional. State of FTP / FTPS service. Possible values: AllAllowed, FtpsOnly, Disabled. Default is Disabled."
  type        = string
  default     = "Disabled"
  nullable    = false

  validation {
    condition     = contains(["AllAllowed", "FtpsOnly", "Disabled"], var.ftps_state)
    error_message = "FTPS state must be one of: AllAllowed, FtpsOnly, Disabled."
  }
}

variable "docker_image_name" {
  description = "Optional. The Docker image name to use for the Linux Web App."
  type        = string
  default     = null
}

variable "docker_registry_url" {
  description = "Optional. The URL of the Docker registry."
  type        = string
  default     = null
}

variable "health_check_path" {
  description = "Optional. The path to use for the health check."
  type        = string
  default     = null
}

variable "health_check_eviction_time_in_min" {
  description = "Optional. The amount of time in minutes that a node is unhealthy before being removed. Default is 10."
  type        = number
  default     = 10
  nullable    = false
}

variable "app_settings" {
  description = "Optional. A map of key-value pairs of App Settings."
  type        = map(string)
  default     = {}
}

variable "enable_detailed_logging" {
  description = "Optional. Should detailed error messages and failed request tracing be enabled? Default is true."
  type        = bool
  default     = true
  nullable    = false
}

variable "log_retention_in_days" {
  description = "Optional. The retention period in days for HTTP logs. Default is 7."
  type        = number
  default     = 7
  nullable    = false

  validation {
    condition     = var.log_retention_in_days >= 1 && var.log_retention_in_days <= 99
    error_message = "Log retention must be between 1 and 99 days."
  }
}

variable "log_retention_in_mb" {
  description = "Optional. The maximum size in megabytes that HTTP log files can use. Default is 35."
  type        = number
  default     = 35
  nullable    = false

  validation {
    condition     = var.log_retention_in_mb >= 25 && var.log_retention_in_mb <= 100
    error_message = "Log retention size must be between 25 and 100 MB."
  }
}

variable "application_log_level" {
  description = "Optional. Log level for application logs. Possible values: Off, Verbose, Information, Warning, Error. Default is Information."
  type        = string
  default     = "Information"
  nullable    = false

  validation {
    condition     = contains(["Off", "Verbose", "Information", "Warning", "Error"], var.application_log_level)
    error_message = "Application log level must be one of: Off, Verbose, Information, Warning, Error."
  }
}

variable "tags" {
  description = "Optional. A mapping of tags to assign to the resource."
  type        = map(string)
  default     = {}
}
