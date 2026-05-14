"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function ReportsPage() {
  const reports = [
    { id: "R1", name: "Quarterly Performance", type: "Academic", date: "2024-04-01" },
    { id: "R2", name: "Attendance Summary", type: "Administrative", date: "2024-05-01" },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Reports & Analytics</h1>
          <p className="text-muted-foreground">Generate and view institutional reports.</p>
        </div>
        <Button>Generate Report</Button>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        {reports.map((report) => (
          <Card key={report.id}>
            <CardHeader>
              <CardTitle>{report.name}</CardTitle>
              <CardDescription>{report.type} report generated on {report.date}</CardDescription>
            </CardHeader>
            <CardContent>
              <Button variant="outline" size="sm">Download PDF</Button>
              <Button variant="ghost" size="sm" className="ml-2">View Online</Button>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  );
}
