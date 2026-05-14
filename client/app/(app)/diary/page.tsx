"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function DiaryPage() {
  const entries = [
    { id: 1, date: "2024-05-14", title: "Project Milestone Reached", content: "Successfully completed the core infrastructure for the modular monolith." },
    { id: 2, date: "2024-05-13", title: "Research on Keycloak", content: "Integrated Keycloak for OIDC authentication flow." },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Personal Diary</h1>
          <p className="text-muted-foreground">Keep track of your academic journey and thoughts.</p>
        </div>
        <Button>New Entry</Button>
      </div>

      <div className="grid gap-6">
        {entries.map((entry) => (
          <Card key={entry.id}>
            <CardHeader>
              <div className="text-sm text-muted-foreground mb-1">{entry.date}</div>
              <CardTitle>{entry.title}</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-sm leading-relaxed">{entry.content}</p>
            </CardContent>
            <CardFooter>
              <Button variant="ghost" size="sm">Edit</Button>
              <Button variant="ghost" size="sm" className="text-destructive">Delete</Button>
            </CardFooter>
          </Card>
        ))}
      </div>
    </div>
  );
}
