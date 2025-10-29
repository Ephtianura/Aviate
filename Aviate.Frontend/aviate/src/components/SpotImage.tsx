"use client";

import { useState } from "react";
import Image from "next/image";

export default function SpotImage({ image, title }: { image: string; title: string }) {
    const [open, setOpen] = useState(false);

    return (
        <>
            {/* обычная картинка */}
            <div
                className="  w-full h-full cursor-pointer"
                onClick={() => setOpen(true)}
            >
                <Image
                    src={image}
                    alt={title}
                    fill
                    className="object-cover opacity-90"
                />
            </div>

            {/* модалка */}
            {open && (
                <div
                    className="fixed inset-0 z-50 bg-black/80 flex items-center justify-center"
                    onClick={() => setOpen(false)}
                >
                    <div className=" w-[90vw] h-[90vh]">
                        <Image
                            src={image}
                            alt={title}
                            fill
                            className="object-contain"
                        />
                    </div>
                </div>
            )}
        </>
    );
}