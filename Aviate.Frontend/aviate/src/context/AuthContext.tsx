"use client";

import { createContext, useContext, useState, useEffect } from "react";
import { useRouter, usePathname } from "next/navigation";
import { getUserMe } from "@/lib/api";

type AuthContextType = {
  isLoggedIn: boolean;
  setIsLoggedIn: (value: boolean) => void;
  userRole: string | null;
  setUserRole: (role: string | null) => void;
  userName: string | null;
  setUserName: (name: string | null) => void;
  refreshAuth: () => Promise<void>;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [isLoggedIn, setIsLoggedIn] = useState<boolean >(false);
  const [userRole, setUserRole] = useState<string | null>(null);
  const [userName, setUserName] = useState<string | null>(null);

  const router = useRouter();
  const pathname = usePathname();

  // Загружаем данные пользователя при инициализации
const refreshAuth = async () => {
  try {
    const user = await getUserMe();
    setIsLoggedIn(true);

    let roleStr: string | null = null;
    switch (user.role) {
      case 1:
        roleStr = "Employee";
        break;
      case 2:
        roleStr = "Admin";
        break;
      default:
        roleStr = null;
    }

    setUserRole(roleStr);
    setUserName(user.fullName || null);
  } catch (err: any) {
    if (err.status === 401) {
      setIsLoggedIn(false);
      setUserRole(null);
      setUserName(null);
      return;
    }
    setIsLoggedIn(false);
    setUserRole(null);
    setUserName(null);
  }
};

  useEffect(() => {
    refreshAuth();
  }, []);


  return (
    <AuthContext.Provider
      value={{
        isLoggedIn,
        setIsLoggedIn,
        userRole,
        setUserRole,
        userName,
        setUserName,
        refreshAuth,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within AuthProvider");
  return context;
};
