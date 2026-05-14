"use client";

import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { apiFetch } from "@/lib/api/client";

export default function ImportExportPage() {
  const [loading, setLoading] = useState(false);

  const handleExport = async () => {
    setLoading(true);
    try {
      const response = await apiFetch<{ exportId: string }>("/api/import-export/export", {
        method: "POST",
        body: { type: "FullDatabaseBackup" },
      });
      alert(`Export started with ID: ${response.exportId}`);
    } catch (e) {
      alert("Failed to start export.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-6 container mx-auto py-10">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Import/Export Data</h1>
          <p className="text-muted-foreground">Manage your system data backups and imports.</p>
        </div>
        <Button onClick={handleExport} disabled={loading}>
          {loading ? "Exporting..." : "Start Export"}
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Recent Exports</CardTitle>
          <CardDescription>Your system data exports.</CardDescription>
        </CardHeader>
        <CardContent>
          <p className="text-sm text-muted-foreground">No exports found.</p>
        </CardContent>
      </Card>
    </div>
  );
}
