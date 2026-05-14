"use client";
import { useEffect, useState } from "react";
import { getFlights } from "@/hooks/apiFlights";
import FlightCard from "@/components/Cards/FlightCard";
import { div } from "framer-motion/client";
import Pagination from "./Pagination";

interface FlightListProps {
  selectedFlight: any;
  setSelectedFlight: (flight: any) => void;
}

export default function FlightList({ selectedFlight, setSelectedFlight }: FlightListProps) {
  const [loading, setLoading] = useState(true);
  const [flights, setFlights] = useState<any[]>([]);

  const [page, setPage] = useState(1);
  const [pageSize] = useState(20);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);

      const data = await getFlights(page, pageSize);

      setFlights(data.items ?? []);
      setTotalPages(data.totalPages ?? 1);

      setLoading(false);
    };

    fetchData();
  }, [page]);

  if (loading) return <p>Завантаження рейсів...</p>;
  if (!flights.length) return <p>Рейсів ще немає</p>;

  return (
    <div>
      <div className="mb-4">
        <Pagination
          page={page}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      </div>
      <div className="grid md:grid-cols-1 lg:grid-cols-2 gap-4 w-full">


        {flights.map(f => (
          <FlightCard
            key={f.id}
            flight={f}
            bgColor={selectedFlight?.id === f.id ? "bg-blue-100" : "bg-gray-100"}
            onClick={() => setSelectedFlight(f)}
          />
        ))}
      </div>
      <div className="mt-4">
        <Pagination
          page={page}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      </div>
    </div>
  );
}
