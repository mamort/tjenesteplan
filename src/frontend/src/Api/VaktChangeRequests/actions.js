import { vaktChangeRequestsConstants } from './constants';
import { apiService } from '../../_services';
import notificationsApi from '../../Api/Notifications';

export const vaktChangeRequestsActions = {
    getAllVaktChangeRequests,
    getVaktChangeRequests,
    getVaktChangeRequestsReceived,
    saveVaktChangeRequests,
    saveVaktChangeSuggestions,
    acceptVaktbytte,
    undoVaktChange
};

function getAllVaktChangeRequests(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: vaktChangeRequestsConstants.START_GET_ALLVAKTCHANGEREQUESTS });

        return apiService.get("tjenesteplaner/" + tjenesteplanId + "/VaktChangeRequests")
            .then(function(response) {
                dispatch({ type: vaktChangeRequestsConstants.GET_ALLVAKTCHANGEREQUESTS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vaktChangeRequestsConstants.GET_ALLVAKTCHANGEREQUESTS_FAILED, error : error });
                throw error;
            });
    }
}

function getVaktChangeRequests(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: vaktChangeRequestsConstants.START_GET_VAKTCHANGEREQUESTS });

        return apiService.get("tjenesteplaner/" + tjenesteplanId + "/VaktChangeRequests/sent")
            .then(function(response) {
                dispatch({ type: vaktChangeRequestsConstants.GET_VAKTCHANGEREQUESTS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vaktChangeRequestsConstants.GET_VAKTCHANGEREQUESTS_FAILED, error : error });
                throw error;
            });
    }
}

function getVaktChangeRequestsReceived(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: vaktChangeRequestsConstants.START_GET_VAKTCHANGEREQUESTSRECEIVED });

        return apiService.get("tjenesteplaner/" + tjenesteplanId + "/VaktChangeRequests/Received")
            .then(function(response) {
                dispatch({ type: vaktChangeRequestsConstants.GET_VAKTCHANGEREQUESTSRECEIVED_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vaktChangeRequestsConstants.GET_VAKTCHANGEREQUESTSRECEIVED_FAILED, error : error });
                throw error;
            });
    }
}

export function saveVaktChangeRequests(tjenesteplanId, dagsplan, dates) {
    return (dispatch) => {

        dispatch({ type: vaktChangeRequestsConstants.START_SAVE_VAKTCHANGEREQUESTS });

        return apiService.post("vaktChangeRequests", JSON.stringify({ TjenesteplanId: tjenesteplanId, Dagsplan: dagsplan, Dates: dates }))
            .then(function(response) {
                dispatch({ type: vaktChangeRequestsConstants.SAVE_VAKTCHANGEREQUESTS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vaktChangeRequestsConstants.SAVE_VAKTCHANGEREQUESTS_FAILED, error : error });
            });
    }
}

function saveVaktChangeSuggestions(id, dates) {
    return (dispatch) => {
        dispatch({type: vaktChangeRequestsConstants.START_SAVE_VAKTCHANGESUGGESTIONS});

        const dateList = dates.map(d => d.date);

        return apiService.put("VaktChangeRequests/Received/" + id, JSON.stringify({Dates: dateList}))
            .then(function(response) {
                dispatch({type : vaktChangeRequestsConstants.SAVE_VAKTCHANGESUGGESTIONS_SUCCEEDED, payload : response});
            }).catch(function(error) {
                dispatch({type : vaktChangeRequestsConstants.SAVE_VAKTCHANGESUGGESTIONS_FAILED, error : error});
                throw error;
            });
    }
}

function acceptVaktbytte(tjenesteplanId, id, date) {
    return (dispatch) => {
        dispatch({type: vaktChangeRequestsConstants.START_ACCEPTVAKTCHANGE});

        return apiService.post("VaktChangeRequests/Accept", JSON.stringify({replyId: id, date: date}))
            .then(function(response) {
                dispatch({type : vaktChangeRequestsConstants.ACCEPTVAKTCHANGE_SUCCEEDED, payload : response});
                dispatch(getVaktChangeRequests(tjenesteplanId));
                dispatch(notificationsApi.actions.getNotifications());
            }).catch(function(error) {
                dispatch({type : vaktChangeRequestsConstants.ACCEPTVAKTCHANGE_FAILED, error : error});
                throw error;
            });
    }
}

function undoVaktChange(tjenesteplanId, id) {
    return (dispatch) => {
        dispatch({type: vaktChangeRequestsConstants.START_UNDOVAKTCHANGE});

        return apiService.delete("tjenesteplaner/" + tjenesteplanId + "/VaktChangeRequests/" + id)
            .then(function(response) {
                dispatch({type : vaktChangeRequestsConstants.UNDOVAKTCHANGE_SUCCEEDED, payload : response});
            }).catch(function(error) {
                dispatch({type : vaktChangeRequestsConstants.UNDOVAKTCHANGE_FAILED, error : error});
                throw error;
            });
    }
}