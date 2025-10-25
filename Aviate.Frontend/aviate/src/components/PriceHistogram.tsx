"use client";

import { useState } from "react";
import { IoChevronBack, IoChevronForward } from "react-icons/io5";

interface FlightData {
    date: string;
    price: number;
    id: string;
}

interface PriceChartProps {
    data: FlightData[];
}

export default function PriceChart({ data }: PriceChartProps) {
    const [startIndex, setStartIndex] = useState(0);
    const [selectedIndex, setSelectedIndex] = useState<number | null>(null);
    const [hoveredIndex, setHoveredIndex] = useState<number | null>(null);

    const daysPerView = 10;
    const totalColumns = 30; // всегда минимум 30 колонок
    const MAX_HEIGHT = 200;

    const today = new Date();

    // создаём массив дат для всего графика
    const chartData: { date: Date; flight: FlightData | null }[] = [];
    for (let i = 0; i < totalColumns; i++) {
        const date = new Date(today);
        date.setDate(today.getDate() + i);

        const flight = data.find(f => new Date(f.date).toDateString() === date.toDateString()) || null;
        chartData.push({ date, flight });
    }

    const viewData = chartData.slice(startIndex, startIndex + daysPerView);

    const minPrice = Math.min(...data.map(d => d.price));

    const handlePrev = () => setStartIndex(Math.max(0, startIndex - daysPerView));
    const handleNext = () => setStartIndex(Math.min(chartData.length - daysPerView, startIndex + daysPerView));
    const handleSelect = (index: number) => setSelectedIndex(index);

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
                    disabled={startIndex + daysPerView >= chartData.length}
                    className="p-2 bg-gray-200 rounded hover:bg-gray-300 disabled:opacity-50"
                >
                    <IoChevronForward />
                </button>
            </div>

            {/* Гистограмма */}
            <div className="flex items-end gap-2 h-40 border-b border-gray-300 relative">
                {viewData.map((day, i) => {
                    const index = startIndex + i;
                    const isSelected = selectedIndex === index;
                    const isHovered = hoveredIndex === index;
                    const hasFlight = day.flight !== null;

                    const price = hasFlight ? day.flight!.price : 0;

                    const maxPrice = Math.max(...data.map(d => d.price), 1);

                    const barHeightPx = hasFlight
                        ? (price / maxPrice) * MAX_HEIGHT
                        : 20;

                    // Цвет колонки
                    const isMin = hasFlight && price === minPrice;
                    let bgColor: string;
                    if (hasFlight) {
                        bgColor = isSelected || isHovered
                            ? "bg-blue-500"
                            : isMin
                                ? "bg-green-500"
                                : "bg-gray-300";
                    } else {
                        bgColor = isHovered ? "bg-gray-500" : "bg-gray-100";
                    }

                    return (
                        <div
                            key={index}
                            className="flex flex-col items-center cursor-pointer"
                            onClick={() => hasFlight && handleSelect(index)}
                            onMouseEnter={() => setHoveredIndex(index)}
                            onMouseLeave={() => setHoveredIndex(null)}
                        >
                            {/* Цена сверху */}
                            <div
                                className={`text-sm font-bold text-blue-500 mb-1 transition-opacity duration-200 ${isSelected || isHovered ? "opacity-100" : "opacity-0"
                                    }`}
                            >
                                {hasFlight ? `${price} ₴` : "Невідома ціна"}
                            </div>

                            {/* Колонка */}
                            <div
                                className={`${bgColor} w-full max-w-10 rounded-t-md transition-all duration-300`}
                                style={{ height: `${barHeightPx}px` }}
                            ></div>

                            {/* Дата снизу */}
                            <div className="text-xs mt-1 text-gray-600">
                                {day.date.getDate()}
                            </div>
                        </div>
                    );
                })}
            </div>

            {/* Кнопка выбрать дату */}
            {selectedIndex !== null && chartData[selectedIndex].flight && (
                <button className="mt-2 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">
                    Вибрати{" "}
                    {chartData[selectedIndex].flight &&
                        new Date(chartData[selectedIndex].flight!.date).toLocaleDateString("uk-UA", {
                            day: "numeric",
                            month: "long",
                        })}
                </button>
            )}
        </div>
    );
}
