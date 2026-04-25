import { defineConfig } from "vitest/config";

export default defineConfig({
  test: {
    include: ["lib/**/*.test.ts", "lib/**/*.test.tsx", "components/**/*.test.tsx", "app/**/*.test.tsx"],
    exclude: ["e2e/**", "tests/**", "node_modules/**"],
    passWithNoTests: true,
  },
});
