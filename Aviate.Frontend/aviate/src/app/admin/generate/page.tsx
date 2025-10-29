"use client";

import { useState } from "react";
import WhiteCard from "@/components/Cards/WhiteCard";
import { apiFetch } from "@/lib/api";
import { useToast } from "@/components/ToastProvider";

export default function DegeneratePage() {
  const { success, error } = useToast();

  const [loading, setLoading] = useState(false);
  const [used, setUsed] = useState(false);
  const [usedUkr, setUsedUkr] = useState(false);

  const handleGenerate = async (ukr = false) => {
    if (loading) return;
    if (!ukr && used) return;
    if (ukr && usedUkr) return;

    const confirm1 = confirm("Ви впевнені?");
    if (!confirm1) return;

    const confirm2 = confirm("Це займе багато часу. Продовжити?");
    if (!confirm2) return;

    try {
      setLoading(true);

      success("Генерацію рейсів запущено 🚀🔥🔥🔥");

      const url = ukr
        ? "/degenerate/generate-flights?AreYouSure=true&Ukr=true"
        : "/degenerate/generate-flights?AreYouSure=true";

      await apiFetch(url, { method: "POST" });

      success("Генерація успішно проведена✅");

      if (ukr) setUsedUkr(true);
      else setUsed(true);
    } catch {
      error("Помилка при запуску генерації ❌");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <h1 className="text-4xl font-extrabold mb-8 text-primary">
        ⚙️ Генерація рейсів
      </h1>

      <WhiteCard>
        <div className="flex flex-col items-center gap-6 py-10">
          <p className="text-gray-600 text-center max-w-md">
            Ця дія згенерує велику кількість рейсів. Її можна виконати лише один раз до перезавантаження сторінки.
          </p>

          <div className="flex gap-4">
            <button
              onClick={() => handleGenerate(false)}
              disabled={loading || used}
              className={`px-6 py-3 rounded text-white font-semibold transition ${
                used
                  ? "bg-gray-400 cursor-not-allowed"
                  : loading
                    ? "bg-yellow-400"
                    : "bg-primary hover:bg-primary/80"
              }`}
            >
              {used ? "Вже виконано" : loading ? "Виконується..." : "Звичайні рейси"}
            </button>

            <button
              onClick={() => handleGenerate(true)}
              disabled={loading || usedUkr}
              className={`px-6 py-3 rounded text-white font-semibold transition ${
                usedUkr
                  ? "bg-gray-400 cursor-not-allowed"
                  : loading
                    ? "bg-yellow-400"
                    : "bg-blue-600 hover:bg-blue-700"
              }`}
            >
              {usedUkr ? "Вже виконано" : loading ? "Виконується..." : "🇺🇦 УКР рейси"}
            </button>
          </div>
        </div>
      </WhiteCard>
    </>
  );
}