import * as toastService from "./../toast.js";

document.getElementById("user-signup-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const name = document.getElementById("name").value;
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const sex = document.getElementById("sex").value;
    const birthDate = document.getElementById("birth-date").value;
    const url = document.getElementById("url").value;

    const response = await fetch("/user/insert-user", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            name : name,
            username : username,
            password : password,
            url : url,
            birthDate : birthDate,
            sex : sex,
            userBalance : 0,
            isActive : true
    })});

    const responseData = await response.json();


    if (response.status === 200 || response.status === 201){
        toastService.showToast("success", responseData.message);
        return;
    }

    if (response.status === 422){
        responseData.data.forEach(error => {
            toastService.showToast("error", error);
        });
        return;
    }

    toastService.showToast("error", responseData.message);

})