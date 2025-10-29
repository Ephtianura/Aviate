"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { login, getUserMe } from "@/lib/api";
import { useAuth } from "@/context/AuthContext";
import { IoMdClose } from "react-icons/io";

interface LoginModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSwitchToRegister: () => void;
}

export const LoginModal: React.FC<LoginModalProps> = ({
  isOpen,
  onClose,
  onSwitchToRegister
}) => {
  const router = useRouter();
  const { setIsLoggedIn, setUserRole, setUserName,  } = useAuth();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const res = await login({ email, password });
      if (res.error) {
        setError(res.error);
        return;
      }

      const user = await getUserMe();
      if (!user) {
        setError("Не вдалося отримати дані користувача");
        return;
      }
      setUserName(user.fullName);

      setIsLoggedIn(true);
      setUserRole(user.role || null);
      onClose();
      window.location.href = "/";
    } catch (err: any) {
      setError(err?.data?.message || "Невідома помилка");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black/20 z-50">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-lg p-8 relative">
        <button
          onClick={onClose}
          className="absolute top-4 right-4 p-1 rounded-full hover:bg-gray-200"
        >
          <IoMdClose className="w-5 h-5 text-gray-500" />
        </button>

        <h2 className="text-2xl font-bold text-center mb-3">
          Увійти в профіль <span className="text-primary">Aviate</span>
        </h2>
        <p className="text-center text-gray-500 mb-6">
          Щоб стежити за цінами на потрібні квитки
        </p>

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full p-3 border rounded-xl outline-none"
            required
          />
          <input
            type="password"
            placeholder="Пароль"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full p-3 border rounded-xl outline-none"
            required
          />

          {error && <p className="text-red-500 text-sm">{error}</p>}

          <button
            type="submit"
            disabled={loading}
            className="w-full py-3 bg-primary text-white font-bold rounded-xl hover:bg-btn-hover cursor-pointer"
          >
            {loading ? "Входимо..." : "Увійти"}
          </button>
        </form>

        <div className="text-center mt-4">
          Немає акаунту?{" "}
          <button
            className="text-primary font-bold cursor-pointer"
            onClick={onSwitchToRegister}
          >
            Зареєструватися
          </button>
        </div>
      </div>
    </div>
  );
};
