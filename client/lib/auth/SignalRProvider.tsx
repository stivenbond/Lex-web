"use client";

import React, { createContext, useContext, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { tokenManager } from "./tokenManager";
import { useSignalRStore } from "../store/useSignalRStore";

interface SignalRContextType {
  connection: signalR.HubConnection | null;
}

const SignalRContext = createContext<SignalRContextType | null>(null);

export const SignalRProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const { setStatus } = useSignalRStore();

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("/hubs/jobs", {
        accessTokenFactory: () => tokenManager.accessToken || "",
      })
      .withAutomaticReconnect()
      .build();

    newConnection.onreconnecting(() => setStatus("reconnecting"));
    newConnection.onreconnected(() => setStatus("connected"));
    newConnection.onclose(() => setStatus("disconnected"));

    const startConnection = async () => {
      try {
        await newConnection.start();
        console.log("SignalR Connected");
        setStatus("connected");
        setConnection(newConnection);
      } catch (err) {
        console.error("SignalR connection failed: ", err);
        setStatus("disconnected");
        setTimeout(startConnection, 5000);
      }
    };

    startConnection();

    return () => {
      newConnection.stop();
    };
  }, [setStatus]);

  return (
    <SignalRContext.Provider value={{ connection }}>
      {children}
    </SignalRContext.Provider>
  );
};

export const useSignalR = () => {
  const context = useContext(SignalRContext);
  if (!context) throw new Error("useSignalR must be used within a SignalRProvider");
  return context;
};
