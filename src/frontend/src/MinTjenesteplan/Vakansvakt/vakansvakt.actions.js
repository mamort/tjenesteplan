import { vakansvaktRequestsActions } from '../../Api/VakansvaktRequests';
import sykehusApi from '../../Api/Sykehus';
import minTjenesteplanApi from '../../Api/MinTjenesteplan';

export function initialize(tjenesteplanId) {
    return (dispatch) => {
        dispatch(vakansvaktRequestsActions.getMyVakansvaktRequests(tjenesteplanId));

        return Promise.all([
            dispatch(minTjenesteplanApi.actions.getMinTjenesteplan(tjenesteplanId)),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
    }
}

export function initializeAcceptVakansvakt(vakansvaktRequestId) {
    return (dispatch) => {
        return dispatch(vakansvaktRequestsActions.getVakansvaktRequest(vakansvaktRequestId));
    }
}