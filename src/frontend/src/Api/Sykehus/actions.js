import { sykehusConstants } from './constants';
import { apiService } from '../../_services';
import { alertActions } from '../../_actions';

export const sykehusActions = {
    getSykehus,
    createSykehus,
    editSykehus,
    deleteSykehus
};

export function getSykehus() {
    return (dispatch) => {
        dispatch({ type: sykehusConstants.START_GET_SYKEHUS });

        return apiService.get("sykehus")
            .then(function(response) {
                return dispatch({ type: sykehusConstants.GET_SYKEHUS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: sykehusConstants.GET_SYKEHUS_FAILED, error : error });
            });
    }
}

export function createSykehus(title) {
    return (dispatch) => {
        dispatch({ type: sykehusConstants.START_CREATE_SYKEHUS });

        return apiService.post("sykehus", JSON.stringify({ name: title }))
            .then(function(response) {
                return dispatch({ type: sykehusConstants.CREATE_SYKEHUS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: sykehusConstants.CREATE_SYKEHUS_FAILED, error : error });
            });
    }
}

export function editSykehus(id, title) {
    return (dispatch) => {
        dispatch({ type: sykehusConstants.START_EDIT_SYKEHUS });

        return apiService.put("sykehus/"+id, JSON.stringify({ name: title }))
            .then(function(response) {
                return dispatch({ type: sykehusConstants.EDIT_SYKEHUS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: sykehusConstants.EDIT_SYKEHUS_FAILED, error : error });
            });
    }
}

export function deleteSykehus(id) {
    return (dispatch) => {
        dispatch({ type: sykehusConstants.START_DELETE_SYKEHUS });

        return apiService.delete("sykehus/"+id)
            .then(function(response) {
                return dispatch({ type: sykehusConstants.DELETE_SYKEHUS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: sykehusConstants.DELETE_SYKEHUS_FAILED, error : error });
            });
    }
}