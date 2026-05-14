"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useAuthStore } from "@/lib/store/useAuthStore";
import { useAuth } from "@/lib/auth/AuthContext";

export default function ProfilePage() {
  const { user } = useAuthStore();
  const { logout } = useAuth();

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
          <div className="pt-6">
            <Button variant="destructive" onClick={logout}>
              Sign Out
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
