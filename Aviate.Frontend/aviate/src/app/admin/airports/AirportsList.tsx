"use client";

import WhiteCard from "@/components/Cards/WhiteCard";
import Pagination from "@/components/Pagination";

type Airport = {
  id: string;
  name: string;
  code: string;
  city: string;
  country: string;
};

interface Props {
  loading: boolean;
  airports: Airport[];
  page: number;
  totalPages: number;
  setPage: (page: number) => void;
  setEditingAirport: (airport: Airport) => void;
  deleteAirport: (id: string) => void;
}

export default function AirportsList({
  loading,
  airports,
  page,
  totalPages,
  setPage,
  setEditingAirport,
  deleteAirport,
}: Props) {
  return (
    <WhiteCard>
      {loading ? (
        <p className="text-gray-500">Завантаження аеропортів...</p>
      ) : (
        <>
          <div className="mb-4">
            <Pagination
              page={page}
              totalPages={totalPages}
              onPageChange={setPage}
            />
          </div>

          <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {airports.map((airport) => (
              <div
                key={airport.id}
                className="border flex flex-col justify-between border-gray-200 rounded-lg p-4 hover:shadow-lg transition-shadow bg-white"
              >
                <div>
                  <h2 className="text-lg font-bold">{airport.name}</h2>
                  <p className="text-sm text-gray-500">
                    <b>Код:</b> {airport.code}
                  </p>
                  <p className="text-sm text-gray-500">
                    <b>Місто:</b> {airport.city}
                  </p>
                  <p className="text-sm text-gray-500">
                    <b>Країна:</b> {airport.country}
                  </p>
                </div>

                <div className="flex gap-2 mt-3">
                  <button
                    onClick={() => setEditingAirport(airport)}
                    className="w-full bg-blue-400 text-white py-1 rounded hover:bg-blue-500"
                  >
                    Редагувати
                  </button>

                  <button
                    onClick={() => deleteAirport(airport.id)}
                    className="w-full bg-red-300 text-white py-1 rounded hover:bg-red-400"
                  >
                    Видалити
                  </button>
                </div>
              </div>
            ))}
          </div>

          <div className="mt-4">
            <Pagination
              page={page}
              totalPages={totalPages}
              onPageChange={setPage}
            />
          </div>
        </>
      )}
    </WhiteCard>
  );
}