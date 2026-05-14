import WhiteCard from "@/components/Cards/WhiteCard";

export default function NotFound() {
    return (
        <div className="w-5xl h-200 flex items-center justify-center mx-auto">
            <WhiteCard>
                <main className="flex flex-col items-center justify-center text-center ">
                    <h1 className="text-7xl text-primary font-bold mb-4">404</h1>
                    <p>Такої сторінки не інує</p>
                </main>
            </WhiteCard>
        </div>

    );
}
