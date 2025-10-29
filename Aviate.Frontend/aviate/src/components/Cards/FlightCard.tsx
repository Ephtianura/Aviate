// components/FlightCard.tsx
"use client";

import { IoTimeOutline } from "react-icons/io5";

interface FlightCardProps {
    flight: {
        id: string;
        basePrice: number;
        flightNumber: string;
        departureTime: string;
        arrivalTime: string;
        departureAirport: { city: string };
        arrivalAirport: { city: string };
    };
    bgColor?: string; // новый проп для цвета фона
    onClick?: () => void;
}

export default function FlightCard({ flight, bgColor = "bg-gray-100", onClick }: FlightCardProps) {
    const dep = new Date(flight.departureTime);
    const arr = new Date(flight.arrivalTime);
    const diffMs = arr.getTime() - dep.getTime();
    const hours = Math.floor(diffMs / 1000 / 60 / 60);
    const minutes = Math.floor((diffMs / 1000 / 60) % 60);

    return (
        <div
            className={`p-6 rounded-2xl shadow-md w-100 cursor-pointer w-full ${bgColor}`}
            key={flight.id} onClick={onClick}
        >
            <div className="flex flex-col gap-2">
                {/* Цена */}
                <h2 className="text-primary-black text-2xl font-bold">
                    {flight.basePrice} ₴
                </h2>

                {/* Основная информация */}
                <div className="flex justify-between items-center">
                    <div>
                        <p className="text-lg font-semibold text-primary">
                            {flight.departureAirport.city} → {flight.arrivalAirport.city}
                        </p>
                        <p className="text-sm text-gray-600">
                            Номер рейсу: {flight.flightNumber}
                        </p>
                        <p className="text-sm text-gray-600">
                            Час: {dep.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })} →
                            {arr.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}
                        </p>
                    </div>

                    {/* Время в пути */}
                    <div className="text-sm text-gray-900 font-medium text-right flex gap-1 items-center">
                        <IoTimeOutline className="w-5 h-5" />
                        {hours} ч {minutes} хв
                    </div>
                </div>
            </div>
        </div>
    );
}
