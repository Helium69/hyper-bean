import * as validateSession from "./validate-user-login.js";

document.addEventListener("DOMContentLoaded", async () => {
    await validateSession.validateUserSession();
});