"use client";

import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { apiFetch } from "@/lib/api";
import AirplaneForm from "./AirplaneForm";
import { useToast } from "@/components/ToastProvider";

type Airplane = {
  id: string;
  model: string;
  registrationNumber: string;
  capacity: number;
  status: 0 | 1 | 2;
  manufactureDate: string;
};

export default function AirplanesPage() {
  const [airplanes, setAirplanes] = useState<Airplane[]>([]);
  const [loading, setLoading] = useState(true);
  const [editing, setEditing] = useState<Airplane | null>(null);

  const { success, error } = useToast();

  const statusMap: Record<number, string> = {
    0: "Доступний",
    1: "На ремонті",
    2: "Недоступний",
  };

  const fetchAirplanes = async () => {
    try {
      const res = await apiFetch("/admin/airplanes?PageSize=100");
      setAirplanes(res.items ?? []);
    } catch {
      error("Помилка завантаження літаків");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAirplanes();
  }, []);

  const reload = async () => {
    await fetchAirplanes();
  };

  const deletePlane = async (id: string) => {
    try {
      await apiFetch(`/admin/airplanes/${id}`, { method: "DELETE" });
      success("Літак видалено");
      await reload();
    } catch {
      error("Помилка видалення літака");
    }
  };

  return (
    <>
      <h1 className="text-4xl font-extrabold mb-8 text-primary">
        ✈️ Наші літаки
      </h1>

      <AirplaneForm
        key={editing?.id || "create"}
        airplaneToEdit={editing}
        onCancel={() => setEditing(null)}
        onSuccess={async () => {
          await reload();
          success(editing ? "Літак оновлено" : "Літак створено");
          setEditing(null);
        }}
      />

      <div className="my-4" />

      <WhiteCard>
        {loading ? (
          <p className="text-gray-500">Завантаження літаків...</p>
        ) : (
          <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {airplanes.map((plane) => (
              <div
                key={plane.id}
                className="border border-gray-200 rounded-lg p-4 hover:shadow-lg transition-shadow bg-white flex flex-col justify-between"
              >
                <div>
                  <h2 className="text-lg font-bold">{plane.model}</h2>

                  <p className="text-sm text-gray-500">
                    <b>№:</b> {plane.registrationNumber}
                  </p>

                  <p className="text-sm text-gray-500">
                    <b>Місткість:</b> {plane.capacity}
                  </p>

                  <p className="text-sm text-gray-500">
                    <b>Статус:</b> {statusMap[plane.status]}
                  </p>

                  <p className="text-sm text-gray-500">
                    <b>Дата:</b>{" "}
                    {new Date(plane.manufactureDate).toLocaleDateString("uk-UA")}
                  </p>
                </div>

                <div className="flex gap-2 mt-3">
                  <button
                    onClick={() => setEditing(plane)}
                    className="w-full bg-blue-400 text-white py-1 rounded hover:bg-blue-500"
                  >
                    Редагувати
                  </button>

                  <button
                    onClick={() => deletePlane(plane.id)}
                    className="w-full bg-red-300 text-white py-1 rounded hover:bg-red-400"
                  >
                    Видалити
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </WhiteCard>
    </>
  );
}