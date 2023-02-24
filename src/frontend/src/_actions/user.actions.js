import { userConstants } from '../_constants';
import { userService } from '../_services';
import { alertActions } from './';
import { history } from '../_helpers';

export const userActions = {
    login,
    logout,
    register,
    getAll,
    getCurrentUser,
    delete: _delete,
    resetPassword,
    resetPasswordReset,
    saveNewPassword,
    resetNewPassword
};

function login(username, password, returnUrl) {
    return dispatch => {
        dispatch(request({ username }));

        userService.login(username, password)
            .then(
                user => {
                    dispatch(success(user));
                    if(returnUrl) {
                        history.push(returnUrl);
                    } else {
                        history.push('/');
                    }
                },
                error => {
                    dispatch(failure(error.msg));

                    if(error.httpStatusCode == 400) {
                        dispatch(alertActions.error("Feil brukernavn eller passord."));
                    }else {
                        dispatch(alertActions.error(error.msg));
                    }
                }
            );
    };

    function request(user) { return { type: userConstants.LOGIN_REQUEST, user } }
    function success(user) { return { type: userConstants.LOGIN_SUCCESS, user } }
    function failure(error) { return { type: userConstants.LOGIN_FAILURE, error } }
}

function logout() {
    userService.logout();
    return { type: userConstants.LOGOUT };
}

function register(user) {
    return dispatch => {
        dispatch(request(user));

        userService.register(user)
            .then(
                () => {
                    dispatch(success());
                    history.push('/login');
                    dispatch(alertActions.success('Registration successful'));
                },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                }
            );
    };

    function request(user) { return { type: userConstants.REGISTER_REQUEST, user } }
    function success(user) { return { type: userConstants.REGISTER_SUCCESS, user } }
    function failure(error) { return { type: userConstants.REGISTER_FAILURE, error } }
}

function getAll() {
    return dispatch => {
        dispatch(request());

        userService.getAll()
            .then(
                users => dispatch(success(users)),
                error => dispatch(failure(error))
            );
    };

    function request() { return { type: userConstants.GETALL_REQUEST } }
    function success(users) { return { type: userConstants.GETALL_SUCCESS, users } }
    function failure(error) { return { type: userConstants.GETALL_FAILURE, error } }
}

function getCurrentUser() {
    return dispatch => {
        dispatch(request());

        return userService.getCurrentUser()
            .then(
                user => dispatch(success(user)),
                error => dispatch(failure(error))
            );
    };

    function request() { return { type: userConstants.GETCURRENTUSER_REQUEST } }
    function success(user) { return { type: userConstants.GETCURRENTUSER_SUCCESS, user } }
    function failure(error) { return { type: userConstants.GETCURRENTUSER_FAILURE, error } }
}


// prefixed function name with underscore because delete is a reserved word in javascript
function _delete(id) {
    return dispatch => {
        dispatch(request(id));

        userService.delete(id)
            .then(
                () => {
                    dispatch(success(id));
                },
                error => {
                    dispatch(failure(id, error));
                }
            );
    };

    function request(id) { return { type: userConstants.DELETE_REQUEST, id } }
    function success(id) { return { type: userConstants.DELETE_SUCCESS, id } }
    function failure(id, error) { return { type: userConstants.DELETE_FAILURE, id, error } }
}

function resetPasswordReset()
{
    return { type: userConstants.RESETPASSWORD_RESET };
}

function resetPassword(email) {
    return dispatch => {
        dispatch(request(email));

        userService.resetPassword(email)
            .then(
                () => {
                    dispatch(success(email));
                },
                error => {
                    dispatch(failure(email, error));
                    dispatch(alertActions.error('Kunne ikke tilbakeestille passord pga en feil'));
                }
            );
    };

    function request(id) { return { type: userConstants.RESETPASSWORD_REQUEST, id } }
    function success(id) { return { type: userConstants.RESETPASSWORD_SUCCESS, id } }
    function failure(id, error) { return { type: userConstants.RESETPASSWORD_FAILURE, id, error } }
}

function saveNewPassword(token, password) {
    return dispatch => {
        dispatch(request());

        userService.saveNewPassword(token, password)
            .then(
                () => {
                    dispatch(success());
                },
                error => {
                    dispatch(failure(error));
                    dispatch(alertActions.error('Kunne ikke sette nytt passord pga en feil'));
                }
            );
    };

    function request() { return { type: userConstants.NEWPASSWORD_REQUEST } }
    function success() { return { type: userConstants.NEWPASSWORD_SUCCESS } }
    function failure(error) { return { type: userConstants.NEWPASSWORD_FAILURE, error } }
}

function resetNewPassword()
{
    return { type: userConstants.NEWPASSWORD_RESET };
}