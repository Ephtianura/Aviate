"use client";

import Link from "next/link";
import { IoMdSettings, IoMdClose } from "react-icons/io";
import { TbPlaneInflight } from "react-icons/tb";



export default function SidebarProfile() {


  return (
    <div className=" mt-1 right-0 bg-white p-4 rounded-xl text-primary-black shadow-lg w-55">
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



      </div>

    </div>
  );
};
