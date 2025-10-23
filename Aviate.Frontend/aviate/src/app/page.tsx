"use client";
import { useEffect, useState } from "react";
import { useAuth } from "@/context/AuthContext";
import SearchCard from "@/components/SearchCard";
import WhiteCard from "@/components/Cards/WhiteCard";
import HotFlights from "@/components/HotFlights";
import { SpotCard } from "@/components/Cards/SpotCard";



export default function HomePage() {
  const { isLoggedIn } = useAuth();


  return (
    <div className="">

      <main>
        <div className="flex  justify-center items-end bg-primary h-[360px]">
          <h1 className="text-white text-5xl font-extrabold text-center mb-10">
            Тут купують дешеві авіаквитки
          </h1>
        </div>


        <div className=" sticky top-16 z-10">
          <SearchCard />
        </div>
        <div className=" bg-primary h-[220px]"> </div>


        <div className="flex justify-center items-center -mt-[150px]">
          <div className="container px-90 grid grid-cols-2 gap-x-6 gap-y-12 justify-center items-center">
            <HotFlights />
            <SpotCard
              city="Тбілісі"
              title="Фортеця Нарікала"
              image="/images/tbilisi-narikala.jpg"
              description="Висока, красива та неприступна фортеця, пов’язана з історією міста. У спекотну погоду піднімайтеся канатною дорогою від парку Ріке та зустрічайте захід сонця на стінах."
            />

            <div className="col-span-2 ">
              <SpotCard
                city="Стамбул"
                title="Мечеть Султанахмет"
                image="/images/istanbul-blue-mosque.jpg"
                description="Найфотогенічніша мечеть Стамбула з шістьма мінаретами. Щоб зрозуміти, чому її називають Блакитною, потрібно зазирнути всередину."
              />
            </div>

            <SpotCard
              city="Батумі"
              title="Алфавітна вежа"
              image="/images/batumi-tower.jpg"
              description="Незвичайна споруда у формі ДНК, прикрашена літерами грузинського алфавіту. Зверху відкривається приголомшливий вид на море."
            />

            <SpotCard
              city="Алмати"
              title="Медео"
              image="/images/almaty-medeo.jpg"
              description="Високогірний каток та легендарне місце відпочинку. Прозоре повітря, гори та приголомшливі краєвиди навколо."
            />

            <div className="col-span-2">
              <SpotCard
                city="Вільнюс"
                title="Вежа Гедиміна"
                image="/images/vilnius-gediminas-tower.jpg"
                description="Середньовічна кам’яна вежа на пагорбі, звідки відкривається мальовничий вид на Старе місто Вільнюса та річки."
              />
            </div>
          </div>
        </div>


        {/* <div className="h-[3000px]"></div> */}

      </main>
    </div>
  );
}
