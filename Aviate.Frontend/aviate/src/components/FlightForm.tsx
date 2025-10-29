"use client";
import { useState, useEffect } from "react";
import { getAirports, getAirplanes, createFlight, updateFlight } from "@/hooks/apiFlights";
import WhiteCard from "@/components/Cards/WhiteCard";
import { useToast } from "@/components/ToastProvider";

interface FlightFormProps {
  flightToEdit?: any;
  onSuccess?: () => void;
}

const flightStatusOptions = [
  { value: 0, label: "Заплановано" },
  { value: 1, label: "У польоті" },
  { value: 2, label: "Перенесено" },
  { value: 3, label: "Скасовано" },
  { value: 4, label: "Завершено" },
];

export default function FlightForm({ flightToEdit, onSuccess }: FlightFormProps) {
  const { error, success } = useToast();
  const [airports, setAirports] = useState<any[]>([]);
  const [airplanes, setAirplanes] = useState<any[]>([]);
  const [form, setForm] = useState({
    airplaneId: "",
    departureAirportId: "",
    arrivalAirportId: "",
    basePrice: 0,
    departureTime: "",
    arrivalTime: "",
    economySeats: 0,
    businessSeats: 0,
    firstClassSeats: 0,
    status: 0,
  });
  const [errors, setErrors] = useState<Record<string, string[]>>({});
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (flightToEdit) {
      setForm({
        airplaneId: flightToEdit.airplaneId,
        departureAirportId: flightToEdit.departureAirportId,
        arrivalAirportId: flightToEdit.arrivalAirportId,
        basePrice: flightToEdit.basePrice,
        departureTime: flightToEdit.departureTime.slice(0, 16),
        arrivalTime: flightToEdit.arrivalTime.slice(0, 16),
        economySeats: flightToEdit.economySeats || 0,
        businessSeats: flightToEdit.businessSeats || 0,
        firstClassSeats: flightToEdit.firstClassSeats || 0,
        status: flightToEdit.status ?? 0,
      });
    }
  }, [flightToEdit]);

  useEffect(() => {
    const fetchData = async () => {
      const [airportsData, airplanesData] = await Promise.all([getAirports(), getAirplanes()]);
      setAirports(airportsData);
      setAirplanes(airplanesData);
    };
    fetchData();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const value = e.target.type === "number" ? Number(e.target.value) : e.target.value;
    setForm({ ...form, [e.target.name]: value });
  };

  const handleSubmit = async () => {
    setLoading(true);
    setErrors({});
    try {
      if (flightToEdit) {
        await updateFlight(flightToEdit.id, form);
      } else {
        await createFlight(form);
      }
      onSuccess?.();
      if (!flightToEdit) {
        setForm({
          airplaneId: "",
          departureAirportId: "",
          arrivalAirportId: "",
          basePrice: 0,
          departureTime: "",
          arrivalTime: "",
          economySeats: 0,
          businessSeats: 0,
          firstClassSeats: 0,
          status: 0,
        });
        success("Успішно!");
      }
    } catch (err: any) {
      if (err.data?.errors) setErrors(err.data.errors);
      else error(`Помилка: ${err?.message ?? "Помилка"}`);
    } finally {
      setLoading(false);
    }
  };

  const inputStyle = "border border-gray-300 rounded px-3 py-2 w-full focus:ring-2 focus:ring-primary focus:outline-none";

  return (
    <WhiteCard>
      <h2 className="text-xl font-bold mb-4">{flightToEdit ? "Редагувати рейс" : "Створити рейс"}</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-medium mb-1">Аеропорт відправлення</label>
          <select className={inputStyle} name="departureAirportId" value={form.departureAirportId} onChange={handleChange}>
            <option value="">Виберіть аеропорт відправлення</option>
            {airports.map(a => <option key={a.id} value={a.id}>{a.city} ({a.code})</option>)}
          </select>
          {errors.DepartureAirportId && <p className="text-red-500 text-sm">{errors.DepartureAirportId.join(", ")}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Аеропорт прибуття</label>
          <select className={inputStyle} name="arrivalAirportId" value={form.arrivalAirportId} onChange={handleChange}>
            <option value="">Виберіть аеропорт прибуття</option>
            {airports.map(a => <option key={a.id} value={a.id}>{a.city} ({a.code})</option>)}
          </select>
          {errors.ArrivalAirportId && <p className="text-red-500 text-sm">{errors.ArrivalAirportId.join(", ")}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Літак</label>
          <select className={inputStyle} name="airplaneId" value={form.airplaneId} onChange={handleChange}>
            <option value="">Виберіть літак</option>
            {airplanes.map(a => <option key={a.id} value={a.id}>{a.model} ({a.registrationNumber})</option>)}
          </select>
          {errors.AirplaneId && <p className="text-red-500 text-sm">{errors.AirplaneId.join(", ")}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Ціна, ₴</label>
          <input className={inputStyle} type="number" name="basePrice" value={form.basePrice} onChange={handleChange} />
          {errors.BasePrice && <p className="text-red-500 text-sm">{errors.BasePrice.join(", ")}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Час відправлення</label>
          <input className={inputStyle} type="datetime-local" name="departureTime" value={form.departureTime} onChange={handleChange} />
          {errors.DepartureTime && <p className="text-red-500 text-sm">{errors.DepartureTime.join(", ")}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Час прибуття</label>
          <input className={inputStyle} type="datetime-local" name="arrivalTime" value={form.arrivalTime} onChange={handleChange} />
          {errors.ArrivalTime && <p className="text-red-500 text-sm">{errors.ArrivalTime.join(", ")}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Місць економ</label>
          <input className={inputStyle} type="number" name="economySeats" value={form.economySeats} onChange={handleChange} />
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Місць бізнес</label>
          <input className={inputStyle} type="number" name="businessSeats" value={form.businessSeats} onChange={handleChange} />
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Місць перший клас</label>
          <input className={inputStyle} type="number" name="firstClassSeats" value={form.firstClassSeats} onChange={handleChange} />
        </div>

        <div>
          <label className="block text-sm font-medium mb-1">Статус рейсу</label>
          <select className={inputStyle} name="status" value={form.status} onChange={handleChange}>
            {flightStatusOptions.map(opt => (
              <option key={opt.value} value={opt.value}>{opt.label}</option>
            ))}
          </select>
        </div>
      </div>

      <button
        onClick={handleSubmit}
        disabled={loading}
        className="mt-4 px-6 py-2 bg-primary text-white font-semibold rounded hover:bg-primary-dark"
      >
        {loading ? "Завантаження..." : flightToEdit ? "Оновити" : "Створити"}
      </button>
    </WhiteCard>
  );
}
