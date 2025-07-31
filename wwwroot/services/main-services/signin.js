import * as toastService from "./../toast.js";

document.getElementById("admin-signin").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("admin-username").value;
    const password = document.getElementById("admin-password").value;

    const response = await fetch("/admin/validate-signin", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            username : username,
            password : password
        })})

    const responseData = await response.json();

    if (response.status === 422){
        responseData.data.forEach(error => {
            toastService.showToast("error", error);
        });
        return;
    }

    if (response.status === 200){
        toastService.showToast("success", responseData.message);
        window.location.href = "home.html";
        return;
    }

    toastService.showToast("error", responseData.message);
    
});