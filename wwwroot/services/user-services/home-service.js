import * as validateSession from "./validate-user-login.js";

document.addEventListener("DOMContentLoaded", async () => {
    await validateSession.validateUserSession();

    const infoResponse = await fetch ("/user/get-user-account", {
        method : "GET",
        credentials : "include"
    })

    

    const userInfo = await infoResponse.json();

    document.getElementById("name").innerText = userInfo.data.name;
    document.getElementById("sex").innerText = userInfo.data.sex;
    document.getElementById("birth-date").innerText = userInfo.data.birthDate;
    document.getElementById("username").innerText = userInfo.data.username;
    document.getElementById("total-balance").innerText = userInfo.data.userBalance;

    document.getElementById("user-profile").src = userInfo.data.url;
});

