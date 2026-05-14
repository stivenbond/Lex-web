"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function AdminSettingsPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Platform Settings</h1>
        <p className="text-muted-foreground">Configure global system parameters.</p>
      </div>

      <div className="grid gap-6">
        <Card>
          <CardHeader>
            <CardTitle>General Configuration</CardTitle>
            <CardDescription>Basic platform identification and contact info.</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid gap-2">
              <label className="text-sm font-medium">Institution Name</label>
              <input className="w-full px-3 py-2 border rounded-md" defaultValue="Lex Academy" />
            </div>
            <div className="grid gap-2">
              <label className="text-sm font-medium">Support Email</label>
              <input className="w-full px-3 py-2 border rounded-md" defaultValue="support@lex.edu" />
            </div>
            <Button>Save Changes</Button>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Infrastructure Status</CardTitle>
            <CardDescription>Current state of connected services.</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex justify-between items-center py-2 border-b">
              <span>Database (PostgreSQL)</span>
              <span className="text-green-500 font-medium text-sm">Online</span>
            </div>
            <div className="flex justify-between items-center py-2 border-b">
              <span>Message Bus (RabbitMQ)</span>
              <span className="text-green-500 font-medium text-sm">Online</span>
            </div>
            <div className="flex justify-between items-center py-2">
              <span>Identity Provider (Keycloak)</span>
              <span className="text-green-500 font-medium text-sm">Online</span>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
