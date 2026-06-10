"use client";

import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { useToast } from "@/components/ToastProvider";

type Airplane = {
  id: string;
  model: string;
  registrationNumber: string;
  capacity: number;
  status: 0 | 1 | 2;
  manufactureDate: string;
};

interface Props {
  airplaneToEdit?: Airplane | null;
  onSuccess?: (a: Airplane) => void;
  onCancel?: () => void;
}

export default function AirplaneForm({
  airplaneToEdit,
  onSuccess,
  onCancel,
}: Props) {
  const [form, setForm] = useState({
    model: "",
    registrationNumber: "",
    capacity: 0,
    status: 0 as 0 | 1 | 2,
    manufactureDate: "",
  });

  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<Record<string, string[]>>({});

  const { error, success } = useToast();

  useEffect(() => {
    if (airplaneToEdit) {
      setForm({
        model: airplaneToEdit.model,
        registrationNumber: airplaneToEdit.registrationNumber,
        capacity: airplaneToEdit.capacity,
        status: airplaneToEdit.status,
        manufactureDate: airplaneToEdit.manufactureDate?.split("T")[0] ?? "",
      });
    } else {
      setForm({
        model: "",
        registrationNumber: "",
        capacity: 0,
        status: 0,
        manufactureDate: "",
      });
    }
  }, [airplaneToEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setForm((p) => ({
      ...p,
      [e.target.name]:
        e.target.name === "capacity" || e.target.name === "status"
          ? Number(e.target.value)
          : e.target.value,
    }));
  };

  const handleSubmit = async () => {
    setLoading(true);
    setErrors({});

    try {
      const isEdit = !!airplaneToEdit;

      const url = isEdit
        ? `${process.env.NEXT_PUBLIC_API_URL}/admin/airplanes/${airplaneToEdit!.id}`
        : `${process.env.NEXT_PUBLIC_API_URL}/admin/airplanes`;

      const res = await fetch(url, {
        method: isEdit ? "PUT" : "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(form),
      });

      if (!res.ok) {
        const err = await res.json().catch(() => ({}));

        if (err?.errors)
          setErrors(err.errors);
        error("Помилка збереження літака. Перевірте заповнення полів.");

        return;
      }

      const data = await res.json().catch(() => null);
      onSuccess?.(data);
      // success("Успішно!");
    } catch {
      error("Щось пішло не так");
    } finally {
      setLoading(false);
    }
  };

  const input = "border border-gray-300 rounded px-3 py-2 w-full";

  return (
    <WhiteCard>
      <h2 className="text-xl font-bold mb-4">
        {airplaneToEdit ? "Редагувати літак" : "Створити літак"}
      </h2>

      <div className="grid md:grid-cols-2 gap-4">
        <input className={input} name="model" value={form.model} onChange={handleChange} placeholder="Модель" />

        <input
          className={input}
          name="registrationNumber"
          value={form.registrationNumber}
          onChange={handleChange}
          placeholder="Реєстраційний номер"
        />

        <input
          className={input}
          type="number"
          name="capacity"
          value={form.capacity}
          onChange={handleChange}
          placeholder="Місткість"
        />

        <select className={input} name="status" value={form.status} onChange={handleChange}>
          <option value={0}>Доступний</option>
          <option value={1}>На ремонті</option>
          <option value={2}>Недоступний</option>
        </select>

        <input
          className={input}
          type="date"
          name="manufactureDate"
          value={form.manufactureDate}
          onChange={handleChange}
        />
      </div>

      <div className="flex gap-2 mt-4">
        <button
          onClick={handleSubmit}
          disabled={loading}
          className="px-6 py-2 bg-primary text-white rounded"
        >
          {loading ? "Завантаження..." : airplaneToEdit ? "Оновити" : "Створити"}
        </button>

        {airplaneToEdit && (
          <button onClick={onCancel} className="px-4 py-2 border rounded">
            Скасувати
          </button>
        )}
      </div>
    </WhiteCard>
  );
}