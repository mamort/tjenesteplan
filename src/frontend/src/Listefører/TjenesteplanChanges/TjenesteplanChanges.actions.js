import { vakansvaktRequestsActions } from '../../Api/VakansvaktRequests';
import sykehusApi from '../../Api/Sykehus';
import { vaktChangeRequestsActions } from '../../Api/VaktChangeRequests';

export function initialize(tjenesteplanId) {
    return (dispatch) => {
        return Promise.all([
            dispatch(vaktChangeRequestsActions.getAllVaktChangeRequests(tjenesteplanId)),
            dispatch(vakansvaktRequestsActions.getApprovedVakansvaktRequests(tjenesteplanId)),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
    }
}

export function undoVakansvakt(tjenesteplanId, vakansvaktRequestId) {
    return (dispatch) => {
        return dispatch(vakansvaktRequestsActions.undoVakansvakt(vakansvaktRequestId));
    }
}

export function undoVaktChange(tjenesteplanId, vaktChangeRequestId) {
    return (dispatch) => {
        return dispatch(vaktChangeRequestsActions.undoVaktChange(tjenesteplanId, vaktChangeRequestId));
    }
}