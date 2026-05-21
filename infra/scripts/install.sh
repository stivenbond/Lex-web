#!/usr/bin/env bash
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
if [[ "$(basename "$SCRIPT_DIR")" == "scripts" ]]; then
  cd "$(dirname "$SCRIPT_DIR")"
else
  cd "$SCRIPT_DIR"
fi

command -v docker &>/dev/null        || { echo "ERROR: docker not found."; exit 1; }
command -v docker compose &>/dev/null || { echo "ERROR: docker compose not found."; exit 1; }

[[ -f .env ]] || {
  echo "Creating .env from template..."
  cp .env.template .env
  
  # Auto-generate secure passwords if they are left as change-me
  sed -i "s/^DB_PASSWORD=change-me/DB_PASSWORD=$(openssl rand -base64 16 | tr -dc 'a-zA-Z0-9' | head -c 20)/" .env
  sed -i "s/^RABBITMQ_PASSWORD=change-me/RABBITMQ_PASSWORD=$(openssl rand -base64 16 | tr -dc 'a-zA-Z0-9' | head -c 20)/" .env
  sed -i "s/^KEYCLOAK_ADMIN_PASSWORD=change-me/KEYCLOAK_ADMIN_PASSWORD=$(openssl rand -base64 16 | tr -dc 'a-zA-Z0-9' | head -c 20)/" .env
  sed -i "s/^MINIO_ACCESS_KEY=minioadmin/MINIO_ACCESS_KEY=$(openssl rand -base64 12 | tr -dc 'a-zA-Z0-9' | head -c 16)/" .env
  sed -i "s/^MINIO_SECRET_KEY=minioadmin/MINIO_SECRET_KEY=$(openssl rand -base64 24 | tr -dc 'a-zA-Z0-9' | head -c 32)/" .env
  
  echo "Auto-generated secure credentials in .env file."
}

source .env
for v in DB_PASSWORD RABBITMQ_PASSWORD KEYCLOAK_ADMIN_PASSWORD MINIO_ACCESS_KEY MINIO_SECRET_KEY; do
  [[ -n "${!v:-}" ]] || { echo "ERROR: $v not set in .env"; exit 1; }
  if [[ "${!v}" == "change-me" || "${!v}" == "minioadmin" ]]; then
    echo "ERROR: $v is still set to the default value. Please change it in .env."
    exit 1
  fi
done

docker compose pull postgres rabbitmq redis minio seq
docker compose build api web
docker compose up postgres rabbitmq redis minio seq

echo "Waiting for infrastructure..."
for s in postgres rabbitmq redis; do
  for i in $(seq 1 30); do
    docker compose ps "$s" | grep -q "healthy" && break
    sleep 2; [[ $i -eq 30 ]] && { echo "ERROR: $s not healthy"; exit 1; }
  done
  echo "  ✓ $s"
done

echo "Starting Keycloak (may take 90s)..."
docker compose up keycloak
for i in $(seq 1 60); do
  docker compose ps keycloak | grep -q "healthy" && break
  sleep 3; [[ $i -eq 60 ]] && { echo "ERROR: Keycloak not healthy"; exit 1; }
done
echo "  ✓ keycloak"

docker compose up api
for i in $(seq 1 30); do
  curl -sf http://localhost:80/readyz &>/dev/null && break
  sleep 3; [[ $i -eq 30 ]] && { echo "ERROR: API /readyz failed"; exit 1; }
done
echo "  ✓ api"

docker compose up web
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo " Lex Platform installed successfully!"
echo ""
echo " Access Points:"
echo "  App       : http://localhost"
echo "  Keycloak  : http://localhost:8080"
echo "  MinIO     : http://localhost:9001"
echo "  Seq       : http://localhost:8083"
echo "  RabbitMQ  : http://localhost:15672"
echo ""
echo " Generated Credentials (saved in infra/.env):"
echo "  Keycloak Admin : admin / ${KEYCLOAK_ADMIN_PASSWORD}"
echo "  MinIO User     : ${MINIO_ACCESS_KEY} / ${MINIO_SECRET_KEY}"
echo "  RabbitMQ User  : lex / ${RABBITMQ_PASSWORD}"
echo "  Postgres User  : lex / ${DB_PASSWORD}"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
