import * as toastService from "./../toast.js";

document.getElementById("user-signin-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("signin-username").value;
    const password = document.getElementById("signin-password").value;
    
    const response = await fetch("/user/validate-user-login", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            username : username,
            password : password
    })});

    const responseData = await response.json();
    
    
    if (response.status === 200){
        toastService.showToast("success", responseData.message);
        window.location.href = "home.html";
        return;
    }
    
    if (response.status === 422){
        responseData.data.forEach(error => {
            toastService.showToast("error", error);
        });
        return;
    }
    
    toastService.showToast("error", responseData.message);
});