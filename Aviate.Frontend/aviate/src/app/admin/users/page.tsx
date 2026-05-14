"use client";

import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { apiFetch } from "@/lib/api";
import { useToast } from "@/components/ToastProvider";
import { useAuth } from "@/context/AuthContext";
import { redirect } from "next/navigation";

type User = {
  id: string;
  fullName: string;
  email: string;
  phone: string | null;
  role: number;
  registrationDate: string;
};

const roleMap: Record<number, string> = {
  0: "User",
  1: "Employee",
  2: "Admin",
};

export default function UsersPage() {
  const { userRole } = useAuth();
  if (userRole === "Employee")
    redirect("/admin/flights");
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);

  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [form, setForm] = useState({
    fullName: "",
    email: "",
    phone: "",
    role: 0,
  });

  const { success, error } = useToast();

  const fetchUsers = async () => {
    try {
      const res = await apiFetch("/admin/users?PageSize=100");
      setUsers(res.items ?? []);
    } catch {
      error("Не вдалося завантажити користувачів");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const openEdit = (user: User) => {
    setEditingUser(user);
    setForm({
      fullName: user.fullName ?? "",
      email: user.email ?? "",
      phone: user.phone ?? "",
      role: user.role,
    });
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      [name]: name === "role" ? Number(value) : value,
    }));
  };

  const updateUser = async () => {
    try {
      await apiFetch(`/admin/users/${editingUser?.id}`, {
        method: "PUT",
        body: JSON.stringify(form),
      });

      success("Користувача оновлено");
      setEditingUser(null);
      fetchUsers();
    } catch {
      error("Помилка оновлення користувача");
    }
  };

  const deleteUser = async (id: string) => {
    try {
      await apiFetch(`/admin/users/${id}`, {
        method: "DELETE",
      });

      success("Користувача видалено");
      fetchUsers();
    } catch {
      error("Помилка видалення");
    }
  };

  return (
    <div className="">
      <h1 className="text-3xl font-bold mb-6">👤 Користувачі</h1>

      <WhiteCard>
        {loading ? (
          <p>Завантаження...</p>
        ) : (
          <div className="grid gap-4">
            {users.map((u) => (
              <div
                key={u.id}
                className="border border-primary p-4 rounded flex justify-between items-center"
              >
                <div>
                  <p className="font-bold">{u.fullName}</p>
                  <p className="text-sm text-gray-500">{u.email}</p>
                  <p className="text-sm">
                    Роль: <b>{roleMap[u.role]}</b>
                  </p>
                </div>

                <div className="flex gap-2">
                  <button
                    onClick={() => openEdit(u)}
                    className="px-3 py-1 bg-blue-500 text-white rounded"
                  >
                    Редагувати
                  </button>

                  <button
                    onClick={() => deleteUser(u.id)}
                    className="px-3 py-1 bg-red-300 text-white rounded"
                  >
                    Видалити
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </WhiteCard>

      {/* MODAL */}
      {editingUser && (
        <div className="fixed inset-0 bg-black/40 flex items-center justify-center">
          <div className="bg-white p-6 rounded w-[400px]">
            <h2 className="text-xl font-bold mb-4">Редагування</h2>

            <input
              className="border border-primary w-full p-2 mb-2"
              name="fullName"
              value={form.fullName}
              onChange={handleChange}
              placeholder="Ім'я"
            />

            <input
              className="border border-primary w-full p-2 mb-2"
              name="email"
              value={form.email}
              onChange={handleChange}
              placeholder="Email"
            />

            <input
              className="border border-primary w-full p-2 mb-2"
              name="phone"
              value={form.phone}
              onChange={handleChange}
              placeholder="Телефон"
            />

            <select
              className="border border-primary w-full p-2 mb-4"
              name="role"
              value={form.role}
              onChange={handleChange}
            >
              <option value={0}>User</option>
              <option value={1}>Employee</option>
              <option value={2}>Admin</option>
            </select>

            <div className="flex justify-end gap-2">
              <button
                onClick={() => setEditingUser(null)}
                className="px-3 py-1"
              >
                Скасувати
              </button>

              <button
                onClick={updateUser}
                className="px-3 py-1 bg-green-500 text-white rounded"
              >
                Зберегти
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}