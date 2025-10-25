"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";


export default function Footer() {
    const router = useRouter();



    return (
        <footer className="bg-white shadow-[0_-4px_10px_-1px_rgba(0,0,0,0.05)] mt-6 z-50 text-primary-black text-sm">


            {/* 4 колонки */}
            <div className="flex sm:justify-between mx-auto max-w-7xl px-4 py-2 gap-8 ">

                {/* 1 колонка */}
                <div className="flex items-center gap-3">
                    <img
                        src="/favicon.ico"
                        alt="Профіль"
                        className="w-10 h-10"
                    />
                    <div className="flex flex-col">
                        <p>
                            Aviate
                        </p>
                        <p>
                            &copy; {new Date().getFullYear()}
                        </p>
                    </div>
                </div>


                {/* 2 колонка */}
                <div className="flex flex-col sm:flex-row gap-2 items-center">

                    <div className="flex gap-2 items-center">
                        <Link href={"/"}>
                            Про Aviate
                        </Link>
                        <Link href={"/"}>
                            Вакансії
                        </Link>
                    </div>

                    <div className="flex gap-2 items-center">
                        <Link href={"/"}>
                            Підтримка
                        </Link>
                        <Link href={"/"}>
                            Реклама
                        </Link>
                        <Link href={"/"}>
                            Пресс-центр
                        </Link>
                    </div>
                </div>
            </div>
        </footer>
    );
}

