"use client";

import { useState } from "react";
import { register, login, getUserMe } from "@/lib/api";
import { useAuth } from "@/context/AuthContext";
import { IoMdClose } from "react-icons/io";
import { useRouter } from "next/navigation";

interface RegisterModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSwitchToLogin: () => void;
}

export const RegisterModal: React.FC<RegisterModalProps> = ({
  isOpen,
  onClose,
  onSwitchToLogin,
}) => {
  const router = useRouter();
  const { setIsLoggedIn, setUserRole } = useAuth();

  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<any>(null);
  const [generalError, setGeneralError] = useState<string | null>(null);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setErrors(null);
    setGeneralError(null);

    const res = await register({ fullName, email, password });

    if (res.error) {
      if (res.validation) setErrors(res.validation);
      else setGeneralError(res.message || "Помилка");

      setLoading(false);
      return;
    }

    // auto login after registration
    await login({ email, password });
    const user = await getUserMe().catch(() => null);

    if (user) {
      setIsLoggedIn(true);
      setUserRole(user.role || null);
    }

    onClose();
    router.refresh();
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black/20">
      <div className="w-full max-w-md bg-white border border-white/30 shadow-lg rounded-2xl p-10 relative">
        <button
          onClick={onClose}
          className="absolute top-4 right-4 cursor-pointer"
        >
          <div className="bg-gray-lightes p-1 rounded-full hover:bg-gray-200">
            <IoMdClose className="w-5 h-5 text-gray-500" />
          </div>
        </button>

        <h1 className="text-2xl font-bold text-center mb-4 text-primary-black">
          Створити акаунт <span className="text-primary">Aviate</span>
        </h1>

        <form onSubmit={handleSubmit} className="space-y-4">
          
          {/* Full name */}
          <div>
            <label className="block text-sm mb-1">Ваше імʼя</label>
            <input
              type="text"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              required
              className="w-full p-3 border rounded-xl"
            />
            {errors?.FullName && (
              <p className="text-red-500 text-sm">{errors.FullName[0]}</p>
            )}
          </div>

          {/* Email */}
          <div>
            <label className="block text-sm mb-1">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="w-full p-3 border rounded-xl"
            />
            {errors?.Email && (
              <p className="text-red-500 text-sm">{errors.Email[0]}</p>
            )}
          </div>

          {/* Password */}
          <div>
            <label className="block text-sm mb-1">Пароль</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="w-full p-3 border rounded-xl"
            />
            {errors?.Password && (
              <p className="text-red-500 text-sm">{errors.Password[0]}</p>
            )}
          </div>

          {generalError && (
            <p className="text-red-500 text-center text-sm">{generalError}</p>
          )}

          <button
            type="submit"
            disabled={loading}
            className="w-full py-3 bg-primary text-white rounded-xl mt-3 cursor-pointer"
          >
            {loading ? "Створюємо..." : "Зареєструватися"}
          </button>
        </form>

        <div className="text-center mt-6 text-sm text-gray-600">
          Вже маєте акаунт?{" "}
          <button
            className="text-primary hover:text-btn-hover cursor-pointer"
            onClick={onSwitchToLogin}
          >
            Увійти
          </button>
        </div>
      </div>
    </div>
  );
};
