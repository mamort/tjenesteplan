export const authenticationService = {
    login,
    logout,
    getUser,
    isLoggedIn,
    isSessionTokenExpired
};

function login(user) {
    // store user details and jwt token in local storage to keep user logged in between page refreshes
    localStorage.setItem('user', JSON.stringify(user));
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
}

function getUser() {
    return JSON.parse(localStorage.getItem('user'));
}

function isLoggedIn() {
    return localStorage.getItem('user')
        ? true
        : false;
}

function isSessionTokenExpired() {
    const user = getUser();
    if(user && user.authToken) {
        const jwt = parseJwt(user.authToken);
        if (Date.now() >= jwt.exp * 1000) {
            return true;
        }
    }

    return false;
}

// https://stackoverflow.com/questions/38552003/how-to-decode-jwt-token-in-javascript-without-using-a-library
function parseJwt (token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}