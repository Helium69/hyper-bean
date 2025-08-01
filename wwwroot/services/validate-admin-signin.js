async function validateAdminSignin(){
    const response = await fetch ("/admin/authorize-signin", {
        method : "GET",
        credentials : "include"
    })

    const responseData = response.json();

    if(response.status !== 200){
        console.log(responseData.message);
        window.location.href = "signin.html";
    }
}


document.addEventListener("DOMContentLoaded", async () => {
    await validateAdminSignin();
    return;
});