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
            body: new FormData(form)
        });

    if (response.ok)
        window.location.href = "/visit/booking-thanks";
    else {
        (document.getElementById("datetime") as HTMLInputElement).setCustomValidity("Проверьте правильность введённых данных.");
    }
});