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

    const data = response.json();
    
});