"use client";

import React, { createContext, useContext, useEffect, useState } from "react";
import Keycloak from "keycloak-js";
import { tokenManager } from "./tokenManager";
import { useAuthStore } from "../store/useAuthStore";

interface AuthContextType {
  initialized: boolean;
  login: () => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [initialized, setInitialized] = useState(false);
  const { setUser, clearUser } = useAuthStore();

  useEffect(() => {
    const keycloak = new Keycloak({
      url: process.env.NEXT_PUBLIC_KEYCLOAK_URL || "http://localhost:8080",
      realm: process.env.NEXT_PUBLIC_KEYCLOAK_REALM || "lex",
      clientId: process.env.NEXT_PUBLIC_KEYCLOAK_CLIENT_ID || "lex-web",
    });

    keycloak
      .init({
        onLoad: "check-sso",
        pkceMethod: "S256",
        silentCheckSsoRedirectUri: typeof window !== "undefined" ? `${window.location.origin}/silent-check-sso.html` : undefined,
      })
      .then((authenticated) => {
        if (authenticated) {
          tokenManager.setTokens({
            accessToken: keycloak.token!,
            refreshToken: keycloak.refreshToken!,
            idToken: keycloak.idToken!,
            expiresAt: (keycloak.tokenParsed?.exp ?? 0) * 1000,
          });

          setUser({
            userId: keycloak.tokenParsed?.sub ?? "",
            email: keycloak.tokenParsed?.email ?? "",
            roles: (keycloak.resourceAccess?.["lex-web"]?.roles ?? []) as string[],
          });
        } else {
          tokenManager.clearTokens();
          clearUser();
        }
        setInitialized(true);
      })
      .catch((err) => {
        console.error("Keycloak init failed", err);
        setInitialized(true);
      });

    keycloak.onTokenExpired = () => {
      keycloak.updateToken(70).then((refreshed) => {
        if (refreshed) {
          tokenManager.setTokens({
            accessToken: keycloak.token!,
            refreshToken: keycloak.refreshToken!,
            idToken: keycloak.idToken!,
            expiresAt: (keycloak.tokenParsed?.exp ?? 0) * 1000,
          });
        }
      });
    };
  }, [setUser, clearUser]);

  const login = () => {
    // This is a simplified version, ideally you'd use the keycloak instance from state
    window.location.href = "/auth/login"; // Or trigger keycloak.login()
  };

  const logout = () => {
    tokenManager.clearTokens();
    clearUser();
    // Trigger keycloak logout if needed
  };

  return (
    <AuthContext.Provider value={{ initialized, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within an AuthProvider");
  return context;
};
