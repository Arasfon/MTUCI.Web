import MobileNavigation from "./navigation";

const mobileNavigation = new MobileNavigation();

const dialog = document.getElementById("menu-item-dialog") as any;
dialog.addEventListener("click", dialog.close);
dialog.children[0].addEventListener("click", (e) => e.stopPropagation());
(document.getElementById("dialog__close") as HTMLAnchorElement).addEventListener("click", (e) => {
    e.preventDefault();
    dialog.close();
});

const dialogParts = {
    name: document.getElementById("dialog__name") as HTMLHeadingElement,
    description: document.getElementById("dialog__description") as HTMLParagraphElement,
    additionalInfo: document.getElementById("dialog__additional-info") as HTMLParagraphElement,
    price: document.getElementById("dialog__price__value") as HTMLSpanElement,
    image: document.getElementById("dialog__image") as HTMLDivElement,
};

for (let link of document.querySelectorAll(".menu-data-item__name")) {
    link.addEventListener("click", showMore);
}

interface IMenuItem {
    fullName: string,
    description: string,
    additionalInfo?: string,
    price: number,
    image: string,
}

const menuItems: IMenuItem[] = [
    {
        fullName: "Котлеты из курицы с рисом",
        description: "Сочные куриные котлеты, приготовленные по оригинальному рецепту, подаются с ароматным рисом, который отлично сочетается с мясом.",
        price: 350,
        image: "/img/menu/0.webp"
    },
    {
        fullName: "Сосиски с пюре",
        description: "Классическое сочетание сочных свиных сосисок, подается с нежным картофельным пюре и аппетитным соусом. Идеальный выбор для тех, кто ценит простоту и вкус домашней кухни.",
        price: 300,
        image: "/img/menu/1.webp"
    },
    {
        fullName: "Макароны с сыром и мясным соусом",
        description: "Аппетитное сочетание ароматных макарон с нежным сыром и густым мясным соусом, приготовленным из свежих ингредиентов. Идеальный выбор для любителей пикантных и насыщенных вкусов.",
        price: 270,
        image: "/img/menu/2.webp"
    },
    {
        fullName: "Овощной ролл",
        description: "Вегетарианский вариант классического ролла, который состоит из свежих овощей, обернутых в мягкий лаваш или тонкие лепёшки.",
        price: 250,
        image: "/img/menu/3.webp"
    },
    {
        fullName: "Салат с баклажанами",
        description: "Лёгкое и освежающее блюдо, которое сочетает в себе сочные овощи и ароматные специи. Все ингредиенты соединены вместе в ароматном соусе на основе оливкового масла и лимонного сока.",
        price: 290,
        image: "/img/menu/4.webp"
    },
    {
        fullName: "Паста с овощами",
        description: "Блюдо, которое сочетает в себе нежные и ароматные овощи с аппетитной пастой. Подается с соусом на основе оливкового масла, чеснока и специй, который придает блюду неповторимый вкус и аромат.",
        price: 310,
        image: "/img/menu/5.webp"
    },
    {
        fullName: "Гуляш с картофельным пюре",
        description: "Сытное и насыщенное блюдо, которое состоит из сочного говяжьего мяса, тушенного в томатном соусе с луком и специями. Подаётся с нежным картофельным пюре, которое создает идеальное сочетание с ароматным гуляшом.",
        price: 370,
        image: "/img/menu/6.webp"
    },
    {
        fullName: "Запеченная рыба с овощами",
        description: "Это нежное и ароматное блюдо, которое состоит из свежей рыбы, запеченной с овощами в духовке. Все ингредиенты запекаются вместе в ароматном соусе на основе оливкового масла, чеснока и специй, которые придают блюду богатый и насыщенный вкус.",
        price: 400,
        image: "/img/menu/7.webp"
    },
    {
        fullName: "Яичница-глазунья",
        description: "Простое и вкусное блюдо, которое состоит из яичницы, приготовленной на сковороде, с жидким желтком на хлебе.",
        price: 230,
        image: "/img/menu/8.webp"
    },
    {
        fullName: "Шоколадный торт",
        description: "Настоящее удовольствие для любителей шоколада. Торт состоит из нескольких слоев нежного шоколадного бисквита, и нежного шоколадного крема.",
        price: 150,
        image: "/img/menu/9.webp"
    },
    {
        fullName: "Фруктовый салат",
        description: "Свежее и полезное блюдо, которое состоит из различных свежих фруктов, таких как киви, ананас, клубника, манго, виноград и другие, нарезанных на кусочки.",
        price: 170,
        image: "/img/menu/10.webp"
    },
    {
        fullName: "Чизкейк",
        description: "Американское десертное блюдо, которое состоит из нежного творожного сыра, смешанного с яйцами, сахаром и ванильным экстрактом, на основе песочного теста.",
        price: 160,
        image: "/img/menu/11.webp"
    },
    {
        fullName: "Мороженое (ваниль, шоколад, клубника)",
        description: "Классическое сладкое лакомство, которое состоит из молока, сливок, яиц и сахара, замороженного до консистенции мягкого крема.",
        price: 90,
        image: "/img/menu/12.webp"
    }
];

function showMore(e: Event) {
    e.preventDefault();
    const row = ((e.currentTarget as HTMLLinkElement).parentElement as HTMLElement).parentElement as HTMLTableRowElement;

    const item: IMenuItem = menuItems[+(row.dataset.id as string)];

    dialogParts.name.innerText = item.fullName;
    dialogParts.description.innerText = item.description;
    dialogParts.additionalInfo.innerText = item.additionalInfo ?? "";
    dialogParts.price.innerText = `${item.price}`;
    dialogParts.image.style.backgroundImage = `url(${item.image})`;

    dialog.showModal();
}