const showCoffeeBtn = document.getElementById('showCoffeeFormBtn');
const showAddonBtn = document.getElementById('showAddonFormBtn');
const coffeeForm = document.getElementById('coffeeForm');
const addonForm = document.getElementById('addonForm');

showCoffeeBtn.addEventListener('click', () => {
    coffeeForm.classList.remove('hidden');
    addonForm.classList.add('hidden');
});

showAddonBtn.addEventListener('click', () => {
    addonForm.classList.remove('hidden');
    coffeeForm.classList.add('hidden');
});