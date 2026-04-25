variable "keycloak_url" {
  type    = string
  default = "http://localhost:8080"
}

variable "keycloak_admin_user" {
  type    = string
  default = "admin"
}

variable "keycloak_admin_password" {
  type      = string
  sensitive = true
}

variable "realm_name" {
  type    = string
  default = "lex"
}
