"use client";

import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { apiFetch } from "@/lib/api/client";

export default function AssessmentsPage() {
  const [loading, setLoading] = useState(false);
  const assessments = [
    { id: "A1", title: "Midterm Calculus", status: "Completed", score: "85/100" },
    { id: "A2", title: "Quantum Physics Quiz", status: "In Progress", score: "-" },
    { id: "A3", title: "Organic Chemistry Lab", status: "Upcoming", score: "-" },
  ];

  const handleCreateAssessment = async () => {
    setLoading(true);
    try {
      const response = await apiFetch<{ assessmentId: string }>("/api/assessments", {
        method: "POST",
        body: { title: "New Assessment Draft" },
      });
      alert(`Created assessment with ID: ${response.assessmentId}`);
    } catch (e) {
      alert("Failed to create assessment.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Assessments</h1>
          <p className="text-muted-foreground">Track your progress and complete evaluations.</p>
        </div>
        <Button onClick={handleCreateAssessment} disabled={loading}>
          {loading ? "Creating..." : "New Assessment"}
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Recent Assessments</CardTitle>
          <CardDescription>Your assessment history and upcoming tasks.</CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Title</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Score</TableHead>
                <TableHead className="text-right">Action</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {assessments.map((a) => (
                <TableRow key={a.id}>
                  <TableCell className="font-medium">{a.title}</TableCell>
                  <TableCell>{a.status}</TableCell>
                  <TableCell>{a.score}</TableCell>
                  <TableCell className="text-right">
                    <Button variant="ghost" size="sm">View</Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
