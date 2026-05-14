"use client";

import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { apiFetch } from "@/lib/api/client";

export default function ReportingPage() {
  const [loading, setLoading] = useState(false);

  const handleGenerateReport = async () => {
    setLoading(true);
    try {
      const response = await apiFetch<{ reportId: string }>("/api/reporting/generate", {
        method: "POST",
        body: { title: "Monthly Usage Report" },
      });
      alert(`Generated report with ID: ${response.reportId}`);
    } catch (e) {
      alert("Failed to generate report.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Reporting</h1>
          <p className="text-muted-foreground">View and generate platform analytics.</p>
        </div>
        <Button onClick={handleGenerateReport} disabled={loading}>
          {loading ? "Generating..." : "Generate Report"}
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Recent Reports</CardTitle>
          <CardDescription>Your generated reports and analytics.</CardDescription>
        </CardHeader>
        <CardContent>
          <p className="text-sm text-muted-foreground">No reports found.</p>
        </CardContent>
      </Card>
    </div>
  );
}
