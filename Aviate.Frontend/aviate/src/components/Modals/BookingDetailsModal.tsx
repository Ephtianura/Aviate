"use client";

import WhiteCard from "../Cards/WhiteCard";
import { IoTimeOutline } from "react-icons/io5";
import { useState } from "react";

export default function BookingDetailsModal({
    booking,
    close,
}: {
    booking: any;
    close: () => void;
}) {
    const { flight, seat, totalPrice, status, id } = booking;

    const dep = flight.departureAirport;
    const arr = flight.arrivalAirport;

    const departureTime = new Date(flight.departureTime);
    const arrivalTime = new Date(flight.arrivalTime);

    // Расчет времени в пути
    const diffMs = arrivalTime.getTime() - departureTime.getTime();
    const hours = Math.floor(diffMs / (1000 * 60 * 60));
    const minutes = Math.floor((diffMs / (1000 * 60)) % 60);

    const [paymentMethod, setPaymentMethod] = useState<number | null>(null);
    const [isPaying, setIsPaying] = useState(false);
    const [paymentResult, setPaymentResult] = useState<string | null>(null);

    const payBooking = async () => {
        if (paymentMethod === null) return;

        setIsPaying(true);
        setPaymentResult(null);

        try {
            const res = await fetch(
                `${process.env.NEXT_PUBLIC_API_URL}/bookings/${id}/pay?paymentMethod=${paymentMethod}`,
                {
                    method: "POST",
                    credentials: "include",
                }
            );

            const data = await res.json();

            if (data.isSuccessful) {
                setPaymentResult("Успішно оплачено!");
            } else {
                setPaymentResult("Помилка: " + data.errorMessage);
            }
        } catch (err) {
            setPaymentResult("Сталася помилка під час оплати.");
        } finally {
            setIsPaying(false);
        }
    };

    return (
        <>
            {/* Затемнение */}
            <div className="fixed inset-0 bg-black/20" onClick={close} />

            {/* Модалка */}
            <div className="fixed inset-0 z-50 flex justify-center items-center p-4">
                <WhiteCard>
                    <div className="p-6 flex flex-col gap-5 min-w-[380px]">

                        {/* Заголовок */}
                        <div className="flex justify-between items-center">
                            <h2 className="text-3xl font-bold text-primary-black">
                                Деталі бронювання
                            </h2>

                            <button
                                onClick={close}
                                className="text-gray-500 hover:text-gray-700 text-xl cursor-pointer"
                            >
                                ✕
                            </button>
                        </div>

                        {/* Маршрут */}
                        <div className="text-xl font-bold">
                            {dep.city}, {dep.country} ({dep.code}) → {arr.city}, {arr.country} ({arr.code})
                        </div>

                        {/* Аеропорти */}
                        <div className="flex flex-col gap-1">
                            <div className="text-sm text-gray-700">
                                <b>Аеропорт вильоту:</b> {dep.name}
                            </div>
                            <div className="text-sm text-gray-700">
                                <b>Аеропорт прильоту:</b> {arr.name}
                            </div>
                        </div>


                        {/* Час */}
                        <div className="grid grid-cols-2 gap-4 text-sm font-bold text-primary-black mt-2">
                            <div>
                                Виліт:
                                <div className="text-gray-text">
                                    {departureTime.toLocaleString("uk-UA")}
                                </div>
                            </div>

                            <div>
                                Приліт:
                                <div className="text-gray-text">
                                    {arrivalTime.toLocaleString("uk-UA")}
                                </div>
                            </div>
                        </div>

                        {/* Время в пути */}
                        <div className="text-sm text-gray-900 font-medium text-right flex gap-1 items-center">

                            <IoTimeOutline className="w-5 h-5" />
                            Час у дорозі: {hours} год {minutes} хв
                        </div>


                        {/* Місце */}
                        <div>
                            <b>Місце:</b> {seat.class} — {seat.seatNumber}
                        </div>

                        {/* Ціна */}
                        <div className="text-lg font-bold text-primary">
                            {totalPrice} грн
                        </div>

                        {/* Оплата */}
                        {status === 0 && (
                            <div className="mt-3 flex flex-col gap-3">
                                <b className="text-primary-black">Оплатити:</b>

                                <select
                                    className="border border-gray-300 rounded-xl px-3 py-2"
                                    value={paymentMethod ?? ""}
                                    onChange={(e) => setPaymentMethod(Number(e.target.value))}
                                >
                                    <option value="">Оберіть спосіб</option>
                                    <option value={0}>Карта</option>
                                    <option value={1}>PayPal</option>
                                    <option value={2}>Банківський переказ</option>
                                    <option value={3}>Готівка</option>
                                </select>

                                <button
                                    onClick={payBooking}
                                    disabled={paymentMethod === null || isPaying}
                                    className="bg-primary text-white px-4 py-2 rounded-xl hover:bg-primary-dark transition disabled:opacity-50"
                                >
                                    {isPaying ? "Оплата..." : "Оплатити"}
                                </button>

                                {paymentResult && (
                                    <p className="font-bold text-center text-primary-black">
                                        {paymentResult}
                                    </p>
                                )}
                            </div>
                        )}

                    </div>
                </WhiteCard>
            </div>
        </>
    );
}
