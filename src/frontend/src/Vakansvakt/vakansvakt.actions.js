import { vakansvaktRequestsActions } from '../Api/VakansvaktRequests';
import { alertActions } from '../_actions';
import sykehusApi from '../Api/Sykehus';
import minTjenesteplanApi from '../Api/MinTjenesteplan';
import tjenesteplanerApi from '../Api/Tjenesteplaner';

export function initialize(tjenesteplanId) {
    return (dispatch) => {
        return Promise.all([
            dispatch(vakansvaktRequestsActions.getUnapprovedVakansvaktRequests(tjenesteplanId)),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
    }
}

export function initializeAvailableVakansvakt(vakansvaktRequestId) {
    return (dispatch, getState) => {
        const getSykehusPromise = dispatch(sykehusApi.actions.getSykehus());
        return dispatch(vakansvaktRequestsActions.getVakansvaktRequest(vakansvaktRequestId))
            .then(() => {
                const state = getState();
                if(!state.vakansvaktRequests.isError) {
                    const tjenesteplanId = state.vakansvaktRequests.request.tjenesteplanId;
                    return Promise.all([
                        dispatch(tjenesteplanerApi.actions.getTjenesteplan(tjenesteplanId)),
                        getSykehusPromise
                    ]);
                }
            });
    }
}

export function initializeAvailableVakansvakter(tjenesteplanId) {
    return (dispatch) => {
        dispatch(vakansvaktRequestsActions.getAvailableVakansvaktRequests(tjenesteplanId));

        return Promise.all([
            dispatch(minTjenesteplanApi.actions.getMinTjenesteplan(tjenesteplanId)),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
    }
}

export function initializeAcceptVakansvakt(vakansvaktRequestId) {
    return (dispatch) => {
        return Promise.all([
            dispatch(vakansvaktRequestsActions.getVakansvaktRequest(vakansvaktRequestId)),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
    }
}

export function approveVakansvaktRequest(id) {
    return vakansvaktRequestsActions.approveVakansvaktRequest(id);
}

export function rejectVakansvaktRequest(id) {
    return vakansvaktRequestsActions.rejectVakansvaktRequest(id);
}

export function acceptVakansvakt(id, date) {
    return (dispatch) => {
         return dispatch(vakansvaktRequestsActions.acceptVakansvakt(id))
            .then(() => {
                return dispatch(alertActions.success('Vakansvakt ' + date.format("dddd, Do MMMM") + ' er akseptert'));
            });
    }
}


