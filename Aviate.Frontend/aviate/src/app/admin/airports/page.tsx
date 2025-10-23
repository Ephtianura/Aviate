"use client";

import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { AdminLayout } from "@/components/Layouts/AdminLayout";
import { apiFetch } from "@/lib/api";

type Airport = {
    id: string;
    name: string;
    code: string;
    country: string;
    city: string;
};

export default function AirportsPage() {
    const [airports, setAirports] = useState<Airport[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchAirports = async () => {
            try {
                // PageSize можно добавить прямо в URL
                const res = await apiFetch("/airports?PageSize=100");
                setAirports(res.items ?? []);
            } catch (err) {
                console.error("Error fetching airports", err);
            } finally {
                setLoading(false);
            }
        };
        fetchAirports();
    }, []);

    return (
        <AdminLayout>
            <h1 className="text-4xl font-extrabold mb-8 text-primary drop-shadow-sm">
                🛫 Аеропорти, які ми обслуговуємо
            </h1>

            <WhiteCard>
                {loading ? (
                    <p className="text-gray-500">Завантаження аеропортів...</p>
                ) : (
                    <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                        {airports.map((airport) => (
                            <div
                                key={airport.id}
                                className="border border-gray-200 rounded-lg p-4 hover:shadow-lg transition-shadow bg-white"
                            >
                                <h2 className="text-lg font-bold text-gray-800 mb-1">
                                    {airport.name}
                                </h2>
                                <p className="text-sm text-gray-500 mb-1">
                                    <span className="font-semibold">Код:</span> {airport.code}
                                </p>
                                <p className="text-sm text-gray-500">
                                    <span className="font-semibold">Місто:</span> {airport.city}
                                </p>
                                <p className="text-sm text-gray-500">
                                    <span className="font-semibold">Країна:</span> {airport.country}
                                </p>
                            </div>
                        ))}
                    </div>
                )}
            </WhiteCard>
        </AdminLayout>
    );
}
