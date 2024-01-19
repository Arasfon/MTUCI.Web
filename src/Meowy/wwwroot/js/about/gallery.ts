import MobileNavigation from "./../navigation";
import Glide from "@glidejs/glide";

const mobileNavigation = new MobileNavigation();

new Glide(".glide",
    {
        type: "carousel",
        focusAt: "center",
        perView: 3,
        gap: 10,
        autoplay: 3000,
        hoverpause: true,
        breakpoints: {
            960: {
                perView: 1
            }
        }
    }).mount();