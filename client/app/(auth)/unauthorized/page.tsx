"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function UnauthorizedPage() {
  return (
    <div className="flex h-screen items-center justify-center">
      <Card className="w-[400px] border-destructive">
        <CardHeader>
          <CardTitle className="text-destructive">Unauthorized Access</CardTitle>
          <CardDescription>You do not have permission to view this page.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <p className="text-sm text-muted-foreground">
            Please contact your administrator if you believe this is an error.
          </p>
          <Button className="w-full" onClick={() => window.location.href = "/"}>
            Go to Home
          </Button>
        </CardContent>
      </Card>
    </div>
  );
}
