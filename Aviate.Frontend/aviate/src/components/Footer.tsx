"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";


export default function Footer() {
    const router = useRouter();



    return (
        <footer className="bg-white shadow-[0_-4px_10px_-1px_rgba(0,0,0,0.05)] mt-6 z-50 text-primary-black text-sm">


            {/* 4 колонки */}
            <div className="grid grid-cols-3 mx-auto max-w-7xl px-4 py-2 gap-8 ">

                {/* 1 колонка */}
                <div className="flex flex-col gap-2 ">
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
                    {/* <div className="grid grid-cols-4 gap-2 mt-4">

                        {/* <Link href={"/"}>
                            FF
                        </Link>
                         <Link href={"/"}>
                            FF
                        </Link>
                         <Link href={"/"}>
                            FF
                        </Link>
                         <Link href={"/"}>
                            FF
                        </Link>
                        <Link href={"/"}>
                            FF
                        </Link>
                         <Link href={"/"}>
                            FF
                        </Link>
                         <Link href={"/"}>
                            FF
                        </Link>
                         <Link href={"/"}>
                            FF
                        </Link> 

                    </div> */}
                </div>
                <div>

                </div>

                {/* 2 колонка */}
                <div className="flex gap-2 items-center">
                    <Link href={"/"}>
                        Про Aviate
                    </Link>
                    <Link href={"/"}>
                        Вакансії
                    </Link>
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
                {/*
              
                <div>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit. Odit tempora consequuntur dolores quaerat aperiam
                    iusto earum harum qui explicabo praesentium sunt, asperiores ab nesciunt labore quibusdam voluptates cupiditate, illum sit.
                </div>
                
                <div>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit. Odit tempora consequuntur dolores quaerat aperiam
                    iusto earum harum qui explicabo praesentium sunt, asperiores ab nesciunt labore quibusdam voluptates cupiditate, illum sit.
                </div>
*/}
            </div>
        </footer>
    );
}

