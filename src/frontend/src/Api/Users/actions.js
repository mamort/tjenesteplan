import legeConstants from './constants';
import { apiService } from '../../_services';
import { userConstants } from '../../_constants';

export const userActions = {
    getLeger,
    getAlleLeger,
    getLege
};

export function getLeger() {
    return (dispatch) => {
        dispatch({ type: userConstants.START_GET_LEGER });

        return apiService.get("users/leger")
            .then(function(response) {
                dispatch({ type: userConstants.GET_LEGER_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: userConstants.GET_LEGER_FAILED, error : error });
            });
    }
}

export function getAlleLeger() {
    return (dispatch) => {
        dispatch({ type: userConstants.START_GET_ALLELEGER });

        return apiService.get("users/alle-leger")
            .then(function(response) {
                dispatch({ type: userConstants.GET_ALLELEGER_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: userConstants.GET_ALLELEGER_FAILED, error : error });
            });
    }
}

export function getLege(id) {
    return (dispatch) => {
        dispatch({ type: userConstants.START_GET_LEGE });

        return apiService.get("users/" + id)
            .then(function(response) {
                dispatch({ type: userConstants.GET_LEGE_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: userConstants.GET_LEGE_FAILED, error : error });
            });
    }
}


export function removeFromAvdeling(avdelingId, legeId) {
    return (dispatch) => {
        dispatch({ type: legeConstants.START_REMOVEFROMAVDELING });

        return apiService.delete("users/avdelinger/" + avdelingId + "/leger/" + legeId)
            .then(function(response) {
                return dispatch({ type: legeConstants.REMOVEFROMAVDELING_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: legeConstants.REMOVEFROMAVDELING_FAILED, error : error });
            });
    }
}


export function addLegeToAvdeling(avdelingId, legeId) {
    return (dispatch) => {
        dispatch({ type: legeConstants.START_ADDLEGETOAVDELING });

        return apiService.post("users/avdelinger/" + avdelingId + "/leger", JSON.stringify({ userId: legeId }))
            .then(function(response) {
                return dispatch({ type: legeConstants.ADDLEGETOVDELING_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: legeConstants.AVDELING_FAILED, error : error });
            });
    }
}

