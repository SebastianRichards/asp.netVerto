

const hamburger = document.querySelector(".hamburger");
const navMenu = document.querySelector(".nav-menu");
const allContent = document.querySelector(".all-content");
const buttons = document.querySelectorAll("[data-carousel-button]");
const footer = document.querySelector(".foot");


hamburger.addEventListener("click", () => {
    hamburger.classList.toggle("active");
    navMenu.classList.toggle("active");
    allContent.classList.toggle("active");

})

document.querySelectorAll(".nav-link").forEach(n => n.addEventListener("click", () => {
    hamburger.classList.remove("active")
    navMenu.classList.remove("active")
    allContent.classList.remove("active")

}))

const slides = document.querySelector("[data-carousel] [data-slides]");
let currentIndex = 0;

function switchSlide(offset) {
    const activeSlide = slides.querySelector("[data-active]");
    activeSlide.removeAttribute("data-active");

    currentIndex += offset;
    if (currentIndex < 0) currentIndex = slides.children.length - 1;
    if (currentIndex >= slides.children.length) currentIndex = 0;

    slides.children[currentIndex].setAttribute("data-active", true);
}

function nextSlide() {
    switchSlide(1);
}

function prevSlide() {
    switchSlide(-1);
}

function startCarousel() {
    intervalId = setInterval(nextSlide, 5000);
}

function stopCarousel() {
    clearInterval(intervalId);
}
buttons.forEach((button) => {
    button.addEventListener("click", () => {
        const offset = button.dataset.carouselButton === "next" ? 1 : -1;
        switchSlide(offset);
    });
});

// Automatic Carousel Transition
if (window.location.pathname === "/" || window.location.pathname === "/home" || window.location.pathname === "/home/submitChanges") {
    startCarousel();
} 
// Stop automatic transition on button click
buttons.forEach((button) => {
    button.addEventListener("click", () => {
        clearInterval(intervalId);
    });
});




function adjustFooterPosition() {
    const contentHeight = document.querySelector('.pb-3').offsetHeight;
    const windowHeight = window.innerHeight;
    const footerHeight = document.querySelector('.foot').offsetHeight;

    if (contentHeight + footerHeight < windowHeight) {
        document.querySelector('.foot').classList.add('fixed-bottom');
    } else {
        document.querySelector('.foot').classList.remove('fixed-bottom');
    }
}

window.addEventListener('load', adjustFooterPosition);
window.addEventListener('resize', adjustFooterPosition);