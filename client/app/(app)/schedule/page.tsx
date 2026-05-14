"use client";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

export default function SchedulePage() {
  const schedule = [
    { time: "08:00 - 09:30", monday: "Mathematics", tuesday: "Physics", wednesday: "Chemistry", thursday: "Biology", friday: "English" },
    { time: "09:45 - 11:15", monday: "History", tuesday: "Geography", wednesday: "Art", thursday: "Music", friday: "Physical Ed" },
    { time: "11:30 - 13:00", monday: "Computer Science", tuesday: "Literature", wednesday: "Philosophy", thursday: "Economics", friday: "Sociology" },
  ];

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Academic Schedule</h1>
        <p className="text-muted-foreground">Manage and view your weekly lesson schedule.</p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Weekly View</CardTitle>
          <CardDescription>Your current timetable for the 2024-2025 academic year.</CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead className="w-[150px]">Time</TableHead>
                <TableHead>Monday</TableHead>
                <TableHead>Tuesday</TableHead>
                <TableHead>Wednesday</TableHead>
                <TableHead>Thursday</TableHead>
                <TableHead>Friday</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {schedule.map((item) => (
                <TableRow key={item.time}>
                  <TableCell className="font-medium">{item.time}</TableCell>
                  <TableCell>{item.monday}</TableCell>
                  <TableCell>{item.tuesday}</TableCell>
                  <TableCell>{item.wednesday}</TableCell>
                  <TableCell>{item.thursday}</TableCell>
                  <TableCell>{item.friday}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
