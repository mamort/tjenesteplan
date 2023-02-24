import { avdelingerConstants } from './constants';
import { apiService } from '../../_services';
import { alertActions } from '../../_actions';

export const avdelingerActions = {
    getAvdelinger,
    updateAvdeling,
    deleteAvdeling
};

export function getAvdelinger(sykehusId) {
    return (dispatch) => {
        dispatch({ type: avdelingerConstants.START_GET_AVDELINGER });

        apiService.get("sykehus/" + sykehusId + "/avdelinger")
            .then(function(response) {
                dispatch({ type: avdelingerConstants.GET_AVDELINGER_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: avdelingerConstants.GET_AVDELINGER_FAILED, error : error });
                throw error;
            });
    }
}

export function createAvdeling(sykehusId, title) {
    return (dispatch) => {
        dispatch({ type: avdelingerConstants.START_CREATE_AVDELING });

        return apiService.post("sykehus/" + sykehusId + "/avdelinger", JSON.stringify({ name: title }))
            .then(function(response) {
                return dispatch({ type: avdelingerConstants.CREATE_AVDELING_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: avdelingerConstants.CREATE_AVDELING_FAILED, error : error });
                throw error;
            });
    }
}

export function updateAvdeling(id, title, listeforerId) {
    return (dispatch) => {
        dispatch({ type: avdelingerConstants.START_EDIT_AVDELING });

        return apiService.put("avdelinger/" + id, JSON.stringify({ name: title, listeforerId: listeforerId }))
            .then(function(response) {
                return dispatch({ type: avdelingerConstants.EDIT_AVDELING_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: avdelingerConstants.EDIT_AVDELING_FAILED, error : error });
                throw error;
            });
    }
}

export function deleteAvdeling(id) {
    return (dispatch) => {
        dispatch({ type: avdelingerConstants.START_DELETE_AVDELING });

        return apiService.delete("avdelinger/" + id)
            .then(function(response) {
                return dispatch({ type: avdelingerConstants.DELETE_AVDELING_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: avdelingerConstants.DELETE_AVDELING_FAILED, error : error });
                throw error;
            });
    }
}