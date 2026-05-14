"use client";

import { createContext, useContext, useState } from "react";
import { AnimatePresence, motion } from "framer-motion";
import { GiConfirmed } from "react-icons/gi";
import { MdOutlineErrorOutline } from "react-icons/md";

type ToastType = "success" | "error";

type Toast = {
  id: number;
  message: string;
  type: ToastType;
};

type ToastContextType = {
  success: (msg: string) => void;
  error: (msg: string) => void;
};

const ToastContext = createContext<ToastContextType | null>(null);

export function useToast() {
  const ctx = useContext(ToastContext);
  if (!ctx) throw new Error("useToast must be used inside ToastProvider");
  return ctx;
}

export function ToastProvider({ children }: { children: React.ReactNode }) {
  const [toasts, setToasts] = useState<Toast[]>([]);

  const removeToast = (id: number) => {
    setToasts((prev) => prev.filter((t) => t.id !== id));
  };

  const pushToast = (message: string, type: ToastType) => {
    const id = Date.now();

    setToasts((prev) => [...prev, { id, message, type }]);

    setTimeout(() => removeToast(id), 4000);
  };

  const api = {
    success: (msg: string) => pushToast(msg, "success"),
    error: (msg: string) => pushToast(msg, "error"),
  };

  return (
    <ToastContext.Provider value={api}>
      {children}

      <AnimatePresence>
        {toasts.map((toast) => (
          <motion.div
            key={toast.id}
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -20 }}
            transition={{ duration: 0.25 }}
            className={`fixed top-10 left-1/2 -translate-x-1/2 -translate-y-1/2 px-6 py-4 rounded-lg flex items-center gap-3 shadow-lg z-[9999]
              ${
                toast.type === "success"
                  ? "bg-green-100 border border-green-400 text-green-800"
                  : "bg-red-100 border border-red-400 text-red-800"
              }`}
          >
            {toast.type === "success" ? (
              <GiConfirmed className="w-6 h-6" />
            ) : (
              <MdOutlineErrorOutline className="w-6 h-6" />
            )}

            <span>{toast.message}</span>

            <button onClick={() => removeToast(toast.id)}>✕</button>
          </motion.div>
        ))}
      </AnimatePresence>
    </ToastContext.Provider>
  );
}