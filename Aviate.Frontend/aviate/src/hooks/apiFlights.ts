import { apiFetch } from "@/lib/api";

export const getAirports = async (search?: string) => {
  const q = search ? `?Search=${encodeURIComponent(search)}&PageSize=100` : "?PageSize=100";
  const res = await apiFetch(`/admin/airports${q}`);
  return res.items ?? [];
};

export const getAirplanes = async (search?: string) => {
  const q = search ? `?Search=${encodeURIComponent(search)}&PageSize=100` : "?PageSize=100";
  const res = await apiFetch(`/admin/airplanes${q}`);
  return res.items ?? [];
};

export const getFlights = async () => {
  const res = await apiFetch(`/flights?PageSize=100`);
  return res.items ?? [];
};

export const createFlight = async (data: any) => {
  return apiFetch("/admin/flights", {
    method: "POST",
    body: JSON.stringify(data),
  });
};

export const updateFlight = async (id: string, data: any) => {
  return apiFetch(`/admin/flights/${id}`, {
    method: "PUT",
    body: JSON.stringify(data),
  });
};

export const deleteFlight = async (id: string) => {
  return apiFetch(`/admin/flights/${id}`, { method: "DELETE" });
};
