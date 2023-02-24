import { alertConstants } from '../_constants';

export const alertActions = {
    success,
    error,
    clear
};

function success(message) {
    return (dispatch) => {
        dispatchMessage(dispatch, { type: alertConstants.SUCCESS, message });
    }
}

function error(message) {
    return (dispatch) => {
        dispatchMessage(dispatch, { type: alertConstants.ERROR, message });
    }
}

function clear() {
    return { type: alertConstants.CLEAR };
}

function dispatchMessage(dispatch, msg) {
    dispatch(msg);
    setTimeout(function(){
        dispatch(clear());
    }, 4000);
}