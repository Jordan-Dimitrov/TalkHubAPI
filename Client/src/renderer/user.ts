
function setupLogin() : void
{
    let login = document.getElementById('login');
    let username = document.getElementById('username');
    let password = document.getElementById('password');
    let loginButton = document.getElementById('login-button');

    loginButton?.addEventListener('click', () => 
    {
        // @ts-expect-error
        console.log(api.test);
    });
}

setupLogin();
