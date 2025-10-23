// components/Seat.tsx
"use client";
import { ImCross } from "react-icons/im";

interface SeatProps {
    seat: any; // { seatNumber: string, class: string, isBooked: boolean }
    selected: boolean;
    onSelect: (seat: any) => void;
}

export default function Seat({ seat, selected, onSelect }: SeatProps) {
    const handleClick = () => {
        if (!seat.isBooked) onSelect(seat);
    };

    // Цвет по классу
    let bgColor = "";
    let textColor = "text-white";
    let borderColor = "";


    if (seat.class === "Economy") {
        bgColor = "bg-blue-400";
        borderColor = "border-blue-600";
    } else if (seat.class === "Business") {
        bgColor = "bg-orange-400";
        borderColor = "border-orange-600";
    } else if (seat.class === "First") {
        bgColor = "bg-purple-400";
        borderColor = "border-purple-600";
    }

    // Если выбрано
    if (selected) {
        bgColor = "bg-green-500";
        borderColor = "border-green-700";
    }

    return (
        <div
            className={`w-14 h-14 flex items-center justify-center rounded-md border cursor-pointer relative border-3  ${borderColor} ${bgColor} ${textColor}`}
            onClick={handleClick}
            title={`${seat.class} — ${seat.seatNumber} ${seat.isBooked ? "(заброньовано)" : ""}`}
        >
            {seat.seatNumber}
            {seat.isBooked && (
                <span className="absolute inset-0 flex items-center justify-center text-red-700 font-bold text-3xl ">
                    <ImCross />
                </span>
            )}
        </div>
    );
}
