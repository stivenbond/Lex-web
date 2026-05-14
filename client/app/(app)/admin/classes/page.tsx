"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Button } from "@/components/ui/button";

export default function AdminClassesPage() {
  const classes = [
    { id: "C1", name: "Class 10-A", students: 30, teacher: "Mr. Brown" },
    { id: "C2", name: "Class 11-B", students: 25, teacher: "Ms. Green" },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Class Management</h1>
          <p className="text-muted-foreground">Manage school classes and enrollments.</p>
        </div>
        <Button>Create Class</Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Active Classes</CardTitle>
          <CardDescription>All academic groups for the current term.</CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Class Name</TableHead>
                <TableHead>Students</TableHead>
                <TableHead>Primary Teacher</TableHead>
                <TableHead className="text-right">Action</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {classes.map((c) => (
                <TableRow key={c.id}>
                  <TableCell className="font-medium">{c.name}</TableCell>
                  <TableCell>{c.students}</TableCell>
                  <TableCell>{c.teacher}</TableCell>
                  <TableCell className="text-right">
                    <Button variant="ghost" size="sm">Manage</Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
