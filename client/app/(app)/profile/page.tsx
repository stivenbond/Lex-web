"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useAuthStore } from "@/lib/store/useAuthStore";
import { useAuth } from "@/lib/auth/AuthContext";
import { apiFetch } from "@/lib/api/client";
import { useState } from "react";

export default function ProfilePage() {
  const { user } = useAuthStore();
  const { logout } = useAuth();
  const [loading, setLoading] = useState(false);

  const handleSyncGoogleDrive = async () => {
    setLoading(true);
    try {
      const response = await apiFetch<{ syncedCount: number }>("/api/google/sync", {
        method: "POST",
        body: { accessToken: "dummy_google_token" },
      });
      alert(`Successfully synced ${response.syncedCount} items from Google Drive.`);
    } catch (e) {
      alert("Failed to sync Google Drive.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mx-auto py-10">
      <Card className="max-w-2xl mx-auto">
        <CardHeader>
          <CardTitle>User Profile</CardTitle>
          <CardDescription>View and manage your account details.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="grid grid-cols-2 gap-4 border-b pb-4">
            <div className="font-medium text-muted-foreground">User ID</div>
            <div>{user?.userId}</div>
          </div>
          <div className="grid grid-cols-2 gap-4 border-b pb-4">
            <div className="font-medium text-muted-foreground">Email Address</div>
            <div>{user?.email}</div>
          </div>
          <div className="grid grid-cols-2 gap-4 border-b pb-4">
            <div className="font-medium text-muted-foreground">Roles</div>
            <div className="flex flex-wrap gap-2">
              {user?.roles.map((role) => (
                <span key={role} className="bg-primary/10 text-primary px-2 py-1 rounded text-xs">
                  {role}
                </span>
              ))}
            </div>
          </div>
          <div className="pt-6 flex gap-4">
            <Button variant="outline" onClick={handleSyncGoogleDrive} disabled={loading}>
              {loading ? "Syncing..." : "Sync Google Drive"}
            </Button>
            <Button variant="destructive" onClick={logout}>
              Sign Out
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
