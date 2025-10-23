"use client";

import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { AdminLayout } from "@/components/Layouts/AdminLayout";
import { apiFetch } from "@/lib/api";

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

    const statusMap: Record<number, string> = {
        0: "Доступний",
        1: "На ремонті",
        2: "Недоступний",
    };

    useEffect(() => {
        const fetchAirplanes = async () => {
            try {
                const res = await apiFetch("/admin/airplanes?PageSize=100");
                setAirplanes(res.items ?? []);
            } catch (err) {
                console.error("Error fetching airplanes", err);
            } finally {
                setLoading(false);
            }
        };
        fetchAirplanes();
    }, []);

    return (
        <AdminLayout>
            <h1 className="text-4xl font-extrabold mb-8 text-primary drop-shadow-sm">
                ✈️ Наші літаки
            </h1>

            <WhiteCard>
                {loading ? (
                    <p className="text-gray-500">Завантаження літаків...</p>
                ) : (
                    <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                        {airplanes.map((plane) => (
                            <div
                                key={plane.id}
                                className="border border-gray-200 rounded-lg p-4 hover:shadow-lg transition-shadow bg-white"
                            >
                                <h2 className="text-lg font-bold text-gray-800 mb-1">
                                    {plane.model}
                                </h2>
                                <p className="text-sm text-gray-500 mb-1">
                                    <span className="font-semibold">Реєстраційний №:</span> {plane.registrationNumber}
                                </p>
                                <p className="text-sm text-gray-500 mb-1">
                                    <span className="font-semibold">Місткість:</span> {plane.capacity} місць
                                </p>
                                <p className="text-sm text-gray-500 mb-1">
                                    <span className="font-semibold">Статус:</span> {statusMap[plane.status]}
                                </p>
                                <p className="text-sm text-gray-500">
                                    <span className="font-semibold">Дата виробництва:</span> {new Date(plane.manufactureDate).toLocaleDateString("uk-UA")}
                                </p>
                            </div>
                        ))}
                    </div>
                )}
            </WhiteCard>
        </AdminLayout>
    );
}
