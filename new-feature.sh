#!/usr/bin/env bash
# =============================================================================
# new-feature.sh
# Scaffolds a new vertical slice feature (Command, Query, or Event).
# =============================================================================
set -euo pipefail

MOD=$1
FEAT=$2
TYPE=${3:-command}
APP=${4:-Lex}

if [[ -z "$MOD" || -z "$FEAT" ]]; then
  echo "Usage: $0 <MOD> <FEAT> [TYPE] [APP]"
  exit 1
fi

BASE="src/Modules/$APP.Module.$MOD/$APP.Module.$MOD.Core/Features/$FEAT"
mkdir -p "$BASE"

echo "Scaffolding $TYPE feature: $FEAT in module $MOD"

# Generate based on type
if [[ "$TYPE" == "command" ]]; then
cat > "$BASE/${FEAT}Command.cs" << CS
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.$MOD.Features.$FEAT;

public sealed record ${FEAT}Command() : IRequest<Result<Guid>>;
CS

cat > "$BASE/${FEAT}CommandHandler.cs" << CS
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.$MOD.Features.$FEAT;

internal sealed class ${FEAT}CommandHandler : IRequestHandler<${FEAT}Command, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(${FEAT}Command request, CancellationToken ct)
    {
        // TODO: Implementation
        return Guid.NewGuid();
    }
}
CS

cat > "$BASE/${FEAT}Validator.cs" << CS
using FluentValidation;

namespace Lex.Module.$MOD.Features.$FEAT;

public sealed class ${FEAT}Validator : AbstractValidator<${FEAT}Command>
{
    public ${FEAT}Validator()
    {
        // TODO: Validation rules
    }
}
CS

elif [[ "$TYPE" == "query" ]]; then
cat > "$BASE/${FEAT}Query.cs" << CS
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.$MOD.Features.$FEAT;

public sealed record ${FEAT}Query() : IRequest<Result<${FEAT}Response>>;

public sealed record ${FEAT}Response();
CS

cat > "$BASE/${FEAT}QueryHandler.cs" << CS
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.$MOD.Features.$FEAT;

internal sealed class ${FEAT}QueryHandler : IRequestHandler<${FEAT}Query, Result<${FEAT}Response>>
{
    public async Task<Result<${FEAT}Response>> Handle(${FEAT}Query request, CancellationToken ct)
    {
        // TODO: Implementation
        return new ${FEAT}Response();
    }
}
CS

else # event
cat > "$BASE/${FEAT}Event.cs" << CS
using MediatR;

namespace Lex.Module.$MOD.Features.$FEAT;

public sealed record ${FEAT}Event() : INotification;
CS

cat > "$BASE/${FEAT}EventHandler.cs" << CS
using MediatR;

namespace Lex.Module.$MOD.Features.$FEAT;

internal sealed class ${FEAT}EventHandler : INotificationHandler<${FEAT}Event>
{
    public async Task Handle(${FEAT}Event notification, CancellationToken ct)
    {
        // TODO: Side effects
    }
}
CS
fi

echo "✓ Feature $FEAT scaffolded in $BASE"
