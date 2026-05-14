"use client";

import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { apiFetch } from "@/lib/api";
import { useToast } from "@/components/ToastProvider";
import Pagination from "@/components/Pagination";
import { getAirplanes } from "@/hooks/apiFlights";

type Airplane = {
    id: string;
    model: string;
    registrationNumber: string;
    capacity: number;
    status: 0 | 1 | 2;
    manufactureDate: string;
};

interface Props {
    onEdit: (plane: Airplane) => void;
}

export default function AirplaneList({ onEdit }: Props) {
    const [airplanes, setAirplanes] = useState<Airplane[]>([]);
    const [loading, setLoading] = useState(true);

    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    const { success, error } = useToast();

    const pageSize = 12;

    const statusMap: Record<number, string> = {
        0: "Доступний",
        1: "На ремонті",
        2: "Недоступний",
    };

    const fetchAirplanes = async (pageNumber = 1) => {
        try {
            setLoading(true);

            const res = await getAirplanes(pageNumber, pageSize);

            setAirplanes(res.items ?? []);
            setTotalPages(res.totalPages ?? 1);
        } catch {
            error("Помилка завантаження літаків");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchAirplanes(page);
    }, [page]);

    const deletePlane = async (id: string) => {
        try {
            await apiFetch(`/admin/airplanes/${id}`, { method: "DELETE" });
            success("Літак видалено");
            fetchAirplanes(page);
        } catch {
            error("Помилка видалення літака");
        }
    };

    return (
        <WhiteCard>
            <div className="mb-4">
                <Pagination
                    page={page}
                    totalPages={totalPages}
                    onPageChange={setPage}
                />
            </div>
            {loading ? (
                <p className="text-gray-500">Завантаження літаків...</p>
            ) : (
                <>
                    <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                        {airplanes.map((plane) => (
                            <div
                                key={plane.id}
                                className="border rounded-lg p-4 bg-white flex flex-col justify-between"
                            >
                                <div>
                                    <h2 className="text-lg font-bold">{plane.model}</h2>

                                    <p className="text-sm text-gray-500">
                                        №: {plane.registrationNumber}
                                    </p>

                                    <p className="text-sm text-gray-500">
                                        Місткість: {plane.capacity}
                                    </p>

                                    <p className="text-sm text-gray-500">
                                        Статус: {statusMap[plane.status]}
                                    </p>

                                    <p className="text-sm text-gray-500">
                                        Дата:{" "}
                                        {new Date(plane.manufactureDate).toLocaleDateString(
                                            "uk-UA"
                                        )}
                                    </p>
                                </div>

                                <div className="flex gap-2 mt-3">
                                    <button
                                        onClick={() => onEdit(plane)}
                                        className="w-full bg-blue-400 text-white py-1 rounded"
                                    >
                                        Редагувати
                                    </button>

                                    <button
                                        onClick={() => deletePlane(plane.id)}
                                        className="w-full bg-red-300 text-white py-1 rounded"
                                    >
                                        Видалити
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="mt-4">
                        <Pagination
                            page={page}
                            totalPages={totalPages}
                            onPageChange={setPage}
                        />
                    </div>

                </>
            )}
        </WhiteCard>
    );
}