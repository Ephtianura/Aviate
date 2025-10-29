    "use client";

    import { useEffect, useState } from "react";
    import WhiteCard from "@/components/Cards/WhiteCard";
    import { useToast } from "@/components/ToastProvider";

    type Airport = {
    id: string;
    name: string;
    code: string;
    country: string;
    city: string;
    };

    interface AirportFormProps {
    airportToEdit?: Airport | null;
    onSuccess?: (airport: Airport) => void;
    onCancel?: () => void;
    }

    export default function AirportForm({
    airportToEdit,
    onSuccess,
    onCancel,
    }: AirportFormProps) {
    const [form, setForm] = useState({
        name: "",
        code: "",
        country: "",
        city: "",
    });

    const [errors, setErrors] = useState<Record<string, string[]>>({});
    const [loading, setLoading] = useState(false);

    const { success, error } = useToast();

    // fill form when editing
    useEffect(() => {
        if (airportToEdit) {
        setForm({
            name: airportToEdit.name || "",
            code: airportToEdit.code || "",
            country: airportToEdit.country || "",
            city: airportToEdit.city || "",
        });
        } else {
        setForm({
            name: "",
            code: "",
            country: "",
            city: "",
        });
        }
    }, [airportToEdit]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm((prev) => ({
        ...prev,
        [e.target.name]: e.target.value,
        }));
    };

   const handleSubmit = async () => {
  setLoading(true);
  setErrors({});

  try {
    const isEdit = !!airportToEdit;

    const url = isEdit
      ? `${process.env.NEXT_PUBLIC_API_URL}/admin/airports/${airportToEdit!.id}`
      : `${process.env.NEXT_PUBLIC_API_URL}/admin/airports`;

    const res = await fetch(url, {
      method: isEdit ? "PUT" : "POST",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify(form),
    });

    if (!res.ok) {
      const err = await res.json().catch(() => ({}));

      if (err?.errors) {
        setErrors(err.errors);
      } else {
        error(
          isEdit
            ? "Помилка оновлення аеропорту"
            : "Помилка створення аеропорту"
        );
      }

      return;
    }

    const data = await res.json().catch(() => null);

    success(isEdit ? "Аеропорт оновлено" : "Аеропорт створено");

    onSuccess?.(data);

    if (!isEdit) {
      setForm({
        name: "",
        code: "",
        country: "",
        city: "",
      });
    }

    onCancel?.();
  } catch {
    error("Щось пішло не так");
  } finally {
    setLoading(false);
  }
};
    const inputStyle =
        "border border-gray-300 rounded px-3 py-2 w-full focus:ring-2 focus:ring-primary focus:outline-none";

    return (
        <WhiteCard>
        <h2 className="text-xl font-bold mb-4">
            {airportToEdit ? "Редагувати аеропорт" : "Створити аеропорт"}
        </h2>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
            <label className="block text-sm font-medium mb-1">Назва</label>
            <input
                className={inputStyle}
                name="name"
                value={form.name}
                onChange={handleChange}
            />
            {errors.Name && (
                <p className="text-red-500 text-sm">{errors.Name.join(", ")}</p>
            )}
            </div>

            <div>
            <label className="block text-sm font-medium mb-1">
                Код (IATA)
            </label>
            <input
                className={inputStyle}
                name="code"
                value={form.code}
                onChange={handleChange}
                maxLength={10}
            />
            {errors.Code && (
                <p className="text-red-500 text-sm">{errors.Code.join(", ")}</p>
            )}
            </div>

            <div>
            <label className="block text-sm font-medium mb-1">
                Країна
            </label>
            <input
                className={inputStyle}
                name="country"
                value={form.country}
                onChange={handleChange}
            />
            {errors.Country && (
                <p className="text-red-500 text-sm">
                {errors.Country.join(", ")}
                </p>
            )}
            </div>

            <div>
            <label className="block text-sm font-medium mb-1">Місто</label>
            <input
                className={inputStyle}
                name="city"
                value={form.city}
                onChange={handleChange}
            />
            {errors.City && (
                <p className="text-red-500 text-sm">{errors.City.join(", ")}</p>
            )}
            </div>
        </div>

        <div className="flex gap-2 mt-4">
            <button
            onClick={handleSubmit}
            disabled={loading}
            className="px-6 py-2 bg-primary text-white font-semibold rounded hover:bg-primary-dark"
            >
            {loading
                ? "Завантаження..."
                : airportToEdit
                ? "Оновити"
                : "Створити"}
            </button>

            {airportToEdit && (
            <button
                onClick={onCancel}
                className="px-4 py-2 border rounded"
            >
                Скасувати
            </button>
            )}
        </div>
        </WhiteCard>
    );
    }