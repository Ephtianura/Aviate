"use client";

import { useEffect, useState } from "react";
import WhiteCard from "../Cards/WhiteCard";
import { IoTimeOutline } from "react-icons/io5";
import Seat from "../Seat";
import { apiFetch } from "@/lib/api";

interface FlightBookingModalProps {
    flight: any;
    onClose: () => void;
    onSuccess: (msg: string) => void;
    onError: (msg: string) => void;
}

export default function FlightBookingModal({
    flight,
    onClose,
    onSuccess,
    onError
}: FlightBookingModalProps) {


    const [seats, setSeats] = useState<any[]>([]);
    const [selectedSeat, setSelectedSeat] = useState<any | null>(null);
    const [sortBy, setSortBy] = useState<"Class" | "IsBooked">("Class");
    const [sortDesc, setSortDesc] = useState(false);
    const [isBooking, setIsBooking] = useState(false);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);

    const dep = flight.departureAirport;
    const arr = flight.arrivalAirport;
    const departureTime = new Date(flight.departureTime);
    const arrivalTime = new Date(flight.arrivalTime);
    const diffMs = arrivalTime.getTime() - departureTime.getTime();
    const hours = Math.floor(diffMs / (1000 * 60 * 60));
    const minutes = Math.floor((diffMs / (1000 * 60)) % 60);

    useEffect(() => {
        const fetchSeats = async () => {
            try {
                const params = new URLSearchParams();
                params.append("FlightId", flight.id);
                params.append("PageSize", flight.airplane.capacity?.toString() || "20");
                params.append("Page", "1");

                const data = await apiFetch(`/seats?${params.toString()}`);
                setSeats(data.items || []);
            } catch (err) {
                console.error(err);
            }
        };
        fetchSeats();
    }, [flight.id, flight.totalSeats]);

    const handleSelectSeat = (seat: any) => {
        if (selectedSeat?.id === seat.id) {
            setSelectedSeat(null);
        } else if (!seat.isBooked) {
            setSelectedSeat(seat);
        }
    };

    const getPrice = () => {
        if (!selectedSeat) return flight.basePrice;
        if (selectedSeat.class === "Business") return flight.basePrice * 2.5;
        if (selectedSeat.class === "First") return flight.basePrice * 3;
        return flight.basePrice;
    };

    const sortedSeats = [...seats].sort((a, b) => {
        let res = 0;
        if (sortBy === "Class") res = a.class.localeCompare(b.class);
        if (sortBy === "IsBooked") res = (a.isBooked === b.isBooked ? 0 : a.isBooked ? 1 : -1);
        return sortDesc ? -res : res;
    });

    const bookSeat = async () => {
    setIsBooking(true);
    try {
        const body: any = {
            flightId: flight.id,
        };

        // seatId только если выбрано место
        if (selectedSeat !== null) {
            body.seatId = selectedSeat.id;
        }

        const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/bookings/create`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify(body),
        });

        if (!res.ok) {
            // пробуем распарсить JSON
            let errorMsg = "Помилка бронювання";
            try {
                const errJson = await res.json();
                if (errJson?.error) errorMsg = errJson.error;
            } catch { }

            throw new Error(errorMsg);
        }

        onSuccess("Бронювання успішно створене! Ви можете сплатити його у себе у профілі.");
        onClose();

    } catch (err: any) {
        onError(`Помилка при бронюванні місця: ${err.message}`);
    } finally {
        setIsBooking(false);
    }
};


    return (
        <>
            {/* Затемнение */}
            <div className="fixed inset-0 bg-black/20" onClick={onClose} />

            {/* Модалка */}
            <div className="fixed inset-0 z-50 flex justify-center items-center p-4">
                <WhiteCard>
                    <div className="flex gap-10 min-w-[1100px] max-h-[50vh] overflow-y-auto ">

                        {/* Левая часть - места */}
                        <div className="flex-1 flex flex-col gap-4">
                            <div className="flex justify-between items-center mb-2">
                                <h2 className="text-xl font-bold">За бажанням, ви можете обрати місце:</h2>
                                <div className="flex gap-2">
                                    <select
                                        value={sortBy}
                                        onChange={(e) => setSortBy(e.target.value as any)}
                                        className="border px-2 py-1 rounded"
                                    >
                                        <option value="Class">Клас</option>
                                        <option value="IsBooked">Заброньовано</option>
                                    </select>
                                    <button
                                        onClick={() => setSortDesc(!sortDesc)}
                                        className="border px-2 py-1 rounded"
                                    >
                                        {sortDesc ? "▼" : "▲"}
                                    </button>
                                </div>
                            </div>

                            <div className="grid grid-cols-6 overflow-y-auto gap-y-8 ">
                                {sortedSeats.map((seat) => (
                                    <Seat
                                        key={seat.id}
                                        seat={seat}
                                        selected={selectedSeat?.id === seat.id}
                                        onSelect={handleSelectSeat}
                                    />

                                ))}
                            </div>

                        </div>

                        {/* Правая часть - информация о рейсе */}
                        <div className="w-120 flex-shrink-0 ">
                            <div className="flex justify-between items-center mb-2 ">
                                <h2 className="text-2xl font-bold text-primary-black mx-auto ">
                                    Інформація про рейс
                                </h2>
                                <button
                                    onClick={onClose}
                                    className="text-gray-500 hover:text-gray-700 text-xl cursor-pointer"
                                >
                                    ✕
                                </button>
                            </div>

                            <div className="flex flex-col gap-3 mt-4">
                                <div className="text-lg font-bold">
                                    {dep.city} → {arr.city}
                                </div>

                                <div className="flex flex-col gap-1">
                                    <div className="text-sm text-gray-700">
                                        <b>Аеропорт вильоту:</b> {dep.name}
                                    </div>
                                    <div className="text-sm text-gray-700">
                                        <b>Аеропорт прильоту:</b> {arr.name}
                                    </div>

                                    <div className="flex flex-col py-2">
                                        <div className="text-sm text-gray-700">
                                            <b>Літак:</b> {flight.airplane.model}
                                        </div>
                                        <div className="text-gray-700 text-sm">
                                            Кількість місць: {flight.airplane.capacity}
                                        </div>
                                    </div>

                                </div>

                                <div className="grid grid-cols-2 gap-2 text-sm font-bold text-primary-black">
                                    <div>
                                        Виліт:
                                        <div className="text-gray-text">{departureTime.toLocaleString("uk-UA")}</div>
                                    </div>
                                    <div>
                                        Приліт:
                                        <div className="text-gray-text">{arrivalTime.toLocaleString("uk-UA")}</div>
                                    </div>
                                </div>
                                <div className="text-sm text-gray-900 font-medium flex gap-1 items-center">
                                    <IoTimeOutline className="w-5 h-5" />
                                    Час у дорозі: {hours} год {minutes} хв
                                </div>
                                <div>
                                    <b>Номер рейсу:</b> {flight.flightNumber}
                                </div>
                                <div className="text-lg font-bold text-primary">{getPrice()} ₴</div>
                                <button
                                    onClick={bookSeat}
                                    disabled={isBooking}
                                    className="mt-4 bg-primary text-white px-4 py-2 
                                        rounded-xl hover:bg-primary-dark transition disabled:opacity-50"
                                >
                                    {isBooking ? "Бронюємо..." : "Забронювати"}
                                </button>
                            </div>
                        </div>
                    </div>
                </WhiteCard>
            </div>
        </>
    );
}
