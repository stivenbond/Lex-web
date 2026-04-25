"use client";

import React, { useEffect, useState } from "react";
import { useSignalR } from "@/lib/auth/SignalRProvider";
import { Card, CardContent, CardHeader, CardTitle } from "./ui/card";
import { Loader2, CheckCircle2, XCircle, Info } from "lucide-react";

interface JobStatus {
  id: string;
  name: string;
  progress: number;
  status: "running" | "completed" | "failed";
  message?: string;
}

export const JobStatusList: React.FC = () => {
  const { connection } = useSignalR();
  const [jobs, setJobs] = useState<Record<string, JobStatus>>({});

  useEffect(() => {
    if (!connection) return;

    connection.on("JobStarted", (jobId: string, jobName: string) => {
      setJobs(prev => ({
        ...prev,
        [jobId]: { id: jobId, name: jobName, progress: 0, status: "running" }
      }));
    });

    connection.on("JobProgressUpdated", (jobId: string, progress: number, message: string) => {
      setJobs(prev => ({
        ...prev,
        [jobId]: { ...prev[jobId], progress, message }
      }));
    });

    connection.on("JobCompleted", (jobId: string) => {
      setJobs(prev => ({
        ...prev,
        [jobId]: { ...prev[jobId], progress: 100, status: "completed", message: "Successfully finished." }
      }));
      // Remove completed job after 5 seconds
      setTimeout(() => {
        setJobs(prev => {
          const newJobs = { ...prev };
          delete newJobs[jobId];
          return newJobs;
        });
      }, 5000);
    });

    connection.on("JobFailed", (jobId: string, error: string) => {
      setJobs(prev => ({
        ...prev,
        [jobId]: { ...prev[jobId], status: "failed", message: error }
      }));
    });

    return () => {
      connection.off("JobStarted");
      connection.off("JobProgressUpdated");
      connection.off("JobCompleted");
      connection.off("JobFailed");
    };
  }, [connection]);

  const jobList = Object.values(jobs);

  if (jobList.length === 0) return null;

  return (
    <div className="fixed bottom-4 right-4 w-80 space-y-2 z-50">
      {jobList.map(job => (
        <Card key={job.id} className="bg-zinc-900 border-zinc-800 shadow-xl border-l-4 border-l-primary overflow-hidden">
          <CardHeader className="p-3 pb-0 flex flex-row items-center justify-between">
            <CardTitle className="text-sm font-medium truncate pr-2">{job.name}</CardTitle>
            {job.status === "running" && <Loader2 className="h-4 w-4 animate-spin text-primary" />}
            {job.status === "completed" && <CheckCircle2 className="h-4 w-4 text-green-500" />}
            {job.status === "failed" && <XCircle className="h-4 w-4 text-destructive" />}
          </CardHeader>
          <CardContent className="p-3 pt-2">
            <div className="w-full bg-zinc-800 h-1 rounded-full overflow-hidden mb-2">
              <div 
                className="bg-primary h-full transition-all duration-300" 
                style={{ width: `${job.progress}%` }}
              />
            </div>
            {job.message && (
              <p className="text-[10px] text-muted-foreground flex items-center gap-1">
                <Info className="h-3 w-3" />
                {job.message}
              </p>
            )}
          </CardContent>
        </Card>
      ))}
    </div>
  );
};
