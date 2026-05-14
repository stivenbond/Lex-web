import type { Metadata } from "next";
import { Inter, Noto_Sans, Playfair_Display } from "next/font/google";
import "@/styles/globals.css";
const inter = Inter({ subsets: ["latin"] });
export const metadata: Metadata = { title: { default: "Lex", template: "%s | Lex" }, manifest: "/manifest.webmanifest" };
import { AuthProvider } from "@/lib/auth/AuthContext";
import { SignalRProvider } from "@/lib/auth/SignalRProvider";
import { cn } from "@/lib/utils";

const playfairDisplayHeading = Playfair_Display({subsets:['latin'],variable:'--font-heading'});

const notoSans = Noto_Sans({subsets:['latin'],variable:'--font-sans'});


export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" className={cn("font-sans", notoSans.variable, playfairDisplayHeading.variable)}>
      <body className={inter.className}>
        <AuthProvider>
          <SignalRProvider>
            {children}
          </SignalRProvider>
        </AuthProvider>
      </body>
    </html>
  );
}
