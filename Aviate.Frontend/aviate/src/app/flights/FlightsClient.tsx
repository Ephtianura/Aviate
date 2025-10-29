// app/flights/page.tsx
"use client";

import { useSearchParams } from "next/navigation";
import { useEffect, useState, useRef } from "react";
import { apiFetch } from "@/lib/api";
import FlightCard from "@/components/Cards/FlightCard";
import WhiteCard from "@/components/Cards/WhiteCard";
import FlightSidebar from "@/components/Bars/FlightSidebar";
import PriceHistogram from "@/components/PriceHistogram";
import { MdNavigateNext, MdNavigateBefore } from "react-icons/md";
import SearchCard from "@/components/SearchCard";
import FlightBookingModal from "@/components/Modals/FlightBookingModal";
import { motion, AnimatePresence } from "framer-motion";
import { GiConfirmed } from "react-icons/gi";
import { MdOutlineErrorOutline } from "react-icons/md";
import { useAuth } from "@/context/AuthContext";
import { useToast } from "@/components/ToastProvider";

export default function FlightsClient() {
    const searchParams = useSearchParams();
    const { isLoggedIn } = useAuth();
    const { success, error } = useToast();

    const [flights, setFlights] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    const [selectedFlight, setSelectedFlight] = useState<any | null>(null); // новое состояние для модалки

    const handleSelectFlight = (flight: any) => {
    if (!isLoggedIn) {
        error('Увійдіть для бронювання');
        return;
    }

    setSelectedFlight(flight);
};

    const departureId = searchParams.get("DepartureAirportId");
    const arrivalId = searchParams.get("ArrivalAirportId");

    const scrollRef = useRef<HTMLDivElement>(null);

    const scroll = (direction: "left" | "right") => {
        if (!scrollRef.current) return;
        const scrollAmount = 500;
        scrollRef.current.scrollBy({ left: direction === "left" ? -scrollAmount : scrollAmount, behavior: "smooth" });
    };

    type ToastType = "success" | "error";

    const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);


    const showToast = (message: string, type: ToastType = "success") => {
        setToast({ message, type });

        const timer = setTimeout(() => setToast(null), 10000);

        return () => clearTimeout(timer);
    };

    useEffect(() => {
        // если нет ни одного параметра — смысла делать запрос нет
        if (!departureId && !arrivalId) return;

        const fetchFlights = async () => {
            setLoading(true);
            try {
                const params = new URLSearchParams();

                if (departureId) params.append("DepartureAirportId", departureId);
                if (arrivalId) params.append("ArrivalAirportId", arrivalId);

                const data = await apiFetch(`/flights?${params.toString()}`);
                setFlights(data.items || []);
            } catch (e) {
                console.error(e);
            } finally {
                setLoading(false);
            }
        };

        fetchFlights();
    }, [departureId, arrivalId]);

   

    if (loading) return <div>Загрузка рейсів...</div>;

    const firstFlight = flights[0];
    const fromCity = firstFlight?.departureAirport?.city;
    const toCity = firstFlight?.arrivalAirport?.city;

    return (
        <main className="">
            <div className="sticky top-16 z-10 bg-blue-500/90">
                <SearchCard />
            </div>
            <div className="flex justify-center ">
                <div className="w-full max-w-7xl p-10 mx-4 flex flex-col lg:flex-row gap-10 items-start ">

                    <FlightSidebar from={fromCity} to={toCity} />

                    <div className="flex flex-col gap-14 w-full min-w-0">

                        {/* НАЙДЕШЕВШІ */}
                        <WhiteCard>
                            <div className="flex flex-col gap-6 w-full">
                                <h2 className="text-primary-black text-2xl font-bold">
                                    Найдешевші авіаквитки
                                </h2>

                                <div className="relative">
                                    {/* Кнопки навигации */}
                                    <button
                                        onClick={() => scroll("left")}
                                        className="absolute left-0 top-1/2 transform -translate-y-1/2 z-10 
                                        bg-white p-2 rounded-full shadow-[0_0_15px_rgba(0,0,0,0.2)]"
                                    >
                                        <MdNavigateBefore className="w-5 h-5" />
                                    </button>
                                    <button
                                        onClick={() => scroll("right")}
                                        className=" absolute right-0 top-1/2 transform -translate-y-1/2 z-10 
                                        bg-white p-2 rounded-full shadow-[0_0_15px_rgba(0,0,0,0.2)]"
                                    >
                                        <MdNavigateNext className="w-5 h-5" />
                                    </button>

                                    {/* Список рейсов */}
                                    <div
                                        ref={scrollRef}
                                        className="flex gap-30 overflow-x-auto px-10 py-2 scrollbar-hide flex-nowrap"
                                    >
                                        {flights.length === 0 ? (
                                            <div className="text-gray-700 text-center w-full py-10">
                                                На жаль, рейсів з такими параметрами немає
                                            </div>
                                        ) : (
                                            flights.map(flight => (
                                                <div
                                                    key={flight.id}
                                                    className="shrink-0 cursor-pointer"
                                                    onClick={() => handleSelectFlight(flight)} // клик открывает модалку
                                                >
                                                    <FlightCard flight={flight} />
                                                </div>
                                            ))
                                        )}
                                    </div>
                                </div>
                            </div>
                        </WhiteCard>

                        {/* ГРАФІК ЦІН */}
                        <WhiteCard>
                            <div className="flex flex-col gap-6" id="chart">
                                <h2 className="text-primary-black text-2xl font-bold">
                                    Графік цін
                                </h2>
                                <div>
                                    <PriceHistogram
                                        data={flights}
                                        onSelect={(flight) => handleSelectFlight(flight)}
                                    />
                                </div>
                            </div>
                        </WhiteCard>
                    </div>
                </div>
            </div>

            {/* Модалка */}
            {selectedFlight && (
                <FlightBookingModal
                    flight={selectedFlight}
                    onClose={() => handleSelectFlight(null)}
                    onSuccess={(msg) => showToast(msg, "success")}
                    onError={(msg) => showToast(msg, "error")}
                />

            )}


            <AnimatePresence>
                {toast && (
                    <motion.div
                        key={toast.message}
                        initial={{ opacity: 0, y: -20 }}
                        animate={{ opacity: 1, y: 0 }}
                        exit={{ opacity: 0, y: -20 }}
                        transition={{ duration: 0.3 }}
                        className={`fixed top-5 right-5 px-6 py-4 rounded-lg flex items-center gap-3 shadow-lg z-[9999]
              ${toast.type === "success"
                                ? "bg-green-100 border border-green-400 text-green-800"
                                : "bg-red-100 border border-red-400 text-red-800"
                            }`}
                    >
                        {toast.type === "success" ? (
                            <GiConfirmed className="w-6 h-6" />
                        ) : (
                            <MdOutlineErrorOutline className="w-6 h-6" />
                        )}
                        <span>{toast.message}</span>
                        <button onClick={() => setToast(null)}>✕</button>
                    </motion.div>
                )}
            </AnimatePresence>
        </main>
    );
}
