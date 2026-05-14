"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import Link from "next/link";
import { Activity, Users, BookOpen, Settings, Download, Shield, Database, LayoutDashboard } from "lucide-react";

export default function AdminDashboardPage() {
  const internalLinks = [
    { title: "User Management", desc: "Manage students, teachers, and admins.", href: "/admin/users", icon: <Users className="h-6 w-6 text-primary" /> },
    { title: "Class Management", desc: "Configure classes and enrollments.", href: "/admin/classes", icon: <BookOpen className="h-6 w-6 text-primary" /> },
    { title: "System Settings", desc: "Platform-wide configurations.", href: "/admin/settings", icon: <Settings className="h-6 w-6 text-primary" /> },
    { title: "Import/Export", desc: "System backups and data migrations.", href: "/admin/import-export", icon: <Download className="h-6 w-6 text-primary" /> },
  ];

  const infrastructureLinks = [
    { title: "Telemetry & Logs (Seq)", desc: "Centralized logging and metrics.", href: "http://localhost:8083", icon: <Activity className="h-6 w-6 text-indigo-500" /> },
    { title: "Identity (Keycloak)", desc: "Manage authentication and realms.", href: "http://localhost:8080/admin", icon: <Shield className="h-6 w-6 text-indigo-500" /> },
    { title: "Message Queue (RabbitMQ)", desc: "Monitor background events.", href: "http://localhost:15672", icon: <LayoutDashboard className="h-6 w-6 text-indigo-500" /> },
    { title: "Object Storage (MinIO)", desc: "Manage S3-compatible storage.", href: "http://localhost:9001", icon: <Database className="h-6 w-6 text-indigo-500" /> },
  ];

  return (
    <div className="container mx-auto py-10 space-y-8">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Platform Administration</h1>
        <p className="text-muted-foreground">Centralized hub for managing Lex Platform operations and infrastructure.</p>
      </div>

      <div className="space-y-4">
        <h2 className="text-xl font-semibold border-b pb-2">Platform Features</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          {internalLinks.map((link) => (
            <Card key={link.title} className="hover:shadow-md transition-shadow">
              <CardHeader className="flex flex-row items-center space-x-4 pb-2">
                {link.icon}
                <div className="space-y-1">
                  <CardTitle className="text-lg">{link.title}</CardTitle>
                </div>
              </CardHeader>
              <CardContent>
                <CardDescription className="mb-4">{link.desc}</CardDescription>
                <Button asChild variant="outline" className="w-full">
                  <Link href={link.href}>Manage</Link>
                </Button>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>

      <div className="space-y-4">
        <h2 className="text-xl font-semibold border-b pb-2">Infrastructure & Telemetry</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          {infrastructureLinks.map((link) => (
            <Card key={link.title} className="hover:shadow-md transition-shadow bg-slate-50/50">
              <CardHeader className="flex flex-row items-center space-x-4 pb-2">
                {link.icon}
                <div className="space-y-1">
                  <CardTitle className="text-lg">{link.title}</CardTitle>
                </div>
              </CardHeader>
              <CardContent>
                <CardDescription className="mb-4">{link.desc}</CardDescription>
                <Button asChild variant="secondary" className="w-full">
                  <a href={link.href} target="_blank" rel="noopener noreferrer">Open Console</a>
                </Button>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>
    </div>
  );
}
