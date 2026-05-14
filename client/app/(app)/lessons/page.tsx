"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function LessonsPage() {
  const lessons = [
    { id: 1, title: "Introduction to Calculus", description: "Foundations of limits and derivatives.", subject: "Mathematics" },
    { id: 2, title: "Quantum Mechanics Basics", description: "Exploring the wave-particle duality.", subject: "Physics" },
    { id: 3, title: "Organic Chemistry", description: "Study of carbon-based compounds.", subject: "Chemistry" },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Lesson Management</h1>
          <p className="text-muted-foreground">Browse and manage educational content.</p>
        </div>
        <Button>Create Lesson</Button>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        {lessons.map((lesson) => (
          <Card key={lesson.id} className="flex flex-col">
            <CardHeader>
              <div className="text-xs font-semibold uppercase tracking-wider text-primary mb-1">
                {lesson.subject}
              </div>
              <CardTitle>{lesson.title}</CardTitle>
              <CardDescription>{lesson.description}</CardDescription>
            </CardHeader>
            <CardContent className="flex-1">
              {/* Content preview could go here */}
            </CardContent>
            <CardFooter>
              <Button variant="outline" className="w-full">View Details</Button>
            </CardFooter>
          </Card>
        ))}
      </div>
    </div>
  );
}
