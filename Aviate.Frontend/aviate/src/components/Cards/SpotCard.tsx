"use client";

import Image from "next/image";
import React from "react";
import { IoAirplane } from "react-icons/io5";
import SpotImage from "../SpotImage";

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
        <div className="flex relative w-full rounded-2xl overflow-hidden shadow-lg bg-black h-80 items-end">
            {/* Background image */}
            <div className="">
                <SpotImage image={image} title={title} />
            </div>
            {/*              
            <Image
                src={image}
                alt={title}
                fill
                className="object-cover opacity-90"
            /> */}

            {/* City tag */}
            <div className="absolute top-4 left-4 bg-white/90 text-black px-4 py-1 rounded-full text-sm font-semibold shadow text-primary-black">
                <div className="flex items-center gap-1">
                    <IoAirplane className="h-4 w-4 " />
                    {city}
                </div>
            </div>

            {/* Content */}
            <div className=" z-10 p-5 text-white text-left">
                <h1 className="text-3xl font-bold mb-1 inline-block bg-black/70 rounded-lg px-2 py-1">
                    {title}
                </h1>
                <p className="bg-black/70 rounded-lg px-1  text-sm leading-snug line-clamp-4">
                    {description}
                </p>
            </div>
        </div>
    );
};
