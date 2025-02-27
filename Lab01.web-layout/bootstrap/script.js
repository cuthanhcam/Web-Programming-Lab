document.querySelectorAll(".button").forEach((button) => {
  button.addEventListener("click", () => {
    alert("Product has been added to cart!");
  });
});
document.addEventListener("DOMContentLoaded", () => {
  // Thêm sự kiện cho nút "Buy Now" trong product-details.html
  const buyNowButton = document.querySelector(".btn-success");
  if (buyNowButton) {
    buyNowButton.addEventListener("click", () => {
      alert("Product has been added to cart!");
    });
  }

  // Xử lý hiệu ứng hover trên icon mạng xã hội
  document.querySelectorAll(".social-link").forEach((link) => {
    link.addEventListener("mouseover", () => {
      link.style.transform = "scale(1.1)";
      link.style.transition = "0.3s";
    });
    link.addEventListener("mouseout", () => {
      link.style.transform = "scale(1)";
    });
  });

  // Xử lý click vào sản phẩm trong danh sách
  document.querySelectorAll(".product-card a").forEach((link) => {
    link.addEventListener("click", (event) => {
      event.preventDefault();
      window.location.href = link.getAttribute("href");
    });
  });

  // Toggle menu cho mobile
  const navToggle = document.querySelector(".menu-toggle");
  if (navToggle) {
    navToggle.addEventListener("click", () => {
      document.querySelector(".nav-links").classList.toggle("show");
    });
  }
});
