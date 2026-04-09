# Specification: Institutional Installation Guide (README-install.md)

## 1. Overview
- **Category**: Documentation / Handover
- **Scope**: Definition of the required content for the IT Admin guide.

## 2. Prerequisites Section
- Hardware minimums (RAM, CPU, Storage).
- Network requirements (Internal ports: 443, 80; External ports for Google Integration).
- Docker Engine and Docker Compose versions.

## 3. The "Two-Minute" Install
- Command block for unzipping and running `install.sh`.
- Explanation of what the script does (Image loading, volume creation).

## 4. Configuration Reference
- Detailed table of `.env` variables:
    - `KEYCLOAK_ADMIN_PASSWORD`: Default security setup.
    - `POSTGRES_PASSWORD`: Database hardening.
    - `SMTP_SETTINGS`: Email routing.
    - `SSL_CERT_PATH`: Certificates for YARP.

## 5. Operations Guide
- **Backup**: How to run/schedule `backup.sh`.
- **Upgrade**: How to apply a minor/major version patch via `upgrade.sh`.
- **Troubleshooting**:
    - Checking logs using Seq (`http://localhost:5341`).
    - Verifying system readiness (`/readyz`).
    - Restarting the stack.

## 6. Security Hardening
- Recommendations for firewall rules.
- Guidance on restricting access to the Seq and Keycloak Admin interfaces.
- Backup encryption (Off-host storage).

## 7. Air-Gapped Instructions
- Step-by-step for offline image loading.
- Workaround for lack of public DNS/SSL.
