import { vakansvaktRequestsConstants } from './constants';
import { apiService } from '../../_services';
import { alertActions } from '../../_actions';
import moment from 'moment';
import 'moment/locale/nb';
import notificationsApi from '../../Api/Notifications';

export const vakansvaktRequestsActions = {
    getUnapprovedVakansvaktRequests,
    getApprovedVakansvaktRequests,
    getVakansvaktRequest,
    getMyVakansvaktRequests,
    getAvailableVakansvaktRequests,
    approveVakansvaktRequest,
    rejectVakansvaktRequest,
    acceptVakansvakt,
    undoVakansvakt
};

function getUnapprovedVakansvaktRequests(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: vakansvaktRequestsConstants.START_GET_VAKANSVAKTREQUESTS });

        return apiService.get("tjenesteplaner/" + tjenesteplanId + "/vakansvaktRequests/unapproved")
            .then(function(response) {
                return dispatch({ type: vakansvaktRequestsConstants.GET_VAKANSVAKTREQUESTS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vakansvaktRequestsConstants.GET_VAKANSVAKTREQUESTS_FAILED, error : error });
            });
    }
}


function getApprovedVakansvaktRequests(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: vakansvaktRequestsConstants.START_GET_APPROVEDVAKANSVAKTREQUESTS });

        return apiService.get("tjenesteplaner/" + tjenesteplanId + "/vakansvaktRequests/Approved")
            .then(function(response) {
                return dispatch({ type: vakansvaktRequestsConstants.GET_APPROVEDVAKANSVAKTREQUESTS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vakansvaktRequestsConstants.GET_APPROVEDVAKANSVAKTREQUESTS_FAILED, error : error });
            });
    }
}

function getAvailableVakansvaktRequests(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: vakansvaktRequestsConstants.START_GET_AVAILABLEVAKANSVAKTREQUESTS });

        apiService.get("tjenesteplaner/" + tjenesteplanId + "/vakansvaktRequests/available")
            .then(function(response) {
                dispatch({ type: vakansvaktRequestsConstants.GET_AVAILABLEVAKANSVAKTREQUESTS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vakansvaktRequestsConstants.GET_AVAILABLEVAKANSVAKTREQUESTS_FAILED, error : error });
            });
    }
}

function getMyVakansvaktRequests(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: vakansvaktRequestsConstants.START_GET_VAKANSVAKTREQUESTS });

        apiService.get("tjenesteplaner/" + tjenesteplanId + "/vakansvaktRequests")
            .then(function(response) {
                dispatch({ type: vakansvaktRequestsConstants.GET_VAKANSVAKTREQUESTS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vakansvaktRequestsConstants.GET_VAKANSVAKTREQUESTS_FAILED, error : error });
            });
    }
}

function getVakansvaktRequest(vakansvaktRequestId) {
    return (dispatch) => {
        dispatch({ type: vakansvaktRequestsConstants.START_GET_VAKANSVAKTREQUEST });

        return apiService.get("vakansvaktRequests/" + vakansvaktRequestId)
            .then(function(response) {
                return dispatch({ type: vakansvaktRequestsConstants.GET_VAKANSVAKTREQUEST_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: vakansvaktRequestsConstants.GET_VAKANSVAKTREQUEST_FAILED, error : error });
            });
    }
}

function approveVakansvaktRequest(id) {
    return (dispatch) => {
        dispatch({type: vakansvaktRequestsConstants.START_APPROVEVAKANSVAKTREQUEST});

        return apiService.post("VakansvaktRequests/" + id + "/Approve")
            .then(function(response) {
                dispatch({type : vakansvaktRequestsConstants.APPROVEVAKANSVAKTREQUEST_SUCCEEDED, payload : response});
                dispatch(getVakansvaktRequest(id));
            }).catch(function(error) {
                dispatch({type : vakansvaktRequestsConstants.APPROVEVAKANSVAKTREQUEST_FAILED, error : error});
                dispatch(alertActions.error('Kunne ikke godkjenne vakansvakt pga en feil'));
            });
    }
}

function rejectVakansvaktRequest(id) {
    return (dispatch) => {
        dispatch({type: vakansvaktRequestsConstants.START_REJECTVAKANSVAKTREQUEST});

        return apiService.post("VakansvaktRequests/" + id + "/Reject")
            .then(function(response) {
                dispatch({type : vakansvaktRequestsConstants.REJECTVAKANSVAKTREQUEST_SUCCEEDED, payload : response});
                dispatch(getVakansvaktRequest(id));
            }).catch(function(error) {
                dispatch({type : vakansvaktRequestsConstants.REJECTVAKANSVAKTREQUEST_FAILED, error : error});
                dispatch(alertActions.error('Kunne ikke avvise vakansvakt pga en feil'));
            });
    }
}

function acceptVakansvakt(id) {
    return (dispatch) => {
        return new Promise((resolve, reject) => {
            dispatch({type: vakansvaktRequestsConstants.START_ACCEPTVAKANSVAKTREQUEST});

            apiService.post("VakansvaktRequests/" + id + "/Accept")
                .then(function(response) {
                    dispatch({type : vakansvaktRequestsConstants.ACCEPTVAKANSVAKTREQUEST_SUCCEEDED, payload : response});
                    resolve();
                }).catch(function(error) {
                    dispatch(alertActions.error('Kunne ikke ta vakansvakt pga en feil'));
                    dispatch({type : vakansvaktRequestsConstants.ACCEPTVAKANSVAKTREQUEST_FAILED, error : error});
                    reject();
                });
        });
    }
}


function undoVakansvakt(id) {
    return (dispatch) => {
        return new Promise((resolve, reject) => {
            dispatch({type: vakansvaktRequestsConstants.START_UNDOVAKANSVAKTREQUEST});

            apiService.delete("VakansvaktRequests/" + id)
                .then(function(response) {
                    dispatch({type : vakansvaktRequestsConstants.UNDOVAKANSVAKTREQUEST_SUCCEEDED, payload : response});
                    resolve();
                }).catch(function(error) {
                    dispatch(alertActions.error('Kunne ikke rulle tilbake vakansvakt pga en feil'));
                    dispatch({type : vakansvaktRequestsConstants.UNDOVAKANSVAKTREQUEST_FAILED, error : error});
                    reject();
                });
        });
    }
}