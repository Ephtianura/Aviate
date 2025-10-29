import SearchCard from "@/components/SearchCard";
import SpotsGrid from "@/components/SpotsGrid";

export default function HomePage() {

  return (
    <main className="">
      <div
        className="absolute w-full z-0 bg-[url('/bg.png')] 
      h-[965px] bg-cover bg-center
        brightness-110">

      </div>

      <div className="flex justify-center items-end bg-primary h-[360px] z-10">
        <h1 className="text-white text-5xl font-extrabold text-center mb-10 z-10">
          Тут купують дешеві авіаквитки
        </h1>
      </div>

      <div className="top-16 z-20">
        <SearchCard />
      </div>
      <div className="h-[500px]" style={{
        backgroundImage: `
             radial-gradient(circle at 20% 30%, #fef08a 0%, transparent 50%),
             radial-gradient(circle at 80% 70%, #fef08a 0%, transparent 50%),
             radial-gradient(circle at 50% 10%, #fef08a 0%, transparent 40%)
           `,
        backgroundSize: '200% 200%',
      }}> </div>

      <div className="flex justify-center items-center -mt-[410px]">
        <SpotsGrid />

      </div>
 
    </main>
  );
}
