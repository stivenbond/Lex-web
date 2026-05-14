"use client";

import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { apiFetch } from "@/lib/api/client";

export default function NotificationsPage() {
  const [loading, setLoading] = useState(false);
  const notifications = [
    { id: 1, title: "New Assignment", content: "Mathematics Quiz 3 is now available.", time: "2 hours ago", unread: true },
    { id: 2, title: "Schedule Change", content: "Physics lecture moved to Room 402.", time: "1 day ago", unread: false },
  ];

  const handleSendNotification = async () => {
    setLoading(true);
    try {
      const response = await apiFetch<{ notificationId: string }>("/api/notifications/send", {
        method: "POST",
        body: { userId: "user123", title: "Test Notification", message: "This is a test notification." },
      });
      alert(`Sent notification with ID: ${response.notificationId}`);
    } catch (e) {
      alert("Failed to send notification.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Notifications</h1>
          <p className="text-muted-foreground">Stay updated with the latest activity.</p>
        </div>
        <div className="space-x-2">
          <Button variant="outline" onClick={handleSendNotification} disabled={loading}>
            {loading ? "Sending..." : "Send Test Notification"}
          </Button>
          <Button variant="outline">Mark all as read</Button>
        </div>
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
