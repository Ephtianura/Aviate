"use client";

import { useState } from "react";
import { IoChevronBack, IoChevronForward } from "react-icons/io5";

interface FlightData {
    id: string;
    basePrice: number;
    departureTime: string;

    departureAirport: {
        city: string;
        name: string;
        code: string;
    };

    arrivalAirport: {
        city: string;
        name: string;
        code: string;
    };
}

interface PriceChartProps {
    data: FlightData[];
    onSelect: (flight: FlightData) => void;
}

export default function PriceChart({ data, onSelect }: PriceChartProps) {

    const [startIndex, setStartIndex] = useState(0);
    const [hoveredIndex, setHoveredIndex] = useState<number | null>(null);
    const [selectedIndex, setSelectedIndex] = useState<number | null>(null);
    const daysPerView = 10;
    const totalColumns = 30;
    const MAX_HEIGHT = 200;

    const today = new Date();

    const chartData: { date: Date; flight: FlightData | null }[] = [];

    for (let i = 0; i < totalColumns; i++) {
        const date = new Date(today);
        date.setDate(today.getDate() + i);

        const flight =
            data.find(
                f =>
                    new Date(f.departureTime).toDateString() ===
                    date.toDateString()
            ) || null;

        chartData.push({ date, flight });
    }

    const viewData = chartData.slice(
        startIndex,
        startIndex + daysPerView
    );

    const maxPrice = Math.max(...data.map(d => d.basePrice), 1);
    const minPrice = Math.min(...data.map(d => d.basePrice));

    const handlePrev = () =>
        setStartIndex(Math.max(0, startIndex - daysPerView));

    const handleNext = () =>
        setStartIndex(
            Math.min(
                chartData.length - daysPerView,
                startIndex + daysPerView
            )
        );

    return (
        <div className="flex flex-col gap-4">
            {/* Навигация */}
            <div className="flex justify-between items-center mb-2">
                <button
                    onClick={handlePrev}
                    disabled={startIndex === 0}
                    className="p-2 bg-gray-200 rounded hover:bg-gray-300 disabled:opacity-50"
                >
                    <IoChevronBack />
                </button>

                <button
                    onClick={handleNext}
                    disabled={
                        startIndex + daysPerView >= chartData.length
                    }
                    className="p-2 bg-gray-200 rounded hover:bg-gray-300 disabled:opacity-50"
                >
                    <IoChevronForward />
                </button>
            </div>

            {/* График */}
            <div className="flex items-end gap-2 h-40 border-b border-gray-300 relative">
                {viewData.map((day, i) => {
                    const index = startIndex + i;
                    const hasFlight = !!day.flight;
                    const price = day.flight?.basePrice ?? 0;

                    const isHovered = hoveredIndex === index;
                    const isMin =
                        hasFlight && price === minPrice;

                    const barHeightPx = hasFlight
                        ? (price / maxPrice) * MAX_HEIGHT
                        : 20;

                    let bgColor = "bg-gray-100";

                    if (hasFlight) {
                        bgColor = isHovered
                            ? "bg-blue-500"
                            : isMin
                                ? "bg-green-500"
                                : "bg-gray-300";
                    }

                    return (
                        <div
                            key={index}
                            className="flex flex-1 flex-col items-center cursor-pointer group"
                            onClick={() => hasFlight && setSelectedIndex(index)}
                            onMouseEnter={() =>
                                setHoveredIndex(index)
                            }
                            onMouseLeave={() =>
                                setHoveredIndex(null)
                            }
                        >
                            {/* Цена */}
                            <div className="relative h-6 w-full flex justify-center">
                                <span
                                    className={`absolute bottom-0 text-[10px] font-bold text-blue-600 transition-opacity ${isHovered
                                            ? "opacity-100"
                                            : "opacity-0"
                                        }`}
                                >
                                    {hasFlight
                                        ? `${price}₴`
                                        : ""}
                                </span>
                            </div>

                            {/* Колонка */}
                            <div
                                className={`${bgColor} w-full max-w-[32px] rounded-t-sm transition-all duration-300`}
                                style={{
                                    height: `${barHeightPx}px`,
                                }}
                            />

                            {/* Дата */}
                            <div className="text-[10px] mt-1 text-gray-400">
                                {day.date.getDate()}
                            </div>
                        </div>
                    );
                })}
            </div>
            {selectedIndex !== null && chartData[selectedIndex].flight && (
                <button
                    onClick={() =>
                        onSelect(chartData[selectedIndex].flight!)
                    }
                    className="mt-2 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                >
                    Вибрати{" "}
                    {new Date(
                        chartData[selectedIndex].flight!.departureTime
                    ).toLocaleDateString("uk-UA", {
                        day: "numeric",
                        month: "long",
                    })}
                </button>
            )}
        </div>
    );
}