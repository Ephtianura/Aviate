import { Suspense } from "react";
import FlightsClient from "./FlightsClient";

export default function Page() {
  return (
    <Suspense fallback={null}>
      <FlightsClient />
    </Suspense>
  );
}