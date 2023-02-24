import minTjenesteplan from '../Api/MinTjenesteplan';
import { dagsplanerActions } from '../Api/Dagsplaner';
import { vaktChangeRequestsActions } from '../Api/VaktChangeRequests';
import sykehusApi from '../Api/Sykehus';
import minTjenesteplanApi from '../Api/MinTjenesteplan';

export function initialize(tjenesteplanId) {
    return (dispatch) => {
        dispatch(dagsplanerActions.getDagsplaner());

        return Promise.all([
            dispatch(vaktChangeRequestsActions.getVaktChangeRequests(tjenesteplanId)),
            dispatch(vaktChangeRequestsActions.getVaktChangeRequestsReceived(tjenesteplanId)),
            dispatch(minTjenesteplanApi.actions.getMinTjenesteplan(tjenesteplanId)),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
    }
}

export function initializeVaktbytter(tjenesteplanId) {
    return (dispatch) => {
        dispatch(dagsplanerActions.getDagsplaner());
        dispatch(vaktChangeRequestsActions.getVaktChangeRequests(tjenesteplanId));
        dispatch(vaktChangeRequestsActions.getVaktChangeRequestsReceived(tjenesteplanId));

        return Promise.all([
            dispatch(minTjenesteplanApi.actions.getMinTjenesteplan(tjenesteplanId)),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
    }
}


export function initializeAcceptVaktbytter(tjenesteplanId) {
    return (dispatch) => {
        dispatch(dagsplanerActions.getDagsplaner());

        return Promise.all([
            dispatch(minTjenesteplanApi.actions.getMinTjenesteplan(tjenesteplanId)),
            dispatch(sykehusApi.actions.getSykehus()),
            dispatch(vaktChangeRequestsActions.getVaktChangeRequests(tjenesteplanId)),
            dispatch(vaktChangeRequestsActions.getVaktChangeRequestsReceived(tjenesteplanId))
        ]);
    }
}

export function saveVaktChangeSuggestions(id, dates) {
    return vaktChangeRequestsActions.saveVaktChangeSuggestions(id, dates);
}

export function acceptVaktbytte(tjenesteplanId, id, date) {
    return vaktChangeRequestsActions.acceptVaktbytte(tjenesteplanId, id, date);
}