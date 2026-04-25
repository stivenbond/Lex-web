resource "keycloak_realm" "lex" {
  realm   = var.realm_name
  enabled = true

  display_name = "Lex Web Platform"

  login_with_email_allowed = true
  reset_password_allowed   = true
  remember_me_allowed      = true

  browser_flow = "browser"
}

resource "keycloak_role" "admin" {
  realm_id    = keycloak_realm.lex.id
  name        = "Admin"
  description = "Platform Administrator"
}

resource "keycloak_role" "teacher" {
  realm_id    = keycloak_realm.lex.id
  name        = "Teacher"
  description = "Lesson and Assessment Creator"
}

resource "keycloak_role" "student" {
  realm_id    = keycloak_realm.lex.id
  name        = "Student"
  description = "Platform Consumer"
}

resource "keycloak_openid_client" "lex_web" {
  realm_id  = keycloak_realm.lex.id
  client_id = "lex-web"
  name      = "Lex Web Client"
  enabled   = true

  access_type              = "PUBLIC"
  standard_flow_enabled    = true
  direct_access_grants_enabled = true

  valid_redirect_uris = [
    "http://localhost:3000/*",
    "http://localhost:3000/silent-check-sso.html"
  ]

  web_origins = [
    "http://localhost:3000"
  ]

  pkce_code_challenge_method = "S256"
}

resource "keycloak_openid_client_default_scopes" "client_default_scopes" {
  realm_id  = keycloak_realm.lex.id
  client_id = keycloak_openid_client.lex_web.id

  default_scopes = [
    "profile",
    "email",
    "roles",
    "web-origins"
  ]
}
