"use client";


import { ReactNode } from "react";
import Sidebar from "@/components/Bars/SidebarProfile";

type DashboardLayoutProps = {
  children: ReactNode;
  noContainer?: boolean; // новый пропс
};

export default function DashboardLayout({ children, noContainer }: DashboardLayoutProps) {
  return (
    <div className="flex min-h-screen bg-[var(--color-bg-light)] text-[var(--color-gray-text)]">
      {/* Sidebar */}
      <Sidebar />

      {/* Content */}
      <main className="flex-1 p-6 md:p-10 overflow-y-auto transition-all duration-300">
        {noContainer ? (
          children
        ) : (
          <div className="bg-[var(--color-bg)]/80 backdrop-blur-md rounded-2xl shadow-lg p-8 border border-[var(--color-bg)]/20">
            {children}
          </div>
        )}
      </main>
    </div>
  );
}
