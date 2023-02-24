import minTjenesteplan from '../Api/MinTjenesteplan';
import { alertActions } from '../_actions';
import { dagsplanerActions } from '../Api/Dagsplaner';
import { minTjenesteplanConstants } from './minTjenesteplan.constants.js';
import { apiService } from '../_services';
import sykehusApi from '../Api/Sykehus';
import { vaktChangeRequestsActions } from '../Api/VaktChangeRequests';

export function initialize(tjenesteplanId) {
    return (dispatch) => {
        dispatch(dagsplanerActions.getDagsplaner());
        dispatch(vaktChangeRequestsActions.getVaktChangeRequests(tjenesteplanId));

        return Promise.all([
            dispatch(minTjenesteplan.actions.getMinTjenesteplan(tjenesteplanId)),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
    }
}

export function saveVaktbytter(tjenesteplanId, dagsplan, dates) {
    return (dispatch) => {
        return dispatch(vaktChangeRequestsActions.saveVaktChangeRequests(tjenesteplanId, dagsplan, dates));
    }
}

export function undoVaktbytte(tjenesteplanId, vaktChangeRequestId) {
    return (dispatch) => {
        return dispatch(vaktChangeRequestsActions.undoVaktChange(tjenesteplanId, vaktChangeRequestId));
    }
}

export function sendVakansvaktRequest(tjenesteplanId, date, newDagsplanId, reason) {
    return (dispatch) => {

        dispatch({ type: minTjenesteplanConstants.START_SEND_VAKANSVAKTREQUEST });

        return apiService.post("vakansvaktRequests",
            JSON.stringify(
                {
                    tjenesteplanId: tjenesteplanId,
                    date: date,
                    dagsplan: newDagsplanId,
                    reason: reason
                })
            )
            .then(function(response) {
                dispatch({ type: minTjenesteplanConstants.SEND_VAKANSVAKTREQUEST_SUCCEEDED, payload : response });
                dispatch(alertActions.success('Forespørsel om vakansvakt registrert'));
            }).catch(function(error) {
                dispatch({ type: minTjenesteplanConstants.SEND_VAKANSVAKTREQUEST_FAILED, error : error });
                dispatch(alertActions.error('Beklager, fikk ikke registrert forespørsel pga en feil'));
            });
    }
}