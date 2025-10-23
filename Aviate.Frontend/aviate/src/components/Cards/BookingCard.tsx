"use client";

import WhiteCard from "./WhiteCard";
import { BsArrowRightShort } from "react-icons/bs";
import { useState } from "react";
import BookingDetailsModal from "@/components/Modals/BookingDetailsModal";

export default function BookingCard({ booking }: { booking: any }) {
  const {
    flight,
    totalPrice,
    status,
  } = booking;

  const departure = flight.departureAirport;
  const arrival = flight.arrivalAirport;

  const [isOpen, setIsOpen] = useState(false);

  const formatDate = (dateStr: string) => {
    return new Date(dateStr).toLocaleString("uk-UA", {
      day: "2-digit",
      month: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const bookingStatusMap: Record<number, string> = {
    0: "Очікує оплату",
    1: "Оплачено",
    2: "Скасовано",
  };

  const flightStatusMap: Record<number, string> = {
    0: "Заплановано",
    1: "У польоті",
    2: "Перенесено",
    3: "Скасовано",
    4: "Завершено",
  };

  return (
    <>
      <div className="p-6 rounded-2xl shadow-[0_0_15px_rgba(0,0,0,0.2)] bg-gray-50">
        <div className="flex flex-col gap-4">

          {/* Міста */}
          <div className="flex items-center text-xl font-bold text-primary-black">
            {departure.city} — {arrival.city}
          </div>

          {/* Номер рейсу */}
          <p className="text-gray-text text-sm font-bold">
            Рейс: <span className="text-primary-black">{flight.flightNumber}</span>
          </p>

          {/* Время */}
          <div className="flex gap-10 text-sm text-primary-black font-bold mt-2">
            <div>
              Виліт:
              <div className="text-gray-text">{formatDate(flight.departureTime)}</div>
            </div>
            <div>
              Приліт:
              <div className="text-gray-text">{formatDate(flight.arrivalTime)}</div>
            </div>
          </div>

          {/* Ціна + статуси */}
          <div className="flex justify-between items-center">
            <div className="text-lg font-bold text-primary">
              {totalPrice} грн
            </div>

            <div className="flex flex-col text-right text-sm">
              <span className="font-bold text-primary-black">
                Статус бронювання:
              </span>
              <span className="text-gray-text">{bookingStatusMap[status]}</span>

              <span className="font-bold text-primary-black mt-1">
                Статус рейсу:
              </span>
              <span className="text-gray-text">{flightStatusMap[flight.status]}</span>
            </div>
          </div>

          {/* Кнопка детальніше */}
          <button
            onClick={() => setIsOpen(true)}
            className="flex items-center gap-2 text-primary font-bold hover:text-primary-dark transition cursor-pointer"
          >
            Детальніше <BsArrowRightShort className="w-6 h-6" />
          </button>
        </div>
      </div>

      {isOpen && (
        <BookingDetailsModal booking={booking} close={() => setIsOpen(false)} />
      )}
    </>
  );
}
