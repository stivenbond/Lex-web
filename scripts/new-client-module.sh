#!/usr/bin/env bash
# =============================================================================
# new-client-module.sh
# Scaffolds a new frontend module directory and initial page.
# =============================================================================
set -euo pipefail

MOD=$1

if [[ -z "$MOD" ]]; then
  echo "Usage: $0 <MOD> (lowercase)"
  exit 1
fi

C="client/app/(app)/$MOD"
mkdir -p "$C"

echo "Scaffolding client module: $MOD"

TITLE=$(echo "$MOD" | awk '{print toupper(substr($0,1,1)) substr($0,2)}')

cat > "$C/page.tsx" << TSX
export const metadata = { title: "$TITLE" };

export default function ${TITLE}Page() {
  return (
    <div className="p-6">
      <h1 className="text-2xl font-semibold">$TITLE</h1>
      <p className="mt-4 text-muted-foreground">
        Welcome to the $TITLE module.
      </p>
    </div>
  );
}
TSX

echo "✓ Client module $MOD scaffolded in $C"
