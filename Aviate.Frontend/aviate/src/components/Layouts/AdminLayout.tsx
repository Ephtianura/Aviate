"use client";

import React, { useEffect } from "react";
import AdminSidebar from "@/components/Bars/AdminSidebar";
import { useAuth } from "@/context/AuthContext";
import { useRouter, usePathname } from "next/navigation";

interface AdminLayoutProps {
  children: React.ReactNode;
}

export const AdminLayout: React.FC<AdminLayoutProps> = ({ children }) => {
  const { isLoggedIn, userRole } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    if (!isLoggedIn) return;

    if (userRole === "Admin") return; // полный доступ

    // Employee — доступ только к flights
    const employeeAllowedPaths = ["/admin/flights", "/admin/dashboard"];
    if (userRole === "Employee" && employeeAllowedPaths.includes(pathname)) return;

    // всё остальное — редирект
    router.replace("/");
  }, [isLoggedIn, userRole, pathname, router]);

  if (isLoggedIn === null) {
    return (
      <div className="flex h-screen items-center justify-center text-gray-600">
        Перевірка доступу...
      </div>
    );
  }

  return (
    <div className="min-h-screen flex justify-center">
      <div className="w-full max-w-7xl p-10 mx-4 flex flex-col sm:flex-row gap-10 items-start">
        <AdminSidebar />
        <div className="w-full">{children}</div>
      </div>
    </div>
  );
};
