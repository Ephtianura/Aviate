"use client";

import Link from "next/link";
import { ImAirplane } from "react-icons/im";
import { IoStatsChartSharp } from "react-icons/io5";

interface FlightSidebarProps {
    from?: string;
    to?: string;
}

export default function FlightSidebar({ from, to }: FlightSidebarProps) {
    return (
        <div className="right-0 bg-white p-4 rounded-xl text-primary-black shadow-lg sticky">
            <div className="p-2 flex gap-2 items-center">
                <h1 className="font-bold text-xl">
                    {from && to ? `${from} — ${to}` : "Маршрут не знайдено"}
                </h1>
            </div>

            <div className="mb-6 flex flex-col gap-2">
                {/* Найдешевші */}
                <Link
                    href={"#cheapest"}
                    className="hover:bg-gray-very-light rounded-2xl px-2 py-[6px]"
                >
                    <div className="flex gap-3 items-center">
                        <div className="bg-green-500 rounded-full p-2">
                            <ImAirplane className="w-5 h-5 text-white" />
                        </div>
                        <p>Найдешевші авіаквитки</p>
                    </div>
                </Link>

                {/* Графік цін */}
                <Link
                    href={"#chart"}
                    className="hover:bg-gray-very-light rounded-2xl px-2 py-[6px]"
                >
                    <div className="flex gap-3 items-center">
                        <div
                            className="rounded-full p-2"
                            style={{ backgroundColor: "#25AFF8" }}
                        >
                            <IoStatsChartSharp className="w-5 h-5 text-white" />
                        </div>
                        <p>Графік цін</p>
                    </div>
                </Link>
            </div>
        </div>
    );
}
