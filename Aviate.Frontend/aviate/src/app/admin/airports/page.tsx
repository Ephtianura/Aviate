"use client";

import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { apiFetch } from "@/lib/api";
import AirportForm from "./AirportForm";
import { useToast } from "@/components/ToastProvider";
import Pagination from "@/components/Pagination";
import AirportsList from "./AirportsList";

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
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const pageSize = 12;
    const fetchAirports = async (pageNumber = 1) => {
        try {
            setLoading(true);

            const res = await apiFetch(
                `/airports?Page=${pageNumber}&PageSize=12`
            );

            setAirports(res.items ?? []);
            setTotalPages(res.totalPages ?? 1);
        } catch {
            error("Помилка завантаження аеропортів");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAirports(page);
    }, [page]);

    const reload = async () => {
        await fetchAirports();
    };

    const deleteAirport = async (id: string) => {
        try {
            await apiFetch(`/admin/airports/${id}`, {
                method: "DELETE",
            });

            success("Аеропорт видалено");
            await fetchAirports(page);
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
            <AirportsList
                loading={loading}
                airports={airports}
                page={page}
                totalPages={totalPages}
                setPage={setPage}
                setEditingAirport={setEditingAirport}
                deleteAirport={deleteAirport}
            />
        </>
    );
}