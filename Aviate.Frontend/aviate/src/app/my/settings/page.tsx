"use client";

import { useEffect, useState } from "react";
import { useAuth } from "@/context/AuthContext";
import WhiteCard from "@/components/Cards/WhiteCard";
import { ProfileLayout } from "@/components/Layouts/ProfileLayout";
import { LuLogIn, LuLogOut } from "react-icons/lu";
import { CiSaveDown2 } from "react-icons/ci";
import { getUserMe, logout, updateUserProfile } from "@/lib/api";
import { LoginModal } from "@/components/Modals/LoginModal";
import { RegisterModal } from "@/components/Modals/RegisterModal";

interface UserProfile {
  fullName: string;
  email: string;
  password?: string;
  phone: string;
}

export default function Settings() {
   const { isLoggedIn, setIsLoggedIn, userRole, userName, setUserRole } = useAuth();

  // Текущее состояние пользователя (редактируемые поля)
  const [user, setUser] = useState<UserProfile>({
    fullName: "",
    email: "",
    phone: "",
    password: "",
  });

  // Оригинальные данные пользователя, чтобы сравнивать изменения
  const [originalUser, setOriginalUser] = useState<UserProfile>({
    fullName: "",
    email: "",
    phone: "",
    password: "",
  });

  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const [isLoginOpen, setLoginOpen] = useState(false);
  const [isRegisterOpen, setRegisterOpen] = useState(false);
  const handleLogout = async () => {
    try {
      await logout();
    } catch (err: any) {
      if (err.status !== 401) {
        console.error("Logout error:", err);
      }
    } finally {
      setIsLoggedIn(false);
      setUserRole(null);
      window.location.href = "/";
    }
  };
  // Загружаем данные пользователя при монтировании
  useEffect(() => {
    if (!isLoggedIn) return;

    const fetchUser = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await getUserMe();
        const profile: UserProfile = {
          fullName: data.fullName || "",
          email: data.email || "",
          phone: data.phone || "",
          password: "",
        };
        setUser(profile);
        setOriginalUser(profile);
      } catch (err) {
        console.error(err);
        setError("Не удалось загрузить данные пользователя");
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, [isLoggedIn]);

  // Обновление локального состояния при изменении инпутов
  const handleChange = (field: keyof UserProfile, value: string) => {
    setUser((prev) => ({ ...prev, [field]: value }));
  };

  // Сохранение изменений
  const handleSave = async () => {
    setSaving(true);
    setError(null);
    setSuccess(null);

    try {
      const changedData: Partial<UserProfile> = {};

      if (user.fullName !== originalUser.fullName) changedData.fullName = user.fullName;
      if (user.email !== originalUser.email) changedData.email = user.email;
      if (user.phone !== originalUser.phone) changedData.phone = user.phone;
      if (user.password && user.password.trim() !== "") changedData.password = user.password;

      if (Object.keys(changedData).length === 0) {
        setSuccess("Нічого не змінилось");
        return;
      }

      // Логируем изменения перед отправкой
      console.log("Изменения для сохранения:", changedData);

      await updateUserProfile(changedData);

      setOriginalUser({ ...originalUser, ...changedData, password: "" });
      setUser((prev) => ({ ...prev, password: "" }));

      setSuccess("Дані успішно збережено!");
    } catch (err: any) {
      console.error("Ошибка при сохранении:", err);
      setError(err.message || "Помилка при збереженні даних");
    } finally {
      setSaving(false);
    }
  };

  return (
    <ProfileLayout>
      <div className="flex flex-col gap-4">
        {/* Заголовок */}
        <div className="mb-2">
          <h1 className="text-4xl font-bold text-primary-black">Налаштування</h1>
        </div>

        {/* Особисті дані */}
        <WhiteCard>
          {loading ? (
            <p>Завантаження...</p>
          ) : (
            <div className="flex flex-col gap-4">

              <h1 className="text-primary-black text-xl font-bold">
                Особиста інформація
              </h1>

              {/* Ім'я */}
              <div className="w-full border-[#D8DADD] border rounded-lg py-3 px-4">
                <input
                  type="text"
                  placeholder="Ім'я"
                  className="w-full text-gray-900  focus:outline-none"
                  value={user.fullName}
                  onChange={(e) => handleChange("fullName", e.target.value)}
                />
              </div>

              {/* Номер та пошта */}
              <div className="flex gap-3">
                <div className="w-full border-[#D8DADD] border rounded-lg py-3 px-4">
                  <input
                    type="tel"
                    placeholder="Номер"
                    className="w-full text-gray-900  focus:outline-none"
                    value={user.phone}
                    onChange={(e) => handleChange("phone", e.target.value)}
                  />
                </div>
                <div className="w-full border-[#D8DADD] border rounded-lg py-3 px-4">
                  <input
                    type="email"
                    placeholder="Пошта"
                    className="w-full text-gray-900  focus:outline-none"
                    value={user.email}
                    onChange={(e) => handleChange("email", e.target.value)}
                  />
                </div>
              </div>

              {/* Пароль */}
              <div className="w-full border-[#D8DADD] border rounded-lg py-3 px-4 ">
                <input
                  type="password"
                  placeholder="Пароль"
                  className="w-full text-gray-900 focus:outline-none "
                  value={user.password}
                  onChange={(e) => handleChange("password", e.target.value)}
                  autoComplete="new-password"
                />
              </div>

              {error && <p className="text-red-500 text-sm">{error}</p>}
              {success && <p className="text-green-500 text-sm">{success}</p>}


            </div>
          )}
        </WhiteCard>
        {/* Кнопка зберегти */}
        <button
          className="bg-blue-300 text-primary-black px-4 py-2 rounded-xl hover:bg-blue-400 font-bold flex items-center gap-2 justify-center w-full transition-colors duration-200"
          onClick={handleSave}
          disabled={saving}
        >
          <CiSaveDown2 className="w-6 h-6" />
          {saving ? "Збереження..." : "Зберегти"}
        </button>
        {/* Вихід */}
        <div>
          {isLoggedIn ? (
            <button
              onClick={handleLogout}
              className="bg-gray-200 text-primary-black px-4 py-2 rounded-xl hover:bg-gray-300 font-bold w-full flex items-center gap-2 justify-center transition-colors duration-200"
            >
              <LuLogOut className="w-5 h-5" /> Вийти
            </button>
          ) : (
            <button
              onClick={() => setLoginOpen(true)}
              className="bg-[#1071F2] hover:bg-[#1172F3] active:bg-[#0E64DB] text-white font-bold
              rounded-xl px-3 py-2 w-full flex items-center gap-2 justify-center transition-colors duration-200"
            >
              <LuLogIn className="w-5 h-5" /> Увійти
            </button>
          )}
        </div>

        {/* Модалки */}
        <LoginModal
          isOpen={isLoginOpen}
          onClose={() => setLoginOpen(false)}
          onSwitchToRegister={() => {
            setLoginOpen(false);
            setRegisterOpen(true);
          }}
        />
        <RegisterModal
          isOpen={isRegisterOpen}
          onClose={() => setRegisterOpen(false)}
          onSwitchToLogin={() => {
            setRegisterOpen(false);
            setLoginOpen(true);
          }}
        />
      </div>
    </ProfileLayout>
  );
}

