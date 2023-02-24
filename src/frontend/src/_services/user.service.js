import { apiService } from './';
import { authenticationService } from './authenticationService';

export const userService = {
    login,
    logout,
    isLoggedIn,
    register,
    getUser,
    getAll,
    getById,
    getCurrentUser,
    update,
    delete: _delete,
    resetPassword,
    saveNewPassword
};

function login(username, password) {
    return apiService.post('users/authenticate', JSON.stringify({ username, password }))
        .then(user => {
            // login successful if there's a jwt token in the response
            if (user && user.authToken) {
                authenticationService.login(user);
            }

            return user;
        });
}

function logout() {
    authenticationService.logout();
}

function isLoggedIn() {
    return authenticationService.isLoggedIn();
}

function getUser() {
    return authenticationService.getUser();
}

function getAll() {
    return apiService.get('users');
}

function getById(id) {
    return apiService.get('users/' + id);
}

function getCurrentUser() {
    return apiService.get('users/current');
}

function register(user) {
    if(user.invitationId) {
        return apiService.post('users/register/' + user.invitationId, JSON.stringify(user));
    }

    return apiService.post('users/register', JSON.stringify(user));
}

function update(user) {
    return apiService.put('users/' + user.id);
}

// prefixed function name with underscore because delete is a reserved word in javascript
function _delete(id) {
    return apiService.delete('users/' + id);
}

function resetPassword(email) {
    return apiService.post("users/current/resetPassword", JSON.stringify({email}));
}


function saveNewPassword(token, password) {
    return apiService.post("users/current/newPassword", JSON.stringify({token, password}));
}