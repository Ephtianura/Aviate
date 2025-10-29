"use client";

import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { apiFetch } from "@/lib/api";
import AirportForm from "./AirportForm";
import { useToast } from "@/components/ToastProvider";

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
    const [editingAirport, setEditingAirport] = useState<Airport | null>(null);

    const { success, error } = useToast();

    const fetchAirports = async () => {
        try {
            const res = await apiFetch("/airports?PageSize=100");
            setAirports(res.items ?? []);
        } catch {
            error("Помилка завантаження аеропортів");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAirports();
    }, []);

    const reload = async () => {
        await fetchAirports();
    };

    const deleteAirport = async (id: string) => {
        try {
            await apiFetch(`/admin/airports/${id}`, {
                method: "DELETE",
            });

            success("Аеропорт видалено");
            await reload();
        } catch {
            error("Помилка видалення аеропорту");
        }
    };

    const handleUpdated = async () => {
        await reload();
        success("Аеропорт оновлено");
        setEditingAirport(null);
    };

    const handleCreated = async () => {
        await reload();
        success("Аеропорт створено");
    };

    return (
        <>
            <h1 className="text-4xl font-extrabold mb-8 text-primary">
                🛫 Аеропорти
            </h1>

            <AirportForm
                key={editingAirport?.id || "create"}
                airportToEdit={editingAirport}
                onCancel={() => setEditingAirport(null)}
                onSuccess={async () => {
                    if (editingAirport) {
                        await handleUpdated();
                    } else {
                        await handleCreated();
                    }
                }}
            />

            <div className="my-4" />

            <WhiteCard>
                {loading ? (
                    <p className="text-gray-500">Завантаження аеропортів...</p>
                ) : (
                    <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                        {airports.map((airport) => (
                            <div
                                key={airport.id}
                                className="border flex flex-col justify-between border-gray-200 rounded-lg p-4 hover:shadow-lg transition-shadow bg-white"
                            >
                                <div>
                                    <h2 className="text-lg font-bold">{airport.name}</h2>
                                    <p className="text-sm text-gray-500">
                                        <b>Код:</b> {airport.code}
                                    </p>
                                    <p className="text-sm text-gray-500">
                                        <b>Місто:</b> {airport.city}
                                    </p>
                                    <p className="text-sm text-gray-500">
                                        <b>Країна:</b> {airport.country}
                                    </p>
                                </div>

                                <div className="flex gap-2 mt-3">
                                    <button
                                        onClick={() => setEditingAirport(airport)}
                                        className="w-full bg-blue-400 text-white py-1 rounded hover:bg-blue-500"
                                    >
                                        Редагувати
                                    </button>

                                    <button
                                        onClick={() => deleteAirport(airport.id)}
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