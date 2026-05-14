"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export default function PasswordResetPage() {
  return (
    <div className="flex h-screen items-center justify-center">
      <Card className="w-[400px]">
        <CardHeader>
          <CardTitle>Reset Password</CardTitle>
          <CardDescription>Enter your email to receive a password reset link.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <input 
            type="email" 
            placeholder="email@example.com" 
            className="w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-primary"
          />
          <Button className="w-full">Send Reset Link</Button>
          <Button variant="link" className="w-full" onClick={() => window.location.href = "/"}>
            Back to Login
          </Button>
        </CardContent>
      </Card>
    </div>
  );
}
