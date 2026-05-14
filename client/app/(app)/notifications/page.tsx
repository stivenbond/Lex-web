"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function NotificationsPage() {
  const notifications = [
    { id: 1, title: "New Assignment", content: "Mathematics Quiz 3 is now available.", time: "2 hours ago", unread: true },
    { id: 2, title: "Schedule Change", content: "Physics lecture moved to Room 402.", time: "1 day ago", unread: false },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Notifications</h1>
          <p className="text-muted-foreground">Stay updated with the latest activity.</p>
        </div>
        <Button variant="outline">Mark all as read</Button>
      </div>

      <div className="space-y-4">
        {notifications.map((n) => (
          <Card key={n.id} className={n.unread ? "border-primary bg-primary/5" : ""}>
            <CardHeader className="py-4">
              <div className="flex justify-between items-start">
                <CardTitle className="text-lg">{n.title}</CardTitle>
                <span className="text-xs text-muted-foreground">{n.time}</span>
              </div>
              <CardDescription>{n.content}</CardDescription>
            </CardHeader>
          </Card>
        ))}
      </div>
    </div>
  );
}
