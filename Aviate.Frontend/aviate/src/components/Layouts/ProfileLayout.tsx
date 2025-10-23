"use client";

import React from "react";
import SidebarProfile from "@/components/Bars/SidebarProfile";

interface ProfileLayoutProps {
    children: React.ReactNode;
}

export const ProfileLayout: React.FC<ProfileLayoutProps> = ({ children }) => {
    return (
        <div className="min-h-screen flex justify-center">
            <div className="w-full max-w-7xl p-10 mx-4  flex gap-10 items-start">
                <SidebarProfile>

                </SidebarProfile>
                <div className="w-full">
                    {children}

                </div>
            </div>
        </div>
    );
};
