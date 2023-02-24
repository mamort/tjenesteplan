import { authenticationService } from './authenticationService';
import { history } from '../_helpers';

export const apiService = {
    get,
    post,
    put,
    delete: _delete
};

function get(resource) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(getBaseUrl() + '/' + resource, requestOptions)
        .then(handleResponse, handleError);
}

function post(resource, payload) {
    const requestOptions = {
        method: 'POST',
        headers: { ...authHeader(), 'Content-Type': 'application/json' },
        body: payload
    };

    return fetch(getBaseUrl() + '/' + resource, requestOptions)
        .then(handleResponse, handleError);
}

function put(resource, payload) {
    const requestOptions = {
        method: 'PUT',
        headers: { ...authHeader(), 'Content-Type': 'application/json' },
        body: payload
    };

    return fetch(getBaseUrl() + '/' + resource, requestOptions)
        .then(handleResponse, handleError);
}

function _delete(resource) {
    const requestOptions = {
        method: 'DELETE',
        headers: authHeader()
    };

    return fetch(getBaseUrl() + '/' + resource, requestOptions)
        .then(handleResponse, handleError);
}

function getBaseUrl() {
    return process.env.NODE_ENV === 'production'
        ? process.env.REACT_APP_API_URL
        : 'http://localhost:5000/api'
}

function authHeader() {
    // return authorization header with jwt token
    let user = authenticationService.getUser();

    if (user && user.authToken) {
        return { 'Authorization': 'Bearer ' + user.authToken };
    } else {
        return {};
    }
}

function handleResponse(response) {
    return new Promise((resolve, reject) => {
        if (response.ok) {
            // return json if it was returned in the response
            var contentType = response.headers.get("content-type");
            if (contentType && contentType.includes("application/json")) {
                response.json().then(json => resolve(json));
            } else {
                resolve();
            }
        } else {

            // return error message from response body
            response.text().then(text => {
                reject({
                    msg: text || response.statusText,
                    httpStatusCode: response.status
                });
            });
        }
    });
}

function handleError(error) {
    return Promise.reject({
        msg: error && error.message,
        httpStatusCode: -1
    });
}