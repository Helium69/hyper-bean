import * as validateSession from "./validate-user-login.js";
import * as toastService from "./../toast.js";

let coffeeData = [];
let addonData = [];

// final selection
let selectedAddonsID = [];
let selectedCoffeeID = null;
let selectedSize = null; // string label 
let selectedSizePrice = null; // numeric price
let quantity = 1;


document.getElementById("submit-final-order-form").addEventListener("submit", (e) => {
    e.preventDefault();


})

// Handle addon checkbox changes
document.addEventListener("change", (e) => {
    if (e.target.classList.contains("select-addon")) {
        const id = parseInt(e.target.dataset.id);

        if (e.target.checked) {
            if (!selectedAddonsID.includes(id)) {
                selectedAddonsID.push(id);
            }
        } else {
            const index = selectedAddonsID.indexOf(id);
            if (index !== -1) {
                selectedAddonsID.splice(index, 1);
            }
        }

        updateAddonDisplay();
        updateTotalPrice();
    }

    // Size change
    if (e.target.name === "size") {
        selectedSize = e.target.dataset.sizeName; 
        selectedSizePrice = parseFloat(e.target.value);
        updateSelectedSizeDisplay();
        updateTotalPrice();
    }

    // Quantity change
    if (e.target.type === "number") {
        quantity = parseInt(e.target.value) || 1;
        updateTotalPrice();
    }
});


document.addEventListener("DOMContentLoaded", async () => {
    await validateSession.validateUserSession();

    const coffeeResponse = await fetch("/user/get-available-coffee", {
        method: "GET",
        credentials: "include"
    });

    const addonResponse = await fetch("/user/get-available-addon", {
        method: "GET",
        credentials: "include"
    });

    coffeeData = await coffeeResponse.json();
    addonData = await addonResponse.json();

    coffeeData.data.forEach(coffee => {
        document.querySelector(".coffee-selection-container").innerHTML += `
            <div class="bg-[#3A170C] rounded-lg shadow p-3 flex flex-col items-center text-center text-sm h-52 justify-between">
                <img src="${coffee.url}" alt="Coffee" class="w-20 h-20 object-cover rounded-full mb-2 shadow" />
                <h3 class="text-base font-bold mb-1">${coffee.name}</h3>
                <button data-id="${coffee.id}" class="submit-order bg-yellow-500 hover:bg-yellow-600 text-black text-sm px-3 py-1 rounded font-semibold transition">Order</button>
            </div>
        `;
    });

    addonData.data.forEach(addon => {
        document.querySelector(".addon-selection-container").innerHTML +=
            `<div class="flex items-center bg-[#4B1F0E] p-2 rounded shadow">
                <img src="${addon.url}" alt="Addon" class="w-12 h-12 rounded object-cover mr-3" />
                <div class="flex-1">
                    <p class="font-semibold">${addon.name}</p>
                    <p class="text-sm text-gray-300">₱${addon.price}</p>
                </div>
                <input data-id="${addon.id}" data-price="${addon.price}" type="checkbox" class="select-addon ml-2" /> 
            </div>`;
    });
});

// selects coffee
document.addEventListener("click", (e) => {
    if (e.target.classList.contains("submit-order")) {
        const coffeeId = parseInt(e.target.dataset.id);
        const selectedCoffee = coffeeData.data.find(coffee => coffee.id === coffeeId);

        selectedCoffeeID = coffeeId;

        document.getElementById("coffee-url").src = selectedCoffee.url;
        document.getElementById("coffee-name").innerText = selectedCoffee.name;

        const sizeRadios = document.querySelectorAll('input[name="size"]');
        sizeRadios[0].value = selectedCoffee.small;
        sizeRadios[0].dataset.sizeName = "Small";
        sizeRadios[1].value = selectedCoffee.medium;
        sizeRadios[1].dataset.sizeName = "Medium";
        sizeRadios[2].value = selectedCoffee.large;
        sizeRadios[2].dataset.sizeName = "Large";

        document.getElementById("coffee-small").innerText = selectedCoffee.small;
        document.getElementById("coffee-medium").innerText = selectedCoffee.medium;
        document.getElementById("coffee-large").innerText = selectedCoffee.large;

        selectedSize = null;
        selectedSizePrice = null;
        quantity = 1;
        document.querySelector('input[type="number"]').value = 1;

        selectedAddonsID = [];
        document.querySelectorAll(".select-addon").forEach(cb => cb.checked = false);

        updateAddonDisplay();
        updateSelectedSizeDisplay();
        updateTotalPrice();

        openModal();
    }
});

// removes coffee selection
document.addEventListener("click", (e) => {
    if (e.target.classList.contains("close-modal")) {
        e.preventDefault();
        selectedCoffeeID = null;
        closeModal();
    }
});

function updateAddonDisplay() {
    if (selectedAddonsID.length === 0) {
        document.getElementById("display-selected-addons").innerText = "None";
    } else {
        const names = addonData.data
            .filter(a => selectedAddonsID.includes(a.id))
            .map(a => a.name)
            .join(", ");
        document.getElementById("display-selected-addons").innerText = names;
    }
}

function updateSelectedSizeDisplay() {
    if (selectedSize && selectedSizePrice != null) {
        document.getElementById("display-selected-size").innerText =
            `${selectedSize} - ₱${selectedSizePrice}`;
    } else {
        document.getElementById("display-selected-size").innerText = "None";
    }
}

function updateTotalPrice() {
    if (!selectedCoffeeID || selectedSizePrice == null) {
        document.getElementById("display-price").innerText = "None";
        return;
    }

    const basePrice = selectedSizePrice;
    const addonsTotal = addonData.data
        .filter(a => selectedAddonsID.includes(a.id))
        .reduce((sum, a) => sum + parseFloat(a.price), 0);

    const total = (basePrice + addonsTotal) * quantity;
    document.getElementById("display-price").innerText = total.toFixed(2);
}

function openModal() {
    document.querySelectorAll('input[name="size"]').forEach(radio => {
        radio.required = true;
        radio.checked = false;
    });

    document.getElementById("modal-overlay").classList.remove("hidden");
}

function closeModal() {
    document.querySelectorAll('input[name="size"]').forEach(radio => {
        radio.required = false;
    });

    document.getElementById("modal-overlay").classList.add("hidden");
}
