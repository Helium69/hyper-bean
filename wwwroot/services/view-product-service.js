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

function renderCoffeeCards() {
    coffeeContainer.innerHTML = "";
    coffeeData.forEach(coffee => {
    const card = document.createElement("div");
    card.className = "bg-[#2b1008] rounded shadow-md p-3 flex flex-col";

    card.innerHTML = `
    <div class="flex h-32 gap-3">
        <!-- Full-Height Image -->
        <img src="${coffee.image}" alt="${coffee.name}" class="w-32 h-full object-cover rounded">

        <!-- Info and Buttons -->
        <div class="flex flex-col justify-between flex-1">
        <div>
            <h2 class="text-base font-semibold truncate">${coffee.name}</h2>
            <p class="text-sm">☕ Small: ₱${coffee.small}</p>
            <p class="text-sm">☕ Medium: ₱${coffee.medium}</p>
            <p class="text-sm mb-2">☕ Large: ₱${coffee.large}</p>
        </div>

        <!-- Buttons -->
        <div class="flex flex-wrap gap-1">
            <button class="bg-blue-600 hover:bg-blue-500 text-xs px-3 py-1 rounded">Update</button>
            <button class="bg-red-600 hover:bg-red-500 text-xs px-3 py-1 rounded">Delete</button>
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