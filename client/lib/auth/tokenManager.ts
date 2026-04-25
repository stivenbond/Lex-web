export interface AuthTokens { accessToken: string; refreshToken: string; idToken: string; expiresAt: number; }

class TokenManager {
  private readonly STORAGE_KEY = "lex_auth_tokens";

  getTokens(): AuthTokens | null {
    if (typeof window === "undefined") return null;
    const stored = localStorage.getItem(this.STORAGE_KEY);
    if (!stored) return null;
    try {
      return JSON.parse(stored);
    } catch {
      return null;
    }
  }

  setTokens(tokens: AuthTokens): void {
    if (typeof window === "undefined") return;
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(tokens));
  }

  clearTokens(): void {
    if (typeof window === "undefined") return;
    localStorage.removeItem(this.STORAGE_KEY);
  }

  get accessToken(): string | null {
    const tokens = this.getTokens();
    if (!tokens) return null;
    if (Date.now() >= tokens.expiresAt - 30000) return null; // Expired or expiring within 30s
    return tokens.accessToken;
  }
}

export const tokenManager = new TokenManager();
