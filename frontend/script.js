function toggleMenu() {
  const menu = document.querySelector("nav.menu");

  if (menu) {
    menu.classList.toggle("active");
  }
}

const API_URL = "http://localhost:5194/api/Products";

window.addEventListener("DOMContentLoaded", () => {
  loadProducts();
});

async function loadProducts() {
  const productList = document.getElementById("product-list");
  const statusText = document.getElementById("products-status");

  productList.innerHTML = "";
  statusText.textContent = "Indlæser bøger...";

  try {
    const response = await fetch(API_URL);

    if (!response.ok) {
      throw new Error(`API returned status ${response.status}`);
    }

    const products = await response.json();

    if (products.length === 0) {
      statusText.textContent = "Der blev ikke fundet nogen bøger.";
      return;
    }

    statusText.textContent = `${products.length} bøger hentet fra API.`;

    products.forEach((product) => {
      const productCard = createProductCard(product);
      productList.appendChild(productCard);
    });
  } catch (error) {
    console.error("Could not load products:", error);
    statusText.textContent =
      "Kunne ikke hente bøger fra API. Tjek at backend kører på http://localhost:5194.";
  }
}

function createProductCard(product) {
  const article = document.createElement("article");
  article.className = "book-card";

  const title = document.createElement("h3");
  title.textContent = product.name;

  const description = document.createElement("p");
  description.textContent = product.description;

  const meta = document.createElement("p");
  meta.className = "book-meta";
  meta.textContent = `${product.categoryName} • ${product.statusName}`;

  const price = document.createElement("span");
  price.textContent = `${formatPrice(product.price)} kr.`;

  const stock = document.createElement("p");
  stock.className = "book-stock";
  stock.textContent = `På lager: ${product.quantity}`;

  article.appendChild(title);
  article.appendChild(description);
  article.appendChild(meta);
  article.appendChild(price);
  article.appendChild(stock);

  return article;
}

function formatPrice(price) {
  return Number(price).toLocaleString("da-DK", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
}