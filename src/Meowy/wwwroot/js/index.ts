import flatpickr from "flatpickr";
import {Russian as flatpickrRu} from "flatpickr/dist/l10n/ru.js";

import MobileNavigation from "./navigation";

const mobileNavigation = new MobileNavigation();

// @ts-ignore
flatpickr("#datetime",
    {
        locale: flatpickrRu,
        allowInput: true,
        minDate: "today",
        enableTime: true,
        minTime: "10:00",
        maxTime: "19:30",
        minuteIncrement: 15,
        altInput: true,
        altFormat: "d.m.Y H:i",
        dateFormat: "Z"
    });

document.getElementById("bookForm")!.addEventListener("submit", async event => {
    event.preventDefault();

    const form = event.target as HTMLFormElement;

    const response = await fetch(form.action,
        {
            method: "post",
            body: new FormData(form),
            credentials: 'include'
        });

    if (response.status === 204)
        window.location.href = "/visit/booking-thanks";
    else if (response.status === 404) {
        const errorElement = document.getElementById("bookFormError") as HTMLElement;
        errorElement.classList.add("shown");
        errorElement.innerText = "На данное время все столики уже забронированы. Пожалуйста, выберите другое время.";
    } else {
        const errorElement = document.getElementById("bookFormError") as HTMLElement;
        errorElement.classList.add("shown");
        errorElement.innerText = "Что-то не так. Пожалуйста, проверьте правильность введённых данных.";
    }
});