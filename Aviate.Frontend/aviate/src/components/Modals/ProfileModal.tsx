"use client";

import Link from "next/link";
import { IoMdSettings, IoMdClose } from "react-icons/io";
import { TbPlaneInflight } from "react-icons/tb";
import { LuLogIn, LuLogOut } from "react-icons/lu";
import { LoginModal } from "./LoginModal";
import { RegisterModal } from "./RegisterModal";
import { useState } from "react";

interface ProfileModalProps {
    isOpen: boolean;
    position: { top: number; left: number };
    setProfileModalOpen: (value: boolean) => void;
    isLoggedIn: boolean;
    handleLogout: () => void;
}

export const ProfileModal: React.FC<ProfileModalProps> = ({
    isOpen,
    setProfileModalOpen,
    isLoggedIn,
    handleLogout,
}) => {
    const [isLoginOpen, setLoginOpen] = useState(false);
    const [isRegisterOpen, setRegisterOpen] = useState(false);

    if (!isOpen) return null;

    return (
        <div className="absolute mt-1 right-0 bg-white p-4 rounded-xl text-primary-black shadow-lg w-55">
            <div className="mb-6 flex flex-col gap-2">
                <Link
                    href={"/my/settings"}
                    className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]"
                >
                    <div className="flex gap-3 items-center">
                        <IoMdSettings className="w-5 h-5 text-gray-text" />
                        <p className="font-bold">Налаштування</p>
                    </div>
                </Link>

                <Link
                    href={"/my/bookings"}
                    className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]"
                >
                    <div className="flex gap-3 items-center">
                        <TbPlaneInflight className="w-5 h-5 text-gray-text" />
                        <p className="font-bold">Мої бронювання</p>
                    </div>
                </Link>

                <button
                    className="hover:bg-gray-very-light rounded-xl px-2 py-[6px] cursor-pointer"
                    onClick={() => setProfileModalOpen(false)}
                >
                    <div className="flex gap-3 items-center">
                        <IoMdClose className="w-5 h-5 text-gray-text" />
                        <p className="font-bold">Закрити</p>
                    </div>
                </button>

                {isLoggedIn ? (
                    <button
                        onClick={handleLogout}
                        className="bg-gray-very-light text-primary-black px-4 py-1 rounded-xl hover:bg-gray-300 
                        font-bold w-full flex items-center gap-2 justify-center cursor-pointer px-3 py-2 transition-colors deuration-200"
                    >
                        <LuLogOut className="w-5 h-5" /> Вийти
                    </button>
                ) : (
                    <button
                        onClick={() => setLoginOpen(true)}
                        className="bg-[#1071F2] hover:bg-[#1172F3] active:bg-[#0E64DB] text-white 
                        font-bold rounded-xl px-3 py-2 w-full flex items-center gap-2 justify-center cursor-pointer"
                    >
                        <LuLogIn className="w-5 h-5" /> Увійти
                    </button>
                )}
            </div>

            {/* Модалки */}
            <LoginModal
                isOpen={isLoginOpen}
                onClose={() => setLoginOpen(false)}
                onSwitchToRegister={() => {
                    setLoginOpen(false);
                    setRegisterOpen(true);
                }}
            />

            <RegisterModal
                isOpen={isRegisterOpen}
                onClose={() => setRegisterOpen(false)}
                onSwitchToLogin={() => {
                    setRegisterOpen(false);
                    setLoginOpen(true);
                }}
            />
        </div>
    );
};
