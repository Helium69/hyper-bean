import * as auth from "./../validate-admin-signin.js";

document.addEventListener("DOMContentLoaded", async () => {
    await auth.validateAdminSignin();
    return;
})