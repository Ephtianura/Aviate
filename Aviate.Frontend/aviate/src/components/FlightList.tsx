"use client";
import { useEffect, useState } from "react";
import { getFlights } from "@/hooks/apiFlights";
import FlightCard from "@/components/Cards/FlightCard";

interface FlightListProps {
    selectedFlight: any;
    setSelectedFlight: (flight: any) => void;
}

export default function FlightList({ selectedFlight, setSelectedFlight }: FlightListProps) {
  const [loading, setLoading] = useState(true);
  const [flights, setFlights] = useState<any[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const data = await getFlights();
      setFlights(data);
      setLoading(false);
    };
    fetchData();
  }, []);

  if (loading) return <p>Завантаження рейсів...</p>;
  if (!flights.length) return <p>Рейсів ще немає</p>;

  return (
    <div className="grid sm:grid-cols-1 md:grid-cols-2 gap-4">
      {flights.map(f => (
        <FlightCard 
          key={f.id} 
          flight={f} 
          bgColor={selectedFlight?.id === f.id ? "bg-blue-100" : "bg-gray-100"} 
          onClick={() => setSelectedFlight(f)} 
        />
      ))}
    </div>
  );
}
