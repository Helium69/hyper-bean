import * as validateSession from "./validate-user-login.js";
import * as toastService from "./../toast.js";


const addFundsModal = document.getElementById("addFundsModal");
const cancelAddFunds = document.getElementById("cancelAddFunds");

document.addEventListener("DOMContentLoaded", async () => {
    await validateSession.validateUserSession();

    // user data

    const infoResponse = await fetch ("/user/get-user-account", {
        method : "GET",
        credentials : "include"
    });

    const userInfo = await infoResponse.json();

    document.getElementById("name").innerText = userInfo.data.name;
    document.getElementById("sex").innerText = userInfo.data.sex;
    document.getElementById("birth-date").innerText = userInfo.data.birthDate;
    document.getElementById("username").innerText = userInfo.data.username;
    document.getElementById("total-balance").innerText = userInfo.data.userBalance;

    document.getElementById("user-profile").src = userInfo.data.url;

    // transaction

    
    const transactionResponse = await fetch ("/user/get-transaction", {
        method : "GET",
        credentials : "include"
    });

    const transactionData = await transactionResponse.json();

    if (transactionResponse.status === 200){
        const container = document.getElementById("transaction-container");

        transactionData.data.forEach(transaction => {
            container.innerHTML += `
                <div class="bg-[#5A2A1C] p-4 rounded shadow-md">
                    <div class="flex justify-between text-sm sm:text-base">
                    <p><span class="font-semibold">Amount:</span> ₱<span class="text-red-700">${transaction.totalFee}</span></p>
                    <p><span class="font-semibold">New Balance:</span> ₱${transaction.balanceAmountLeft}</p>
                    </div>
                    <p class="text-xs sm:text-sm text-gray-300 mt-1">${transaction.date}</p>
                </div>
            `;
        });
    }
});

document.getElementById("add-funds-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    const payment = parseFloat(document.getElementById("funds-amount").value);

    const response = await fetch ("/user/add-funds", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({payment : payment})
    });

    const dataResponse = await response.json();

    e.target.reset();

    if (response.status === 200){
        toastService.showToast("success", "User Fund has been successfully updated");
        document.getElementById("total-balance").innerText = dataResponse.data;
        return;
    }

    toastService.showToast("error", dataResponse.message);
});



document.getElementById("addFundsBtn").addEventListener("click", () => {
    addFundsModal.classList.remove("hidden");
});

cancelAddFunds.addEventListener("click", () => {
    addFundsModal.classList.add("hidden");
});


