"use client";

import { useEffect, useState } from "react";
import { useAuth } from "@/context/AuthContext";
import { ProfileLayout } from "@/components/Layouts/ProfileLayout";
import BookingCard from "@/components/Cards/BookingCard";
import WhiteCard from "@/components/Cards/WhiteCard";
import Pagination from "@/components/Pagination";

export default function MyBookings() {
  const { isLoggedIn } = useAuth();

  const [bookings, setBookings] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  const [page, setPage] = useState(1);
  const pageSize = 5;
  const [totalPages, setTotalPages] = useState(1);

  const fetchBookings = async (page: number = 1) => {
    try {
      setLoading(true);

      const res = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/bookings/my?page=${page}&pageSize=${pageSize}&SortBy=BookingDate&SortDesc=true`,
        { credentials: "include" }
      );

      const data = await res.json();

      setBookings(data.items || []);
      setTotalPages(data.totalPages || 1);

    } catch (err) {
      console.error("Error loading bookings:", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBookings(page);
  }, [page]);

  return (
    <ProfileLayout>
      <div className="flex flex-col gap-4">

        <div className="mb-2">
          <h1 className="text-4xl font-bold text-primary-black">Мої бронювання</h1>
        </div>

        <WhiteCard>
          <div className="flex flex-col gap-4">
            <h1 className="text-primary-black text-xl font-bold">
              Мої бронювання
            </h1>

            {loading && <p className="text-gray-text">Завантаження...</p>}
            {!loading && bookings.length === 0 && (
              <p className="text-gray-text font-bold">
                У вас немає бронювань.
              </p>
            )}

            {totalPages > 1 && (
              <div className="">
                <Pagination
                  page={page}
                  totalPages={totalPages}
                  onPageChange={setPage}
                />
              </div>)}
            <div className="flex flex-col gap-6">
              {bookings.map((b) => (
                <BookingCard key={b.id} booking={b} />
              ))}
            </div>

            {/* PAGINATION */}
            {totalPages > 1 && (
              <div>
                <div className="mt-4">
                  <Pagination
                    page={page}
                    totalPages={totalPages}
                    onPageChange={setPage}
                  />
                </div>
                {/* <div className="flex justify-center mt-8">

                  <div className="flex items-center gap-3 bg-white px-4 py-2 rounded-xl shadow-md border">

                    <button
                      onClick={() => setPage((p) => Math.max(1, p - 1))}
                      disabled={page === 1}
                      className={`
                  p-2 rounded-lg transition-all duration-200
                  ${page === 1
                          ? "text-gray-400 cursor-not-allowed"
                          : "hover:bg-gray-100 text-primary-black"}
                  `}
                    >
                      <span className="text-2xl">←</span>
                    </button>

                    <div className="flex items-center gap-2">
                      {Array.from({ length: totalPages }, (_, i) => i + 1).map((p) => (
                        <button
                          key={p}
                          onClick={() => setPage(p)}
                          className={`
              w-9 h-9 flex items-center justify-center rounded-lg border text-sm font-medium
              transition-all duration-200
              ${p === page
                              ? "bg-primary text-white border-primary shadow-md scale-105"
                              : "bg-white hover:bg-gray-100 border-gray-300 text-primary-black"
                            }
            `}
                        >
                          {p}
                        </button>
                      ))}
                    </div>

                    <button
                      onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
                      disabled={page === totalPages}
                      className={`
          p-2 rounded-lg transition-all duration-200
          ${page === totalPages
                          ? "text-gray-400 "
                          : "hover:bg-gray-100 text-primary-black"}
        `}
                    >
                      <span className="text-2xl">→</span>
                    </button>

                  </div>
                </div> */}
              </div>
            )}

          </div>

        </WhiteCard>
      </div>
    </ProfileLayout>
  );
}
