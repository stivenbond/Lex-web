"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiFetch } from "@/lib/api/client";
import { Button } from "@/components/ui/button"; // I'll create this
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"; // I'll create this
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"; // I'll create this
import { FileIcon, UploadIcon, TrashIcon, DownloadIcon, Loader2, CpuIcon } from "lucide-react";
import { useState } from "react";

interface FileMetadata {
  id: string;
  fileName: string;
  contentType: string;
  size: number;
  createdAt: string;
}

export default function FilesPage() {
  const queryClient = useQueryClient();
  const [isUploading, setIsUploading] = useState(false);

  const { data: files, isLoading } = useQuery<FileMetadata[]>({
    queryKey: ["files"],
    queryFn: () => apiFetch("/api/files"),
  });

  const uploadMutation = useMutation({
    mutationFn: async (file: File) => {
      const formData = new FormData();
      formData.append("file", file);
      
      const response = await fetch("/api/files/upload", {
        method: "POST",
        body: formData,
        headers: {
          // Authorization is handled by the middleware/interceptor if possible,
          // but here we might need manual token injection if apiFetch is not used.
          // For now, let's assume the session is handled.
        }
      });
      if (!response.ok) throw new Error("Upload failed");
      return response.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["files"] });
      setIsUploading(false);
    },
  });

  const processMutation = useMutation({
    mutationFn: (file: FileMetadata) => apiFetch("/api/file-processing/enqueue", { 
      method: "POST", 
      body: { fileId: file.id, fileName: file.fileName } 
    }),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => apiFetch(`/api/files/${id}`, { method: "DELETE" }),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["files"] }),
  });

  const handleFileUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setIsUploading(true);
      uploadMutation.mutate(file);
    }
  };

  const formatSize = (bytes: number) => {
    if (bytes === 0) return "0 Bytes";
    const k = 1024;
    const sizes = ["Bytes", "KB", "MB", "GB"];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
  };

  return (
    <div className="container mx-auto py-10 space-y-8">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-4xl font-bold tracking-tight text-white">Files</h1>
          <p className="text-muted-foreground">Manage your documents and assets.</p>
        </div>
        <div className="relative">
          <input
            type="file"
            id="file-upload"
            className="hidden"
            onChange={handleFileUpload}
            disabled={isUploading}
          />
          <Button 
            onClick={() => document.getElementById("file-upload")?.click()}
            disabled={isUploading}
            className="bg-primary hover:bg-primary/90 transition-all duration-200"
          >
            {isUploading ? <Loader2 className="mr-2 h-4 w-4 animate-spin" /> : <UploadIcon className="mr-2 h-4 w-4" />}
            Upload File
          </Button>
        </div>
      </div>

      <Card className="bg-zinc-900 border-zinc-800 shadow-2xl overflow-hidden backdrop-blur-sm bg-opacity-50">
        <CardContent className="p-0">
          <Table>
            <TableHeader className="bg-zinc-800/50">
              <TableRow>
                <TableHead className="w-[400px]">Name</TableHead>
                <TableHead>Type</TableHead>
                <TableHead>Size</TableHead>
                <TableHead>Date</TableHead>
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {isLoading ? (
                <TableRow>
                  <TableCell colSpan={5} className="h-24 text-center">
                    <Loader2 className="h-6 w-6 animate-spin mx-auto text-primary" />
                  </TableCell>
                </TableRow>
              ) : files?.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={5} className="h-24 text-center text-muted-foreground">
                    No files found. Upload something to get started!
                  </TableCell>
                </TableRow>
              ) : (
                files?.map((file) => (
                  <TableRow key={file.id} className="hover:bg-zinc-800/30 transition-colors">
                    <TableCell className="font-medium flex items-center gap-3">
                      <FileIcon className="h-5 w-5 text-blue-400" />
                      {file.fileName}
                    </TableCell>
                    <TableCell className="text-muted-foreground">{file.contentType}</TableCell>
                    <TableCell>{formatSize(file.size)}</TableCell>
                    <TableCell className="text-muted-foreground">
                      {new Date(file.createdAt).toLocaleDateString()}
                    </TableCell>
                    <TableCell className="text-right space-x-2">
                      <Button 
                        variant="ghost" 
                        size="icon" 
                        onClick={() => processMutation.mutate(file)}
                        className="hover:text-blue-400 transition-colors"
                        title="Extract Text"
                      >
                        <CpuIcon className="h-4 w-4" />
                      </Button>
                      <Button 
                        variant="ghost" 
                        size="icon" 
                        onClick={() => window.open(`/api/files/${file.id}`, "_blank")}
                        className="hover:text-primary transition-colors"
                        title="Download"
                      >
                        <DownloadIcon className="h-4 w-4" />
                      </Button>
                      <Button 
                        variant="ghost" 
                        size="icon" 
                        onClick={() => deleteMutation.mutate(file.id)}
                        className="hover:text-destructive transition-colors"
                      >
                        <TrashIcon className="h-4 w-4" />
                      </Button>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
