document.addEventListener("click", async (e) => {
    if (e.target.classList.contains("logout")){
        const response = await fetch ("/admin/logout", {
            method : "GET",
            credentials : "include"
        });

        const responseData = await response.json();
        location.reload();
    }
});