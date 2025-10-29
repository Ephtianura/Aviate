"use client";

import Link from "next/link";
import { useRouter, usePathname } from "next/navigation";
import { useAuth } from "@/context/AuthContext";
import { logout } from "@/lib/api";
import { useState, useEffect, useRef } from "react";
import { FiUser } from "react-icons/fi";
import { RiQuestionnaireFill } from "react-icons/ri";
import { ProfileModal } from "./Modals/ProfileModal";

export default function Navbar() {
  const pathname = usePathname();
  const router = useRouter();
  const { isLoggedIn, setIsLoggedIn, userRole, userName, setUserRole } = useAuth();
  const [mounted, setMounted] = useState(false);
  const [activeIndex, setActiveIndex] = useState(0);
  const buttonRef = useRef<HTMLDivElement>(null);
  const [position, setPosition] = useState({ top: 0, left: 0 });


  const [isProfileModalOpen, setProfileModalOpen] = useState(false);


  useEffect(() => {
    // помечаем, что компонент смонтирован
    setMounted(true);

    // вычисляем позицию модалки при открытии
    if (buttonRef.current && isProfileModalOpen) {
      const rect = buttonRef.current.getBoundingClientRect();
      setPosition({
        top: rect.bottom + window.scrollY + 4,
        left: rect.left + window.scrollX,
      });
    }
  }, [isProfileModalOpen]);


  const handleLogout = async () => {
    try {
      await logout();
    } catch (err: any) {
      if (err.status !== 401) {
        console.error("Logout error:", err);
      }
    } finally {
      setIsLoggedIn(false);
      setUserRole(null);
      window.location.href = "/";
    }
  };

  const navItems = [
    { label: "Авіаквитки", href: "/" },
    { label: "Мої бронювання", href: "/bookings" },
    { label: "Адмін панель", href: "/admin" },
  ];


  if (!mounted) return <nav className="bg-primary shadow-md sticky top-0 z-50 h-16"></nav>;

  return (
    <nav className="bg-primary shadow-md sticky top-0 z-50">

      {/* 3 колонки */}
      <div className="flex justify-between  max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 items-center h-16 ">
        {/* Колонка 1 - Ім'я сайту */}
        <div className="">
          <div>
            <Link href="/" className="font-bold text-xl text-white transition-colors flex items-center gap-2">
              <img
                src="/favicon.ico"
                alt="Aviate Logo"
                className="w-5 h-5"
              />
              <span className="hidden sm:block">Aviate</span>
            </Link>
          </div>
        </div>
              
        {/* Колонка 2 - Панель навігації */}
        <div className="flex justify-center gap-4 ">

          <div className="flex gap-2 bg-btn-no-active rounded-[10px] h-10 p-1 text-primary-light text-md font-bold ">

            <Link href="/"
              className="hidden md:flex items-center hover:bg-btn-hover rounded-[10px] 
                  px-2 transition-colors duration-300 active:bg-white active:text-primary-black 
                  active:duration-10">
              Авіаквитки
            </Link>

            <Link href="/my/bookings" className="hidden sm:flex items-center hover:bg-btn-hover rounded-[10px] px-2 transition-colors duration-300 active:bg-white active:text-primary-black active:duration-10">
              <p>Мої бронювання</p>
            </Link>

            {(userRole === "Admin" || userRole === "Employee") && (
              <Link
                href="/admin/dashboard"
                className="flex items-center hover:bg-btn-hover rounded-[10px] px-2 transition-colors duration-300 active:bg-white active:text-primary-black active:duration-10"
              >
                <span className="sm:hidden">Адмінка</span>
                <span className="hidden sm:block">Адмін панель</span>
              </Link>
            )}
          </div>
        </div>




        {/* Колонка 3 - профіль підтримка валюта */}
        <div className="flex gap-1 justify-items-center text-white text-sm font-bold ">

          {/* Профіль */}
          <div className="relative">
            <div
              ref={buttonRef}
              className="flex items-center gap-2 hover:bg-btn-primary-hover 
                          transition-colors duration-300 rounded-[10px] px-3 py-2
                          cursor-pointer "
              onClick={() => setProfileModalOpen(!isProfileModalOpen)}
            >
              {isLoggedIn && userName ? (
                <div className="w-8 h-8 rounded-full bg-blue-500 flex items-center justify-center text-white font-bold">
                  {userName?.charAt(0).toUpperCase() ?? "?"}
                </div>

              ) : (
                <FiUser className="text-white w-5 h-5" />
              )}
              <p className="text-white hidden sm:block">Профіль</p>
            </div>

            <ProfileModal
              isOpen={isProfileModalOpen}
              position={position}
              setProfileModalOpen={setProfileModalOpen}
              isLoggedIn={isLoggedIn}
              handleLogout={handleLogout}
            />
          </div>

          {/* Підтримка */}
          <a href="/" className="hidden md:flex items-center gap-1 hover:bg-btn-primary-hover transition-colors duration-300 rounded-[10px] px-3 py-2">
            <RiQuestionnaireFill className="text-white w-5 h-5" />
            <p className="">
              Підтримка
            </p>
          </a>
          {/* 
          // Валюта 
          <button className="flex items-center gap-1 hover:bg-btn-primary-hover 
          transition-colors duration-300 rounded-[10px] px-3 py-2 cursor-pointer">
            <TbWorld className="text-white w-5 h-5 " />
            <p>
              UAH
            </p>
          </button>

          <div className="relative">
            <div
              ref={buttonRef}
              className="flex items-center gap-1 hover:bg-btn-primary-hover 
            transition-colors duration-300 rounded-[10px] px-3 py-2"
              onClick={() => setProfileModalOpen(!isProfileModalOpen)}
            >
              <TbWorld className="text-white w-5 h-5 " />
              <p>
                UAH
              </p>
            </div>

            <ProfileModal
              isOpen={isProfileModalOpen}
              position={position}
              setProfileModalOpen={setProfileModalOpen}
              isLoggedIn={isLoggedIn}
              handleLogout={handleLogout}
            />
          </div> */}

        </div>

      </div>


    </nav>
  );
}

/* <div>{isLoggedIn && (
            <>
              <Link href="/my-bookings" className="text-gray-600 hover:text-primary-hover transition-colors">
                Мої бронювання
              </Link>
              <Link href="/my-fines" className="text-gray-600 hover:text-primary-hover transition-colors">
                Мої штрафи
              </Link>
              <Link href="/profile" className="text-gray-600 hover:text-primary-hover transition-colors">
                Профіль
              </Link>
              {userRole === "Admin" && (
                <Link href="/admin/dashboard" className="text-gray-600 hover:text-primary-hover transition-colors">
                  Адмін панель
                </Link>
              )}
            </>
          )}</div> */


