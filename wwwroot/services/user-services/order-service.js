import * as validateSession from "./validate-user-login.js";

document.addEventListener("DOMContentLoaded", async () => {
    await validateSession.validateUserSession();


    const coffeeResponse = await fetch ("/user/get-available-coffee", {
        method : "GET",
        credentials : "include"
    });

    const addonResponse = await fetch ("/user/get-available-addon", {
        method : "GET",
        credentials : "include"
    });


    const coffeeData = await coffeeResponse.json();
    const addonData = await addonResponse.json();

    debugger;
});