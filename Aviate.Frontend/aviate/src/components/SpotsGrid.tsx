"use client";

import { useMemo } from "react";
import { getRandomSpots } from "./spots";
import { SpotCard } from "./Cards/SpotCard";
import HotFlights from "./HotFlights";

export default function SpotsGrid() {
    const spots = useMemo(() => getRandomSpots(5), []);

    return (
        <div className="container max-w-5xl grid mx-4 grid-cols-2 gap-x-6 gap-y-12 justify-center items-center">

            <div className="col-span-2 sm:col-span-1 z-10">
                <HotFlights />
            </div>

            {/* 1 */}
            <div className="flex flex-col items-center gap-2 col-span-2 sm:col-span-1">
                <h2 className="z-10 text-primary text-5xl font-bold">Куди полетіти?</h2>
                {spots[0] && <SpotCard {...spots[0]} />}
            </div>

            {/* 2 */}
            <div className="col-span-2 shrink-0">
                {spots[1] && <SpotCard {...spots[1]} />}
            </div>

            {/* 3 */}
            <div className="">
                {spots[2] && <SpotCard {...spots[2]} />}
            </div>

            {/* 4 */}
            <div className="">
                {spots[3] && <SpotCard {...spots[3]} />}
            </div>

            {/* 5 */}
            <div className="col-span-2">
                {spots[4] && <SpotCard {...spots[4]} />}
            </div>

        </div>
    );
}