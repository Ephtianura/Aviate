"use client";

import { useState } from "react";
import AirplaneForm from "./AirplaneForm";
import AirplaneList from "./AirplaneList";
import { useToast } from "@/components/ToastProvider";

export default function AirplanesPage() {
  const [editing, setEditing] = useState<any>(null);
  const { success } = useToast();

  return (
    <>
      <h1 className="text-4xl font-extrabold mb-8 text-primary">
        ✈️ Наші літаки
      </h1>

      <AirplaneForm
        key={editing?.id || "create"}
        airplaneToEdit={editing}
        onCancel={() => setEditing(null)}
        onSuccess={() => {
          success(editing ? "Літак оновлено" : "Літак створено");
          setEditing(null);
        }}
      />

      <div className="my-4" />

      <AirplaneList onEdit={setEditing} />
    </>
  );
}