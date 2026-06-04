import { apiFetch } from "@/lib/api";

export const getAirports = async (search?: string) => {
  const q = search ? `?Search=${encodeURIComponent(search)}&PageSize=100` : "?PageSize=100";
  const res = await apiFetch(`/admin/airports${q}`);
  return res.items ?? [];
};

export const getAirplanes = async (page = 1, pageSize = 12, search?: string) => {
  const q = [
    `Page=${page}`,
    `PageSize=${pageSize}`,
    search ? `Search=${encodeURIComponent(search)}` : null,
  ]
    .filter(Boolean)
    .join("&");

  const res = await apiFetch(`/admin/airplanes?${q}`);
  return res;
};

export const getFlights = async (page = 1, pageSize = 10) => {
  const res = await apiFetch(
    `/flights?Page=${page}&PageSize=${pageSize}&SortBy=ArrivalTime&SortDesc=true`
  );
  return res; 
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
