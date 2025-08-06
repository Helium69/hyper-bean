export async function validateUserSession(){
    const response = await fetch ("/user/validate-user-session", {
        method : "GET",
        credentials : "include"
    });

    if (response.status !== 200){
        window.location.href = "signin.html";
    }
}