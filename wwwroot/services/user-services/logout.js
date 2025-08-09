document.querySelector(".logout").addEventListener("click", async () => {
    const response = await fetch ("/user/logout", {
        method : "GET",
        credentials : "include"
    });

    const responseData = await response.json();

    window.location.reload();
})