#!/usr/bin/env bash
# =============================================================================
# new-integration.sh
# Scaffolds a new external API integration using Refit.
# =============================================================================
set -euo pipefail

MOD=$1
SVC=$2
APP=${3:-Lex}

if [[ -z "$MOD" || -z "$SVC" ]]; then
  echo "Usage: $0 <MOD> <SVC> [APP]"
  exit 1
fi

echo "Scaffolding external integration: $SVC in module $MOD"

CORE="src/Modules/$APP.Module.$MOD/$APP.Module.$MOD.Core"
INFRA="src/Modules/$APP.Module.$MOD/$APP.Module.$MOD.Infrastructure"

mkdir -p "$CORE/Abstractions"
mkdir -p "$INFRA/ExternalApis"

cat > "$CORE/Abstractions/I${SVC}Client.cs" << CS
namespace Lex.Module.$MOD.Abstractions;

public interface I${SVC}Client
{
    // Define external API methods here
}
CS

cat > "$INFRA/ExternalApis/I${SVC}Api.cs" << CS
using Refit;

namespace Lex.Module.$MOD.ExternalApis;

public interface I${SVC}Api
{
    // Define Refit interface here
    // [Get("/path")]
    // Task<T> GetData();
}
CS

echo "✓ Integration for $SVC scaffolded in $MOD"
