const coffeeData = [
    {
        id: 1,
        name: "Caramel Latte",
        small: 120,
        medium: 140,
        large: 160,
        image: "https://via.placeholder.com/300x200",
        available: true
    }
];

const coffeeContainer = document.getElementById("coffee-container");
const addonContainer = document.getElementById("addon-container");

function renderCoffeeCards() {
    coffeeContainer.innerHTML = "";
    coffeeData.forEach(coffee => {
        const card = document.createElement("div");
        card.className = "bg-[#2b1008] rounded shadow-md p-3 flex flex-col";

        card.innerHTML = `
        <div class="coffee-container flex h-32 gap-3">
            <img src="${coffee.image}" alt="${coffee.name}" class="w-32 h-full object-cover rounded">
            <div class="flex flex-col justify-between flex-1">
                <div>
                    <h2 class="text-base font-semibold truncate">${coffee.name}</h2>
                    <p class="text-sm">☕ Small: ₱${coffee.small}</p>
                    <p class="text-sm">☕ Medium: ₱${coffee.medium}</p>
                    <p class="text-sm mb-2">☕ Large: ₱${coffee.large}</p>
                </div>
                <div class="flex flex-wrap gap-1">
                    <button class="update bg-blue-600 hover:bg-blue-500 text-xs px-3 py-1 rounded">Update</button>
                    <button class="delete bg-red-600 hover:bg-red-500 text-xs px-3 py-1 rounded">Delete</button>
                    <button class="${coffee.available 
                        ? 'bg-green-600 hover:bg-green-500' 
                        : 'bg-gray-600 hover:bg-gray-500'} text-xs px-3 py-1 rounded">
                        ${coffee.available ? 'Available' : 'Unavailable'}
                    </button>
                </div>
            </div>
        </div>
        `;

        coffeeContainer.appendChild(card);
    });
}

renderCoffeeCards();

const showCoffeesBtn = document.getElementById("showCoffeesBtn");
const showAddonsBtn = document.getElementById("showAddonsBtn");

showCoffeesBtn.addEventListener("click", () => {
    coffeeContainer.classList.remove("hidden");
    addonContainer.classList.add("hidden");
});

showAddonsBtn.addEventListener("click", () => {
    coffeeContainer.classList.add("hidden");
    addonContainer.classList.remove("hidden");
    renderAddonCards();
});

function renderAddonCards() {
    addonContainer.innerHTML = "";

    const addons = [
        { name: "Extra Shot", price: 30, image: "https://via.placeholder.com/300x200", available: true },
        { name: "Whipped Cream", price: 20, image: "https://via.placeholder.com/300x200", available: false }
    ];

    addons.forEach(addon => {
        const card = document.createElement("div");
        card.className = "bg-[#2b1008] rounded shadow-md p-3 flex flex-col";

        card.innerHTML = `
        <div class="coffee-container flex h-32 gap-3">
            <img src="${addon.image}" alt="${addon.name}" class="w-32 h-full object-cover rounded">
            <div class="flex flex-col justify-between flex-1">
                <div>
                    <h2 class="text-base font-semibold truncate">${addon.name}</h2>
                    <p class="text-sm mb-2">💰 Price: ₱${addon.price}</p>
                </div>
                <div class="flex flex-wrap gap-1">
                    <button class="update bg-blue-600 hover:bg-blue-500 text-xs px-3 py-1 rounded">Update</button>
                    <button class="delete bg-red-600 hover:bg-red-500 text-xs px-3 py-1 rounded">Delete</button>
                    <button class="${addon.available 
                        ? 'bg-green-600 hover:bg-green-500' 
                        : 'bg-gray-600 hover:bg-gray-500'} text-xs px-3 py-1 rounded">
                        ${addon.available ? 'Available' : 'Unavailable'}
                    </button>
                </div>
            </div>
        </div>
        `;

        addonContainer.appendChild(card);
    });
}

// ----- Modal Management -----
const deleteModal = document.getElementById("deleteModal");
const updateModal = document.getElementById("updateModal");
const deleteAddonModal = document.getElementById("deleteAddonModal");
const updateAddonModal = document.getElementById("updateAddonModal");

const cancelDeleteBtn = document.getElementById("cancelDeleteBtn");
const cancelUpdateBtn = document.getElementById("cancelUpdateBtn");
const cancelDeleteAddonBtn = document.getElementById("cancelDeleteAddonBtn");
const cancelUpdateAddonBtn = document.getElementById("cancelUpdateAddonBtn");

function showModal(modal) {
    modal.classList.remove("hidden");
}
function hideModal(modal) {
    modal.classList.add("hidden");
}

// Cancel modal events
cancelDeleteBtn.addEventListener("click", () => hideModal(deleteModal));
cancelUpdateBtn.addEventListener("click", () => hideModal(updateModal));
cancelDeleteAddonBtn.addEventListener("click", () => hideModal(deleteAddonModal));
cancelUpdateAddonBtn.addEventListener("click", () => hideModal(updateAddonModal));

// Handle all clicks for update/delete buttons
document.addEventListener("click", (e) => {
    const isDelete = e.target.classList.contains("delete");
    const isUpdate = e.target.classList.contains("update");

    if (!isDelete && !isUpdate) return;

    const card = e.target.closest(".coffee-container");
    const isAddon = addonContainer.contains(card);

    if (isDelete) {
        showModal(isAddon ? deleteAddonModal : deleteModal);
    }

    if (isUpdate) {
        showModal(isAddon ? updateAddonModal : updateModal);
    }
});
