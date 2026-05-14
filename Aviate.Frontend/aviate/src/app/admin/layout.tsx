import { ReactNode } from "react";
import { cookies } from "next/headers";
import { redirect } from "next/navigation";
import AdminSidebar from "@/components/Bars/AdminSidebar";
import { apiFetch, getUserMe } from "@/lib/api";
const API_URL = process.env.NEXT_PUBLIC_API_URL;
export type UserRole = 0 | 1 | 2;

export interface MeResponse {
    id: string;
    fullName: string;
    email: string;
    phone: string | null;
    role: UserRole;
    registrationDate: string;
    updatedDate: string;
}
interface Props {
    children: ReactNode;
}

const ROLE = {
    USER: 0,
    EMPLOYEE: 1,
    ADMIN: 2,
};

export default async function AdminLayout({ children }: Props) {

    const API_URL =
        process.env.NODE_ENV === "development"
            ? "http://localhost:5004"
            : "http://aviate_api:5004";

    const cookieStore = await cookies();

    const cookieHeader = cookieStore
        .getAll()
        .map(c => `${c.name}=${c.value}`)
        .join("; ");

    const res = await fetch(`${API_URL}/api/user/me`, {
        method: "GET",
        headers: {
            Cookie: cookieHeader,
        },
        cache: "no-store",
    });

    if (!res.ok) {
        redirect("/");
    }

    const me: MeResponse = await res.json();

    if (me.role !== ROLE.ADMIN && me.role !== ROLE.EMPLOYEE) {
        redirect("/");
    }

    return (
        <div className="min-h-screen flex justify-center">
            <div className="w-full max-w-7xl p-10 mx-4 flex flex-col lg:flex-row gap-10 items-start">
                <AdminSidebar />
                <div className="w-full">{children}</div>
            </div>
        </div>
    );
}