import { ReactNode } from "react";

interface WhiteCardProps {
  children: ReactNode;
}

export default function WhiteCard({
  children,
}: WhiteCardProps) {
  return (
    <div
      className="p-6 rounded-2xl shadow-md bg-white">
      {children}
    </div>
  );
}
