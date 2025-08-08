import * as validateSession from "./validate-user-login.js";


const addFundsModal = document.getElementById("addFundsModal");
const cancelAddFunds = document.getElementById("cancelAddFunds");

document.addEventListener("DOMContentLoaded", async () => {
    await validateSession.validateUserSession();

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

    const dataResponse = response.json();

    e.target.reset();
    
    debugger;
});



document.getElementById("addFundsBtn").addEventListener("click", () => {
    addFundsModal.classList.remove("hidden");
});

cancelAddFunds.addEventListener("click", () => {
    addFundsModal.classList.add("hidden");
});


