#!/usr/bin/env bash
# =============================================================================
# new-adr.sh
# Generates a new Architecture Decision Record (ADR) file.
# =============================================================================
set -euo pipefail

TITLE=$1
MODE=${2:-}

ADR_DIR="docs/adr"
if [[ "$MODE" == "--client" ]]; then
  ADR_DIR="docs/adr/client"
fi

mkdir -p "$ADR_DIR"

if [[ -z "$TITLE" ]]; then
  echo "Usage: $0 <TITLE> [--client]"
  exit 1
fi

# Find next number
LAST_ADR=$(ls "$ADR_DIR" 2>/dev/null | grep -E "ADR-[0-9]{3}\.md" | sort -r | head -n1)
if [[ -z "$LAST_ADR" ]]; then
  NEXT_NUM=1
else
  LAST_NUM=$(echo "$LAST_ADR" | sed 's/ADR-//;s/\.md//' | sed 's/^0*//')
  NEXT_NUM=$((LAST_NUM + 1))
fi

FILENAME=$(printf "ADR-%03d.md" "$NEXT_NUM")
DATE=$(date '+%Y-%m-%d')

cat > "$ADR_DIR/$FILENAME" << MD
# $FILENAME: $TITLE

## Status
Proposed

## Date
$DATE

## Context
<!-- What is the problem we are solving? -->

## Decision
<!-- What is the decision we are making? -->

## Consequences
<!-- What are the positive and negative consequences of this decision? -->
MD

echo "✓ Created $ADR_DIR/$FILENAME: $TITLE"
