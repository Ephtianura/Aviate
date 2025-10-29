import { redirect } from "next/navigation";

const API_URL = process.env.NEXT_PUBLIC_API_URL;

export async function apiFetch(endpoint: string, options: RequestInit = {}) {
  const body = options.body;

  // --- безопасно создаём headers ---
  let headers: Record<string, string> = {};

  if (options.headers) {
    if (options.headers instanceof Headers) {
      options.headers.forEach((value, key) => (headers[key] = value));
    } else if (Array.isArray(options.headers)) {
      options.headers.forEach(([key, value]) => (headers[key] = value));
    } else {
      headers = { ...options.headers };
    }
  }

  // если body есть и это не FormData, добавляем JSON Content-Type
  if (body && !(body instanceof FormData)) {
    headers["Content-Type"] = "application/json";
  }

  const res = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    headers,
    credentials: "include",
  });

  const text = await res.text();
  let data: any = null;
  try {
    data = text && text.startsWith("{") ? JSON.parse(text) : null;
  } catch (e) {
    console.error("JSON parse error:", e, text);
  }

  if (!res.ok) {
    // if (res.status === 401 && typeof window !== "undefined") {
    //   const currentPath = window.location.pathname;
    //   if (currentPath !== "/login" && currentPath !== "/register") {
    //     console.warn("⚠️ Сессия истекла. Перенаправляем на логин...");
    //     document.cookie = "";
    //     window.location.href = "/login";
    //   }
    // }

    const err: any = new Error(
      data?.error || data?.message || `HTTP error! status: ${res.status}`,
    );
    err.status = res.status;
    err.data = data || text;
    throw err;
  }

  return data;
}

// Логин
export async function login(data: { email: string; password: string }) {
  try {
    const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
      credentials: "include",
    });

    if (!res.ok) {
      const text = await res.text().catch(() => "");
      let message = "Помилка авторизації";

      try {
        const json = JSON.parse(text);
        message = json.error || json.message || message;
      } catch (_) {}

      return { error: message };
    }

    return await res.json();
  } catch {
    return {
      error: "Сервер недоступний. Перевірте підключення або спробуйте пізніше.",
    };
  }
}

// Регістрація
export async function register(data: {
  fullName: string;
  email: string;
  password: string;
}) {
  try {
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_API_URL}/auth/register`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include",
      },
    );

    const body = await res.json().catch(() => ({}));

    if (!res.ok) {
      return {
        error: true,
        status: res.status,
        message: body.title ?? "Помилка",
        validation: body.errors ?? null,
      };
    }

    return { error: false, data: body };
  } catch (err) {
    console.error(err);
    return { error: true, message: "Проблема з сервером" };
  }
}

export const logout = () => apiFetch("/auth/logout", { method: "POST" });

// Получение профиля
export const getUserMe = async () => {
  return apiFetch("/user/me");
};

interface UserProfile {
  fullName: string;
  email: string;
  password?: string;
  phone: string;
}

export const updateUserProfile = async (data: Partial<UserProfile>) => {
  const body: Record<string, string> = {};

  if (data.fullName && data.fullName.trim() !== "")
    body.fullName = data.fullName;
  if (data.email && data.email.trim() !== "") body.email = data.email;
  if (data.phone && data.phone.trim() !== "") body.phone = data.phone;
  if (data.password && data.password.trim() !== "")
    body.password = data.password;

  if (Object.keys(body).length === 0) return { message: "Нет изменений" };

  const res = await fetch(`${API_URL}/user/UpdateProfile`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
    credentials: "include",
  });

  // Если статус 204, возвращаем пустой объект
  if (res.status === 204) return {};

  const text = await res.text();

  // Если текст есть — пробуем распарсить JSON
  let dataResponse: any = null;
  if (text) {
    try {
      dataResponse = JSON.parse(text);
    } catch (err) {
      console.warn("Не удалось распарсить JSON:", text);
      dataResponse = text; // оставляем как текст
    }
  }

  if (!res.ok) {
    const errorMessage =
      dataResponse?.error ||
      dataResponse?.message ||
      dataResponse?.title ||
      `HTTP error! status: ${res.status}`;
    throw new Error(errorMessage);
  }

  return dataResponse;
};
