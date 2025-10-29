"use client";
import { useState } from "react";
import FlightForm from "@/components/FlightForm";
import FlightList from "@/components/FlightList";

export default function AdminFlightsPage() {
    // Состояние для выбранного рейса
    const [selectedFlight, setSelectedFlight] = useState<any>(null);

    return (
        <>
            <h1 className="text-4xl font-extrabold mb-8 text-primary drop-shadow-sm">✈️ Управління рейсами</h1>
            <div className="flex flex-col gap-4">
                <FlightForm
                    flightToEdit={selectedFlight}
                    onSuccess={() => {/* обновить список */ }}
                />
                <FlightList
                    selectedFlight={selectedFlight}
                    setSelectedFlight={setSelectedFlight}
                />
            </div>
        </>
    );
}
