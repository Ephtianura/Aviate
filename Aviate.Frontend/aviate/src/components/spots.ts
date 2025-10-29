export type Spot = {
    id: string;
    city: string;
    title: string;
    image: string;
    description: string;
};

export const SPOTS: Spot[] = [
    {
        id: "tbilisi-narikala",
        city: "Тбілісі",
        title: "Фортеця Нарікала",
        image: "/images/tbilisi-narikala.jpg",
        description:
            "Висока, красива та неприступна фортеця, пов’язана з історією міста.",
    },
    {
        id: "istanbul-blue-mosque",
        city: "Стамбул",
        title: "Мечеть Султанахмет",
        image: "/images/istanbul-blue-mosque.jpg",
        description:
            "Найфотогенічніша мечеть Стамбула з шістьма мінаретами.",
    },
    {
        id: "batumi-tower",
        city: "Батумі",
        title: "Алфавітна вежа",
        image: "/images/batumi-tower.jpg",
        description:
            "Незвичайна споруда у формі ДНК з літерами грузинського алфавіту.",
    },
    {
        id: "almaty-medeo",
        city: "Алмати",
        title: "Медео",
        image: "/images/almaty-medeo.jpg",
        description:
            "Високогірний каток та легендарне місце відпочинку.",
    },
    {
        id: "vilnius-gediminas",
        city: "Вільнюс",
        title: "Вежа Гедиміна",
        image: "/images/vilnius-gediminas-tower.jpg",
        description:
            "Середньовічна вежа з видом на старе місто.",
    },

    // --- УКРАЇНА ---
    {
        id: "kyiv-st-sophia",
        city: "Київ",
        title: "Софійський собор",
        image: "/images/kyiv-sophia.jpg",
        description: "Величний символ Київської Русі з унікальними мозаїками та фресками XI століття.",
    },
    {
        id: "lviv-opera",
        city: "Львів",
        title: "Оперний театр",
        image: "/images/lviv-opera.jpg",
        description: "Архітектурна перлина міста, відома своїм розкішним інтер'єром та фасадом.",
    },
    {
        id: "odesa-potemkin-stairs",
        city: "Одеса",
        title: "Потьомкінські сходи",
        image: "/images/odesa-stairs.jpg",
        description: "Знамениті сходи, що з'єднують центр міста з портом та Чорним морем.",
    },
    {
        id: "kharkiv-derzhprom",
        city: "Харків",
        title: "Держпром",
        image: "/images/kharkiv-derzhprom.jpg",
        description: "Перший радянський хмарочос, збудований у стилі конструктивізму на площі Свободи.",
    },
    {
        id: "dnipro-monastery-island",
        city: "Дніпро",
        title: "Монастирський острів",
        image: "/images/dnipro-island.jpg",
        description: "Мальовничий парк на річці Дніпро з білосніжною церквою та водоспадом.",
    },
    {
        id: "chernivtsi-university",
        city: "Чернівці",
        title: "Університет ім. Федьковича",
        image: "/images/chernivtsi-univ.jpg",
        description: "Колишня резиденція митрополитів, яку часто називають «українським Гоґвортсом».",
    },
    {
        id: "ivano-frankivsk-ratusha",
        city: "Івано-Франківськ",
        title: "Ратуша",
        image: "/images/frankivsk-ratusha.jpg",
        description: "Єдина в Україні світська споруда у стилі конструктивізму з оглядовим майданчиком.",
    },
    {
        id: "uzhhorod-castle",
        city: "Ужгород",
        title: "Ужгородський замок",
        image: "/images/uzhhorod-castle.jpg",
        description: "Древня фортеця на пагорбі, де переплелися історія та легенди Закарпаття.",
    },
    {
        id: "rivne-tunnel-of-love",
        city: "Рівне",
        title: "Тунель кохання",
        image: "/images/rivne-tunnel.jpg",
        description: "Природний феномен неподалік міста, де дерева утворили ідеальну зелену арку над колією.",
    },
    {
        id: "zaporizhzhia-khortytsia",
        city: "Запоріжжя",
        title: "Острів Хортиця",
        image: "/images/zapor-khortytsia.jpg",
        description: "Колиска козацтва та найбільший острів на Дніпрі з історичним комплексом «Запорозька Січ».",
    },

    // --- ЯПОНІЯ ---
    {
        id: "tokyo-shibuya-crossing",
        city: "Токіо",
        title: "Перехрестя Сібуя",
        image: "/images/tokyo-shibuya.jpg",
        description: "Найжвавіше пішохідне перехрестя у світі, символ неонового та енергійного Токіо.",
    },
    {
        id: "tokyo-skytree",
        city: "Токіо",
        title: "Tokyo Skytree",
        image: "/images/tokyo-skytree.jpg",
        description: "Найвища телевежа світу з футуристичним дизайном та панорамним видом на мегаполіс.",
    },
    {
        id: "osaka-castle",
        city: "Осака",
        title: "Замок Осака",
        image: "/images/osaka-castle.jpg",
        description: "Велична самурайська фортеця, оточена парком із сотнями дерев сакури.",
    },
    {
        id: "nagoya-tv-tower",
        city: "Нагоя",
        title: "Телевежа Нагої",
        image: "/images/nagoya-tower.jpg",
        description: "Найстаріша телевежа Японії, розташована в самому серці сучасного парку Хісая Одорі.",
    },
    {
        id: "fukuoka-canai-city",
        city: "Фукуока",
        title: "Canal City Hakata",
        image: "/images/fukuoka-canal.jpg",
        description: "«Місто всередині міста» — величезний торгово-розважальний комплекс з каналом.",
    },
    {
        id: "sapporo-odori-park",
        city: "Саппоро",
        title: "Парк Одорі",
        image: "/images/sapporo-odori.jpg",
        description: "Центральний парк міста, де взимку проходить знаменитий Сніговий фестиваль.",
    }

    // +15 любых дальше
];
export function getRandomSpots(count = 5): Spot[] {
    return [...SPOTS]
        .sort(() => Math.random() - 0.5)
        .slice(0, count);
}