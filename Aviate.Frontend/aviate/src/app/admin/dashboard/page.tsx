"use client";

import WhiteCard from "@/components/Cards/WhiteCard";
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
    BarChart,
    Bar,
    CartesianGrid,
} from "recharts";
import { useEffect, useState } from "react";
import { useAuth } from "@/context/AuthContext";
import { redirect } from "next/navigation";

type Flight = {
    id: string;
    departureAirport: { city: string; country: string };
    arrivalAirport: { city: string };
    departureTime: string;
    arrivalTime: string;
};

type Airplane = {
    id: string;
    model: string;
    capacity: number;
};

type Booking = {
    id: string;
    totalPrice: number;
    status: number;
    bookingDate: string;
};

type Seat = {
    id: string;
    flightId: string;
    isBooked: boolean;
};

export default function AdminDashboard() {
    const [flights, setFlights] = useState<Flight[]>([]);
    const [airplanes, setAirplanes] = useState<Airplane[]>([]);
    const [bookings, setBookings] = useState<Booking[]>([]);
    const [seats, setSeats] = useState<Seat[]>([]);
    const NO_ANIMATION = {
        isAnimationActive: false,
        animationDuration: 0,
        animationBegin: 0,
        dot: false,
        activeDot: false,
        key: "line-static"

    };
    const { userRole } = useAuth();
    if (userRole === "Employee")
        redirect("/admin/flights");

    const COLORS = ["#7C3AED", "#F87171", "#FBBF24", "#34D399", "#60A5FA", "#F472B6"];

    useEffect(() => {
        const fetchAll = async () => {
            try {
                const [flightsRes, airplanesRes, bookingsRes, seatsRes] = await Promise.all([
                    apiFetch("/flights?Page=1&PageSize=100"),
                    apiFetch("/admin/airplanes?Page=1&PageSize=100"),
                    apiFetch("/admin/bookings?Page=1&PageSize=100"),
                    apiFetch("/seats?Page=1&PageSize=100"),
                ]);

                setFlights(flightsRes.items ?? []);
                setAirplanes(airplanesRes.items ?? []);
                setBookings(bookingsRes.items ?? []);
                setSeats(seatsRes.items ?? []);
            } catch (err) {
                console.error("Помилка завантаження даних", err);
            }
        };

        fetchAll();
    }, []);

    const monthNames = ["Січ", "Лют", "Бер", "Квіт", "Трав", "Чер", "Лип", "Сер", "Вер", "Жов", "Лис", "Гру"];

    // --- 1. Рейси по місяцях ---
    const monthlyFlights = monthNames.map((month, idx) => ({
        month,
        count: flights.filter(f => new Date(f.departureTime).getMonth() === idx).length,
    }));

    // --- 2. Аеропорти ---
    const airportMap: Record<string, number> = {};
    flights.forEach(f => {
        const city = f.departureAirport.city;
        airportMap[city] = (airportMap[city] || 0) + 1;
    });
    const airportData = Object.entries(airportMap).map(([name, value]) => ({ name, value }));

    // --- 3. Середня тривалість ---
    const avgDuration = monthNames.map((month, idx) => {
        const arr = flights.filter(f => new Date(f.departureTime).getMonth() === idx);

        const avg =
            arr.length === 0
                ? 0
                : arr.reduce((acc, f) => {
                    const d =
                        new Date(f.arrivalTime).getTime() -
                        new Date(f.departureTime).getTime();
                    return acc + d;
                }, 0) / arr.length;

        return {
            month,
            hours: Math.round(avg / 1000 / 60 / 60),
        };
    });

    // --- 4. Топ маршрутів ---
    const routeMap: Record<string, number> = {};
    flights.forEach(f => {
        const route = `${f.departureAirport.city} → ${f.arrivalAirport.city}`;
        routeMap[route] = (routeMap[route] || 0) + 1;
    });

    const routeData = Object.entries(routeMap)
        .map(([route, count]) => ({ route, count }))
        .sort((a, b) => b.count - a.count)
        .slice(0, 10);

    // --- 5. Час доби ---
    const timeBuckets = { Ніч: 0, Ранок: 0, День: 0, Вечір: 0 };

    flights.forEach(f => {
        const h = new Date(f.departureTime).getHours();
        if (h < 6) timeBuckets.Ніч++;
        else if (h < 12) timeBuckets.Ранок++;
        else if (h < 18) timeBuckets.День++;
        else timeBuckets.Вечір++;
    });

    const timeData = Object.entries(timeBuckets).map(([name, value]) => ({ name, value }));

    // --- 6. Місткість літаків ---
    const modelMap: Record<string, number[]> = {};
    airplanes.forEach(a => {
        if (!modelMap[a.model]) modelMap[a.model] = [];
        modelMap[a.model].push(a.capacity);
    });

    const modelCapacityData = Object.entries(modelMap).map(([model, caps]) => ({
        model,
        avg: Math.round(caps.reduce((a, b) => a + b, 0) / caps.length),
    }));

    // --- 7. Дохід ---
    const revenueMap: Record<string, number> = {};
    bookings.forEach(b => {
        const day = new Date(b.bookingDate).toISOString().split("T")[0];
        revenueMap[day] = (revenueMap[day] || 0) + b.totalPrice;
    });

    const revenueData = Object.entries(revenueMap).map(([date, revenue]) => ({
        date,
        revenue,
    }));

    // --- 8. Заповненість ---
    const loadData = flights.slice(0, 20).map(f => {
        const s = seats.filter(x => x.flightId === f.id);
        const booked = s.filter(x => x.isBooked).length;
        const total = s.length;

        return {
            name: f.departureAirport.city,
            load: total === 0 ? 0 : Math.round((booked / total) * 100),
        };
    });

    // --- 9. Країни ---
    const countryMap: Record<string, number> = {};
    flights.forEach(f => {
        const c = f.departureAirport.country;
        countryMap[c] = (countryMap[c] || 0) + 1;
    });

    const countryData = Object.entries(countryMap).map(([name, value]) => ({ name, value }));

    // --- 10. Статуси бронювань ---
    const statusMap: Record<number, number> = {};
    bookings.forEach(b => {
        statusMap[b.status] = (statusMap[b.status] || 0) + 1;
    });

    const statusData = Object.entries(statusMap).map(([status, count]) => ({
        name: `Статус ${status}`,
        value: count,
    }));

    return (
        <>

            <h1 className="text-4xl font-extrabold mb-8 text-primary ">
                Панель адміністратора
            </h1>

            <div className="grid lg:grid-cols-2 gap-10 select-none min-w-0 w-full">

                {/* 1 */}
                <WhiteCard>
                    <h2 className="mb-4">Рейси по місяцях</h2>
                    <ResponsiveContainer width="100%" height={300}>
                        <LineChart {...NO_ANIMATION}
                            data={monthlyFlights}>
                            <XAxis dataKey="month" />
                            <YAxis />
                            <Tooltip />
                            <Line dataKey="count" stroke="var(--color-primary)" />
                        </LineChart>
                    </ResponsiveContainer>
                </WhiteCard>

                {/* 2 */}
                <WhiteCard>
                    <h2 className="mb-4">Аеропорти вильоту</h2>
                    <ResponsiveContainer width="100%" height={300}>
                        <PieChart>
                            <Pie {...NO_ANIMATION} data={airportData} dataKey="value" nameKey="name">
                                {airportData.map((_, i) => (
                                    <Cell key={i} fill={COLORS[i % COLORS.length]} />
                                ))}
                            </Pie>
                            <Tooltip />
                        </PieChart>
                    </ResponsiveContainer>
                </WhiteCard>

                {/* 3 */}
                <WhiteCard>
                    <h2 className="mb-4">Середня тривалість (год)</h2>
                    <LineChart width={400} height={300} {...NO_ANIMATION} data={avgDuration}>
                        <XAxis dataKey="month" />
                        <YAxis />
                        <Tooltip />
                        <Line dataKey="hours" stroke="#34D399" />
                    </LineChart>
                </WhiteCard>

                {/* 4 */}
                <WhiteCard>
                    <h2 className="mb-4">Топ маршрутів</h2>
                    <BarChart width={400} height={300} {...NO_ANIMATION} data={routeData}>
                        <XAxis dataKey="route" hide />
                        <YAxis />
                        <Tooltip />
                        <Bar dataKey="count" fill="#60A5FA" />
                    </BarChart>
                </WhiteCard>

                {/* 5 */}
                <WhiteCard>
                    <h2 className="mb-4">Час доби</h2>
                    <PieChart width={400} height={300}>
                        <Pie {...NO_ANIMATION} data={timeData} dataKey="value">
                            {timeData.map((_, i) => (
                                <Cell key={i} fill={COLORS[i % COLORS.length]} />
                            ))}
                        </Pie>
                        <Tooltip />
                    </PieChart>
                </WhiteCard>

                {/* 6 */}
                <WhiteCard>
                    <h2 className="mb-4">Місткість літаків</h2>
                    <BarChart width={400} height={300} {...NO_ANIMATION} data={modelCapacityData}>
                        <XAxis dataKey="model" hide />
                        <YAxis />
                        <Tooltip />
                        <Bar dataKey="avg" fill="#FBBF24" />
                    </BarChart>
                </WhiteCard>

                {/* 7 */}
                {/* <WhiteCard>
                    <h2 className="mb-4">Дохід по днях</h2>
                    <LineChart width={400} height={300}  {...NO_ANIMATION} data={revenueData}>
                        <XAxis dataKey="date" hide />
                        <YAxis />
                        <Tooltip />
                        <Line dataKey="revenue" stroke="#F87171" />
                    </LineChart>
                </WhiteCard> */}

                {/* 8 */}
                {/* <WhiteCard>
                    <h2 className="mb-4">Заповненість рейсів (%)</h2>
                    <BarChart width={400} height={300}  {...NO_ANIMATION} data={loadData}>
                        <XAxis dataKey="name" hide />
                        <YAxis />
                        <Tooltip />
                        <Bar dataKey="load" fill="#7C3AED" />
                    </BarChart>
                </WhiteCard> */}

                {/* 9 */}
                {/* <WhiteCard>
                    <h2 className="mb-4">Країни</h2>
                    <PieChart width={400} height={300}>
                        <Pie  {...NO_ANIMATION} data={countryData} dataKey="value">
                            {countryData.map((_, i) => (
                                <Cell key={i} fill={COLORS[i % COLORS.length]} />
                            ))}
                        </Pie>
                        <Tooltip />
                    </PieChart>
                </WhiteCard> */}

                {/* 10 */}
                {/* <WhiteCard>
                    <h2 className="mb-4">Статуси бронювань</h2>
                    <PieChart width={400} height={300}>
                        <Pie  {...NO_ANIMATION} data={statusData} dataKey="value">
                            {statusData.map((_, i) => (
                                <Cell key={i} fill={COLORS[i % COLORS.length]} />
                            ))}
                        </Pie>
                        <Tooltip />
                    </PieChart>
                </WhiteCard> */}

            </div>
        </>
    );
}