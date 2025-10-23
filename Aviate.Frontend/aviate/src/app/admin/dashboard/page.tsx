"use client";

import { AdminLayout } from "@/components/Layouts/AdminLayout";
import GradientCard from "@/components/Cards/WhiteCard";
import { apiFetch } from "@/lib/api";
import {
    LineChart,
    Line,
    PieChart,
    Pie,
    Cell,
    Tooltip,
    ResponsiveContainer,
    XAxis,
    YAxis,
    Legend,
} from "recharts";
import { useEffect, useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";

type Flight = {
    id: string;
    departureAirport: { city: string };
    arrivalAirport: { city: string };
    departureTime: string;
    arrivalTime: string;
};

export default function AdminDashboard() {
    const [flights, setFlights] = useState<Flight[]>([]);
    const COLORS = ["#7C3AED", "#F87171", "#FBBF24", "#34D399", "#60A5FA", "#F472B6"];

    useEffect(() => {
        const fetchFlights = async () => {
            try {
                const res = await apiFetch("/flights");
                setFlights(res.items ?? []);
            } catch (err) {
                console.error("Error fetching flights", err);
            }
        };
        fetchFlights();
    }, []);

    // --- График 1: количество полетов по месяцам ---
    const monthNames = ["Січ", "Лют", "Бер", "Квіт", "Трав", "Чер", "Лип", "Сер", "Вер", "Жов", "Лис", "Гру"];
    const monthlyFlights = monthNames.map((month, idx) => ({
        month,
        count: flights.filter(f => new Date(f.departureTime).getMonth() === idx).length,
    }));

    // --- График 2: распределение по аэропортам вылета ---
    const airportMap: Record<string, number> = {};
    flights.forEach(f => {
        const city = f.departureAirport.city;
        airportMap[city] = (airportMap[city] || 0) + 1;
    });
    const airportData = Object.entries(airportMap).map(([city, value]) => ({ name: city, value }));

    return (
        <AdminLayout>
            <h1 className="text-4xl font-extrabold mb-8 text-primary drop-shadow-sm">
                Панель адміністратора
            </h1>

            {/* Charts */}
            <div className="grid lg:grid-cols-2 gap-10">
                {/* График 1: Линейный */}
                <WhiteCard>
                    <h2 className="text-xl font-semibold mb-4 text-primary">
                        Кількість рейсів по місяцях
                    </h2>
                    <ResponsiveContainer width="100%" height={300}>
                        <LineChart data={monthlyFlights}>
                            <XAxis dataKey="month" stroke="var(--color-primary-light)" />
                            <YAxis stroke="var(--color-primary-light)" />
                            <Tooltip />
                            <Line
                                type="monotone"
                                dataKey="count"
                                stroke="var(--color-primary)"
                                strokeWidth={3}
                                dot={{ r: 5, fill: "var(--color-primary-hover)" }}
                                isAnimationActive={true}
                            />
                        </LineChart>
                    </ResponsiveContainer>
                </WhiteCard>

                {/* График 2: Pie */}
                <WhiteCard >
                    <h2 className="text-xl font-semibold mb-4 text-primary">
                        Розподіл рейсів по аеропортах вильоту
                    </h2>
                    <ResponsiveContainer width="100%" height={300}>
                        <PieChart>
                            <Pie
                                data={airportData}
                                dataKey="value"
                                nameKey="name"
                                innerRadius={60}
                                outerRadius={100}
                                paddingAngle={5}
                                isAnimationActive={true}
                            >
                                {airportData.map((_, i) => (
                                    <Cell key={i} fill={COLORS[i % COLORS.length]} />
                                ))}
                            </Pie>
                            <Tooltip />
                            <Legend verticalAlign="bottom" height={36} />
                        </PieChart>
                    </ResponsiveContainer>
                </WhiteCard>
            </div>
        </AdminLayout>
    );
}
