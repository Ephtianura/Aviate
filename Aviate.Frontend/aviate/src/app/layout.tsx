import "./globals.css";
import Navbar from "@/components/Navbar";
import Footer from "@/components/Footer";

import { AuthProvider } from "@/context/AuthContext";

export const metadata = {
  title: "Aviate",
  description: "Найдешевші авіаквитки онлайн",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="uk">
      <body className="bg-gray-100 transition-colors font-stapel">
        <AuthProvider>
          <div className="bg-gray-50 min-h-screen flex flex-col">
            <Navbar />
            <main className="flex-1">{children}</main>
            <Footer />
          </div>
        </AuthProvider>
      </body>
    </html>
  );
}
