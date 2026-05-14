#!/usr/bin/env bash
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
if [[ "$(basename "$SCRIPT_DIR")" == "scripts" ]]; then
  cd "$(dirname "$SCRIPT_DIR")"
else
  cd "$SCRIPT_DIR"
fi

source .env

echo "Pulling latest images for version ${LEX_VERSION:-latest}..."
docker compose pull api web

echo "Starting rolling upgrade..."
# We restart the api container first and wait for health
docker compose up -d --no-deps api
echo "Waiting for API to become healthy..."
for i in $(seq 1 30); do
  curl -sf http://localhost:80/readyz &>/dev/null && break
  sleep 3; [[ $i -eq 30 ]] && { echo "ERROR: API /readyz failed after upgrade"; exit 1; }
done

# Then update the web container
docker compose up -d --no-deps web

echo "✓ Zero-downtime upgrade completed successfully."
