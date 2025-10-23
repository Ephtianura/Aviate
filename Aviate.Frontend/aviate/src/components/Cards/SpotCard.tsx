"use client";

import Image from "next/image";
import React from "react";
import { IoAirplane } from "react-icons/io5";

interface SpotCardProps {
    city: string;
    title: string;
    description: string;
    image: string; // путь до картинки из public
}

export const SpotCard: React.FC<SpotCardProps> = ({
    city,
    title,
    description,
    image,
}) => {
    return (
        <div className="relative w-full rounded-2xl overflow-hidden shadow-lg bg-black h-[320px] flex items-end">
            {/* Background image */}
            <Image
                src={image}
                alt={title}
                fill
                className="object-cover opacity-80"
            />

            {/* City tag */}
            <div className="absolute top-4 left-4 bg-white/90 text-black px-4 py-1 rounded-full text-sm font-semibold shadow text-primary-black">
                <div className="flex items-center gap-1">
                    <IoAirplane className="h-4 w-4 "/>
                    {city}
                </div>


            </div>

            {/* Content */}
            <div className="relative z-10 p-5 text-white">
                <h1 className="text-3xl font-bold mb-1">{title}</h1>
                <p className="text-sm leading-snug line-clamp-4">
                    {description}
                </p>
            </div>
        </div>
    );
};
