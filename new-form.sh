#!/usr/bin/env bash
# =============================================================================
# new-form.sh
# Scaffolds a new React form component using react-hook-form and zod.
# =============================================================================
set -euo pipefail

FORM=$1
MOD=$2

if [[ -z "$FORM" || -z "$MOD" ]]; then
  echo "Usage: $0 <FORM> <MOD>"
  exit 1
fi

C="client/components/forms"
mkdir -p "$C"

echo "Scaffolding form: $FORM in module $MOD"

cat > "$C/${FORM}Form.tsx" << TSX
"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";

const formSchema = z.object({
  // Define schema fields here
});

export type ${FORM}FormValues = z.infer<typeof formSchema>;

interface ${FORM}FormProps {
  onSubmit: (values: ${FORM}FormValues) => void;
}

export function ${FORM}Form({ onSubmit }: ${FORM}FormProps) {
  const form = useForm<${FORM}FormValues>({
    resolver: zodResolver(formSchema),
  });

  return (
    <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
      {/* Form fields here */}
      <button type="submit" className="px-4 py-2 bg-primary text-white rounded">Submit</button>
    </form>
  );
}
TSX

echo "✓ Form component ${FORM}Form.tsx scaffolded in $C"
