"use client";

import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { apiFetch } from "@/lib/api/client";

export default function DeliveryPage() {
  const [loading, setLoading] = useState(false);

  const handleStartSession = async () => {
    setLoading(true);
    try {
      const response = await apiFetch<{ sessionId: string }>("/api/delivery/start", {
        method: "POST",
        body: { snapshotId: "00000000-0000-0000-0000-000000000000", studentId: "student123" },
      });
      alert(`Started session with ID: ${response.sessionId}`);
    } catch (e) {
      alert("Failed to start session.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Assessment Delivery</h1>
          <p className="text-muted-foreground">Take assessments and view your ongoing sessions.</p>
        </div>
        <Button onClick={handleStartSession} disabled={loading}>
          {loading ? "Starting..." : "Start Session"}
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Active Sessions</CardTitle>
          <CardDescription>Assessments you are currently taking.</CardDescription>
        </CardHeader>
        <CardContent>
          <p className="text-sm text-muted-foreground">No active sessions found.</p>
        </CardContent>
      </Card>
    </div>
  );
}
