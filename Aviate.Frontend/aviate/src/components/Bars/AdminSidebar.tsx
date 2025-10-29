"use client";

import Link from "next/link";
import { useAuth } from "@/context/AuthContext";
import { IoIosAirplane } from "react-icons/io";
import { TbBuildingAirport, TbBrandBooking, TbPlaneInflight } from "react-icons/tb";
import { FaUsers } from "react-icons/fa";
import { MdFlightTakeoff } from "react-icons/md";
import { ImStatsDots } from "react-icons/im";

export default function AdminSidebar() {
  const { userRole } = useAuth();

  return (
    <div className="mt-1 right-0 bg-white p-4 rounded-xl text-primary-black shadow-lg w-55">
      <div className="flex flex-col gap-2">
        {(userRole === "Admin" || userRole === "Employee") && (
          <>
            <Link href={"/admin/dashboard"} className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]">
              <div className="flex gap-3 items-center">
                <ImStatsDots className="w-5 h-5 text-gray-text" />
                <p className="font-bold">Статистика</p>
              </div>
            </Link>
          </>
        )}

        {(userRole === "Admin" ) && (
          <>
            <Link href={"/admin/users"} className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]">
              <div className="flex gap-3 items-center">
                <FaUsers className="w-5 h-5 text-gray-text" />
                <p className="font-bold">Користувачі</p>
              </div>
            </Link>

            <Link href={"/admin/airports"} className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]">
              <div className="flex gap-3 items-center">
                <TbBuildingAirport className="w-5 h-5 text-gray-text" />
                <p className="font-bold">Аеропорти</p>
              </div>
            </Link>
             <Link href={"/admin/airplanes"} className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]">
              <div className="flex gap-3 items-center">
                <IoIosAirplane className="w-5 h-5 text-gray-text" />
                <p className="font-bold">Літаки</p>
              </div>
            </Link>

            <Link href={"/admin/flights"} className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]">
              <div className="flex gap-3 items-center">
                <MdFlightTakeoff className="w-5 h-5 text-gray-text" />
                <p className="font-bold">Рейси</p>
              </div>
            </Link>

            {/* <Link href={"/admin/bookings"} className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]">
              <div className="flex gap-3 items-center">
                <TbBrandBooking className="w-5 h-5 text-gray-text" />
                <p className="font-bold">Бронювання</p>
              </div>
            </Link> */}
          </>
        )}

      {userRole === "Employee" && (
        <Link href={"/admin/flights"} className="hover:bg-gray-very-light rounded-xl px-2 py-[6px]">
          <div className="flex gap-3 items-center">
            <MdFlightTakeoff className="w-5 h-5 text-gray-text" />
            <p className="font-bold">Рейси</p>
          </div>
        </Link>
      )}
    </div>
    </div >
  );
}
