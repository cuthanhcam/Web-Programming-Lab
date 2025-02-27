document.querySelectorAll(".button").forEach(button => {
    button.addEventListener("click", () => {
        alert("Product has been added to cart!");
    });
});

document.querySelectorAll(".social-link").forEach(link => {
    link.addEventListener("mouseover", () => {
        link.style.transform = "scale(1.1)";
        link.style.transition = "0.3s";
    });
    link.addEventListener("mouseout", () => {
        link.style.transform = "scale(1)";
    });
});
