import * as toastService from "./../toast.js";

document.getElementById("coffee-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const name = document.getElementById("coffee-name").value;
    const url = document.getElementById("coffee-url").value;
    const small = parseFloat(document.getElementById("coffee-small").value);
    const medium = parseFloat(document.getElementById("coffee-medium").value);
    const large = parseFloat(document.getElementById("coffee-large").value);
    const isAvailable = false;

    const response = await fetch ("/admin/insert-coffee", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            name : name,
            url : url,
            small : small,
            medium : medium,
            large : large,
            isAvailable : isAvailable
    })});

    const result = await response.json();

    if (response.status === 422){
        result.data.forEach(error => {
            toastService.showToast("error", error);
        });
        return;
    }

    if (response.status === 201){
        toastService.showToast("success", result.message);
        return;
    }

    toastService.showToast("error", result.message);
});

document.getElementById("addon-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const name = document.getElementById("addon-name").value;
    const url = document.getElementById("addon-url").value;
    const price = parseFloat(document.getElementById("addon-price").value);
    const isAvailable = false;

    const response = await fetch ("/admin/insert-addon", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            name : name,
            url : url,
            price : price,
            isAvailable : isAvailable
    })});

    const result = await response.json();

    if (response.status === 422){
        result.data.forEach(error => {
            toastService.showToast("error", error);
        });
        return;
    }

    if (response.status === 201){
        toastService.showToast("success", result.message);
        return;
    }

    toastService.showToast("error", result.message);
});














// coffee navigation
const showCoffeeBtn = document.getElementById('showCoffeeFormBtn');
const showAddonBtn = document.getElementById('showAddonFormBtn');

const coffeeForm = document.getElementById('coffee-form');
const addonForm = document.getElementById('addon-form');

showCoffeeBtn.addEventListener('click', () => {
    coffeeForm.classList.remove('hidden');
    addonForm.classList.add('hidden');
});

showAddonBtn.addEventListener('click', () => {
    addonForm.classList.remove('hidden');
    coffeeForm.classList.add('hidden');
});

