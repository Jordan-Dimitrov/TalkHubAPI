"use strict";
function setupLogin() {
    let login = document.getElementById('login');
    let username = document.getElementById('username');
    let password = document.getElementById('password');
    let loginButton = document.getElementById('login-button');
    loginButton === null || loginButton === void 0 ? void 0 : loginButton.addEventListener('click', () => {
        // @ts-expect-error
        console.log(api.test);
    });
}
setupLogin();
