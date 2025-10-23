"use client";

import { useState, useEffect, useRef } from "react";
import { useRouter } from "next/navigation";
import DatePicker, { registerLocale } from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { MdDateRange } from "react-icons/md";
import { uk } from "date-fns/locale";
import { apiFetch } from "@/lib/api"; // твой общий fetch

registerLocale("uk", uk);

interface Airport {
  id: string;
  name: string;
  code: string;
  city: string;
  country: string;
}

export default function SearchCard() {
  const router = useRouter();

  const [fromInput, setFromInput] = useState("");
  const [toInput, setToInput] = useState("");

  const [fromAirports, setFromAirports] = useState<Airport[]>([]);
  const [toAirports, setToAirports] = useState<Airport[]>([]);

  const [selectedFrom, setSelectedFrom] = useState<Airport | null>(null);
  const [selectedTo, setSelectedTo] = useState<Airport | null>(null);

  const [startDate, setStartDate] = useState<Date | null>(null);
  const [endDate, setEndDate] = useState<Date | null>(null);

  const [price, setPrice] = useState<number | null>(100);

  const fromRef = useRef<HTMLDivElement>(null);
  const toRef = useRef<HTMLDivElement>(null);

  // Функция автокомплита аэропортов
  const fetchAirports = async (query: string, setAirports: any) => {
    if (!query) {
      setAirports([]);
      return;
    }
    try {
      const data = await apiFetch(`/airports?Search=${query}`);
      setAirports(data.items || []);
    } catch (e) {
      console.error(e);
      setAirports([]);
    }
  };

  // При выборе даты делаем запрос на минимальную цену
  useEffect(() => {
    const fetchPrice = async () => {
      if (!selectedFrom || !selectedTo || !startDate) {
        setPrice(100);
        return;
      }
      try {
        const params = new URLSearchParams();
        params.append("DepartureAirportId", selectedFrom.id);
        params.append("ArrivalAirportId", selectedTo.id);
        params.append("DepartureFrom", startDate.toISOString());

        const data = await apiFetch(`/flights?${params.toString()}`);
        if (data.items.length > 0) {
          const minPrice = Math.min(...data.items.map((f: any) => f.basePrice));
          setPrice(minPrice);
        }
      } catch (e) {
        console.error(e);
      }
    };
    fetchPrice();
  }, [startDate, selectedFrom, selectedTo]);

  // Закрываем автокомплит при клике вне
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        fromRef.current &&
        !fromRef.current.contains(event.target as Node)
      ) {
        setFromAirports([]);
      }
      if (
        toRef.current &&
        !toRef.current.contains(event.target as Node)
      ) {
        setToAirports([]);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleSearchFlights = () => {
    const params: any = {};
    if (selectedFrom) params.DepartureAirportId = selectedFrom.id;
    if (selectedTo) params.ArrivalAirportId = selectedTo.id;
    if (startDate) params.DepartureFrom = toLocalOffsetISO(startDate);
    if (endDate) params.DepartureTo = toLocalOffsetISO(endDate);


    const query = new URLSearchParams(params).toString();
    router.push(`/flights?${query}`);
  };
  function toLocalOffsetISO(date: Date) {
    const tzOffset = -date.getTimezoneOffset();
    const diff = tzOffset >= 0 ? "+" : "-";
    const pad = (n: number) => String(Math.floor(Math.abs(n))).padStart(2, "0");

    const offsetHours = pad(tzOffset / 60);
    const offsetMinutes = pad(tzOffset % 60);

    return (
      date.getFullYear() +
      "-" +
      pad(date.getMonth() + 1) +
      "-" +
      pad(date.getDate()) +
      "T" +
      pad(date.getHours()) +
      ":" +
      pad(date.getMinutes()) +
      ":" +
      pad(date.getSeconds()) +
      diff +
      offsetHours +
      ":" +
      offsetMinutes
    );
  }

  return (
    <div className="bg-primary w-full py-4">
      <div className="flex mx-auto px-4 py-2 justify-center">
        <div className="flex gap-2">
          {/* Inputs */}
          <div className="flex gap-0.5 flex-1 rounded-xl shadow-[0_0_25px_rgba(0,0,0,0.3)]">
            {/* Звідки */}
            <div className="relative flex-1" ref={fromRef}>
              <input
                type="text"
                placeholder="Звідки"
                className="bg-white rounded-l-xl px-4 py-4 w-full focus:outline-[#ED552B]"
                value={fromInput}
                onChange={(e) => {
                  setFromInput(e.target.value);
                  fetchAirports(e.target.value, setFromAirports);
                }}
              />
              {fromAirports.length > 0 && (
                <div className="absolute top-full left-0 right-0 bg-white shadow-md z-20 max-h-60 overflow-y-auto rounded-xl mt-1">
                  {fromAirports.map((airport) => (
                    <div
                      key={airport.id}
                      className="px-4 py-2 hover:bg-gray-200 cursor-pointer"
                      onClick={() => {
                        setSelectedFrom(airport);
                        setFromInput(airport.city);
                        setFromAirports([]);
                      }}
                    >
                       <div className="flex flex-col">
                        <div className="flex gap-2">
                          <div className="">{airport.city}</div>
                          <div className="text-gray-text">{airport.code}</div>
                        </div>
                        <div className="text-gray-text text-sm">{airport.country}</div>
                        <div>

                        </div>

                      </div>
                    </div>
                  ))}
                </div>
              )}
            </div>

            {/* Куди */}
            <div className="relative flex-1" ref={toRef}>
              <input
                type="text"
                placeholder="Куди"
                className="bg-white px-4 py-4 w-full focus:outline-[#ED552B]"
                value={toInput}
                onChange={(e) => {
                  setToInput(e.target.value);
                  fetchAirports(e.target.value, setToAirports);
                }}
              />
              {toAirports.length > 0 && (
                <div className="absolute top-full left-0 right-0 bg-white shadow-md z-20 max-h-60 overflow-y-auto rounded-xl mt-1">
                  {toAirports.map((airport) => (
                    <div
                      key={airport.id}
                      className="px-4 py-2 hover:bg-gray-200 cursor-pointer"
                      onClick={() => {
                        setSelectedTo(airport);
                        setToInput(airport.city);
                        setToAirports([]);
                      }}
                    >
                      <div className="flex flex-col">
                        <div className="flex gap-2">
                          <div className="">{airport.city}</div>
                          <div className="text-gray-text">{airport.code}</div>
                        </div>
                        <div className="text-gray-text text-sm">{airport.country}</div>
                        <div>

                        </div>

                      </div>

                    </div>
                  ))}
                </div>
              )}
            </div>

            {/* Дата в одну сторону */}
            <div className="relative">
              <DatePicker
                selected={startDate}
                onChange={(date) => setStartDate(date)}
                placeholderText="Коли"
                locale="uk"
                dateFormat="d MMMM"
                className="bg-white px-4 py-4 focus:outline-[#ED552B]"
                renderDayContents={(day) => (
                  <div className="flex flex-col items-center">
                    <span>{day}</span>
                    <span className="text-xs text-primary-green">
                      {price ? `${price}₴` : "100₴"}
                    </span>
                  </div>
                )}
              />
              <div className="absolute right-4 top-[17px]">
                <MdDateRange className="bg-white text-primary w-5 h-5" />
              </div>
            </div>

            {/* Дата назад */}
            <div className="relative">
              <DatePicker
                selected={endDate}
                onChange={(date) => setEndDate(date)}
                placeholderText="Назад"
                locale="uk"
                dateFormat="d MMMM"
                className="bg-white rounded-r-xl px-4 py-4 focus:outline-[#ED552B]"
                renderDayContents={(day) => (
                  <div className="flex flex-col items-center">
                    <span>{day}</span>
                    <span className="text-xs text-primary-green">
                      {price ? `${price}₴` : "100₴"}
                    </span>
                  </div>
                )}
              />
              <div className="absolute right-4 top-[17px]">
                <MdDateRange className="bg-white text-primary w-5 h-5" />
              </div>
            </div>
          </div>

          {/* Кнопка */}
          <button
            className="bg-[#FA742D] shadow-[0_0_25px_rgba(0,0,0,.2)] rounded-xl px-10 py-3 text-white font-bold hover:bg-[#ED7332] active:bg-[#D3662D] transition-colors"
            onClick={handleSearchFlights}
          >
            Знайти квитки
          </button>
        </div>
      </div>
    </div>
  );
}
