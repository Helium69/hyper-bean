let coffeeData = [];
let addonData = [];
const coffeeContainer = document.getElementById("coffee-container");
const addonContainer = document.getElementById("addon-container");

document.addEventListener("DOMContentLoaded", async () => {
    await loadData();
    renderCoffeeCards();
});

document.addEventListener("click", async (e) => {
    const product = e.target.closest(".product-container");

    if (e.target.classList.contains("delete")){

        if (product.classList.contains("coffee-container")){
            const deleteCoffeeName = product.dataset.name;
            
            const response = await fetch("/admin/delete-coffee", {
                method : "POST",
                credentials : "include",
                headers : {"Content-Type" : "application/json"},
                body : JSON.stringify({ name : deleteCoffeeName })
            })

            const result = await response.json();

            await loadData();
            renderCoffeeCards();

            debugger;
        }
        else if (product.classList.contains("addon-container")){
            const deleteAddonName = product.dataset.name;

            const response = await fetch("/admin/delete-addon", {
                method : "POST",
                credentials : "include",
                headers : {"Content-Type" : "application/json"},
                body : JSON.stringify({ name : deleteAddonName })
            })

            const result = await response.json();

            await loadData();
            renderAddonCards();

            debugger;
        }


    }
    else if (e.target.classList.contains("update")){

        if (product.classList.contains("coffee-container")){
            const updateCoffeeName = product.dataset.name;
            const updateCoffeeStatus = product.dataset.status;

            const convertedStatusValue = updateCoffeeStatus === "true";

            const response = await fetch ("/admin/update-coffee", {
                method : "POST",
                credentials : "include",
                headers : {"Content-Type" : "application/json"},
                body : JSON.stringify({
                    name : updateCoffeeName,
                    isAvailable : convertedStatusValue
            })});

            await loadData();
            renderCoffeeCards();

        }
        
        if (product.classList.contains("addon-container")){
            const updateAddonName = product.dataset.name;
            const updateAddonStatus = product.dataset.status;

            const convertedStatusValue = updateAddonStatus === "true";

            const response = await fetch ("/admin/update-addon", {
                method : "POST",
                credentials : "include",
                headers : {"Content-Type" : "application/json"},
                body : JSON.stringify({
                    name : updateAddonName,
                    isAvailable : convertedStatusValue
            })});

            await loadData();
            renderAddonCards();
        }
    }

});

async function loadData(){
    const coffeeResponse = await fetch ("/admin/get-coffee", { method : "GET", credentials : "include"});
    const addonResponse = await fetch ("/admin/get-addon", { method : "GET", credentials : "include"});

    const coffeeResult = await coffeeResponse.json();
    const addonResult = await addonResponse.json();

    if (coffeeResponse.status === 200){
        coffeeData = coffeeResult.data;
    }

    if (addonResponse.status === 200){
        addonData = addonResult.data;
    }
}



function renderCoffeeCards() {
    coffeeContainer.innerHTML = "";
    coffeeData.forEach(coffee => {
        const card = document.createElement("div");
        card.className = "bg-[#2b1008] rounded shadow-md p-3 flex flex-col";
        card.innerHTML = `
            <div data-name="${coffee.name}" data-status="${coffee.isAvailable ? "true" : "false"}" class="coffee-container product-container flex h-32 gap-3">
                <img src="${coffee.url}" alt="${coffee.name}" class="w-32 h-full object-cover rounded">
                <div class="flex flex-col justify-between flex-1">
                    <div>
                        <h2 class="text-base font-semibold truncate">${coffee.name}</h2>
                        <p class="text-sm">☕ Small: ₱${coffee.small}</p>
                        <p class="text-sm">☕ Medium: ₱${coffee.medium}</p>
                        <p class="text-sm mb-2">☕ Large: ₱${coffee.large}</p>
                    </div>
                    <div class="flex flex-wrap gap-1">
                        <button class="delete bg-red-600 hover:bg-red-500 text-xs px-3 py-1 rounded">Delete</button>
                        <button class="update ${coffee.isAvailable 
                            ? 'bg-green-600 hover:bg-green-500' 
                            : 'bg-gray-600 hover:bg-gray-500'} text-xs px-3 py-1 rounded">
                            ${coffee.isAvailable ? 'Available' : 'Unavailable'}
                        </button>
                    </div>
                </div>
            </div>
        `;
        coffeeContainer.appendChild(card);
    });
}

function renderAddonCards() {
    addonContainer.innerHTML = "";
    addonData.forEach(addon => {
        const card = document.createElement("div");
        card.className = "bg-[#2b1008] rounded shadow-md p-3 flex flex-col";
        card.innerHTML = `
            <div data-name="${addon.name}" data-status="${addon.isAvailable ? "true" : "false"}" class="addon-container product-container flex h-32 gap-3">
                <img src="${addon.url}" alt="${addon.name}" class="w-32 h-full object-cover rounded">
                <div class="flex flex-col justify-between flex-1">
                    <div>
                        <h2 class="text-base font-semibold truncate">${addon.name}</h2>
                        <p class="text-sm mb-2">💰 Price: ₱${addon.price}</p>
                    </div>
                    <div class="flex flex-wrap gap-1">
                        <button class="delete bg-red-600 hover:bg-red-500 text-xs px-3 py-1 rounded">Delete</button>
                        <button class="update ${addon.isAvailable 
                            ? 'bg-green-600 hover:bg-green-500' 
                            : 'bg-gray-600 hover:bg-gray-500'} text-xs px-3 py-1 rounded">
                            ${addon.isAvailable ? 'Available' : 'Unavailable'}
                        </button>
                    </div>
                </div>
            </div>
        `;
        addonContainer.appendChild(card);
    });
}


showCoffeesBtn.addEventListener("click", () => {
    coffeeContainer.classList.remove("hidden");
    addonContainer.classList.add("hidden");
    renderCoffeeCards(); 
});

showAddonsBtn.addEventListener("click", () => {
    coffeeContainer.classList.add("hidden");
    addonContainer.classList.remove("hidden");
    renderAddonCards(); 
});
