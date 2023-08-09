import anime from "animejs";

export default class MobileNavigation {
    private menuState = 0;

    constructor() {
        document.querySelector(".nav-menu__menu-button")!.addEventListener("click", this.switchMenu.bind(this));
    }

    switchMenu() {
        const linksWrapper = document.querySelector(".nav-menu__links-wrapper") as HTMLDivElement;
        if (linksWrapper.hasAttribute("open")) {
            linksWrapper.removeAttribute("open");

            linksWrapper.removeEventListener("click", this.outerClickLambda);

            document.body.classList.remove("no-scroll");

            this.menuState = 0;

            anime({
                targets: linksWrapper,
                easing: "easeOutQuart",
                duration: 300,
                opacity: 0,
                complete: () => {
                    if (this.menuState !== 0)
                        return;

                    linksWrapper.style.removeProperty("opacity");
                    linksWrapper.style.removeProperty("visibility");
                }
            });
        } else {
            linksWrapper.setAttribute("open", "");

            linksWrapper.style.visibility = "visible";
            document.body.classList.add("no-scroll");

            linksWrapper.addEventListener("click", this.outerClickLambda);

            this.menuState = 1;

            anime({
                targets: linksWrapper,
                easing: "easeOutQuart",
                duration: 300,
                opacity: 1,
                complete: () => {
                    if (this.menuState !== 1)
                        return;

                    linksWrapper.style.visibility = "visible";
                }
            });

            anime({
                targets: ".nav-menu__links-wrapper a",
                easing: "easeOutQuart",
                duration: 300,
                translateX: ["-1rem", "0"],
                opacity: [0, 1],
                delay: anime.stagger(50)
            });
        }
    }

    private outerClickLambda = (e) => this.switchMenuOnOuterClick(e);

    private switchMenuOnOuterClick(e: Event) {
        this.switchMenu();
    }
}