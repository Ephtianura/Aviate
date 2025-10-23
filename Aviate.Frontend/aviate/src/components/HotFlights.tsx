// components/HotFlights.tsx
"use client";

import { useEffect, useState, useRef } from "react";
import { MdNavigateNext, MdNavigateBefore } from "react-icons/md";
import FlightCard from "./Cards/FlightCard";
import WhiteCard from "./Cards/WhiteCard";
import { apiFetch } from "@/lib/api"; // твоя функция fetch
import { BsFire } from "react-icons/bs";

export default function HotFlights() {
    const [flights, setFlights] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);

    const scrollRef = useRef<HTMLDivElement>(null);

    const scroll = (direction: "left" | "right") => {
        if (!scrollRef.current) return;
        const scrollAmount = 300;
        scrollRef.current.scrollBy({ left: direction === "left" ? -scrollAmount : scrollAmount, behavior: "smooth" });
    };

   useEffect(() => {
    const fetchFlights = async () => {
        setLoading(true);
        try {
            const params = new URLSearchParams();
            params.append("SortBy", "basePrice");
            params.append("SortDesc", "false"); // по возрастанию
            params.append("Page", "1");         // первая страница
            params.append("PageSize", "5");     // количество билетов на странице

            const data = await apiFetch(`/flights?${params.toString()}`);
            setFlights(data.items || []);
        } catch (e) {
            console.error(e);
        } finally {
            setLoading(false);
        }
    };

    fetchFlights();
}, []);


    if (loading) return <div className="text-white text-xl">Завантаження гарячих квитків...</div>;
    if (flights.length === 0) return <div className="text-white text-xl">Гарячих квитків не знайдено</div>;

    return (
        <div className="p-6 rounded-2xl shadow-md bg-[#FA742D]" >
            <div className="flex flex-col gap-6 ">
                <div className="grid grid-cols-2">
                    <div className="flex flex-col gap-2 ">
                        <h2 className="text-white text-4xl font-bold">
                            Гарячі квитки
                        </h2>
                        <p className="text-white  ">
                            Скоро розберуть!
                        </p>
                    </div>

                    <div className="my-auto mx-auto">
                        <BsFire className="w-25 h-25 text-white"/>
                    </div>
                </div>


                <div className="relative">
                    {/* Кнопки навигации */}
                    <button
                        onClick={() => scroll("left")}
                        className="absolute left-0 top-1/2 transform -translate-y-1/2 z-10 
                        bg-white p-2 rounded-full shadow-[0_0_15px_rgba(0,0,0,0.2)] border-2 border-gray-200"
                    >
                        <MdNavigateBefore className="w-5 h-5" />
                    </button>
                    <button
                        onClick={() => scroll("right")}
                        className="absolute right-0 top-1/2 transform -translate-y-1/2 z-10 
                        bg-white p-2 rounded-full shadow-[0_0_15px_rgba(0,0,0,0.2)] border-2 border-gray-200"
                    >
                        <MdNavigateNext className="w-5 h-5" />
                    </button>

                    {/* Список рейсов */}
                    <div
                        ref={scrollRef}
                        className="flex gap-30 overflow-x-auto px-10 py-2 flex-nowrap scrollbar-hide"
                    >
                        {flights.map(flight => (
                            <div key={flight.id} className="flex-shrink-0 w-[300px]">
                                <FlightCard flight={flight} bgColor="bg-white" />
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
}
