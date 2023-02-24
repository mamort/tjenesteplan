import { tjenesteplanLegerConstants } from './tjenesteplanLeger.constants';
import { userConstants } from '../../_constants';
import { apiService } from '../../_services';
import tjenesteplaner from '../../Api/Tjenesteplaner';

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

export function registerLege(tjenesteplanId, registration) {
    return (dispatch) => {
        dispatch({ type: tjenesteplanLegerConstants.START_REGISTER_TJENESTEPLANLEGE });

        apiService.post("tjenesteplaner/" + tjenesteplanId + "/leger", JSON.stringify(registration))
            .then(function(response) {
                dispatch({ type: tjenesteplanLegerConstants.REGISTER_TJENESTEPLANLEGE_SUCCEEDED, payload : response });
                dispatch(getLeger());
                dispatch(tjenesteplaner.actions.getTjenesteplan(tjenesteplanId));
            }).catch(function(error) {
                dispatch({ type: tjenesteplanLegerConstants.REGISTER_TJENESTEPLANLEGE_FAILED, error : error });
            });
    }
}


export function removeLege(tjenesteplanId, legeId) {
    return (dispatch) => {
        dispatch({ type: tjenesteplanLegerConstants.START_REMOVE_TJENESTEPLANLEGE });

        apiService.delete("tjenesteplaner/" + tjenesteplanId + "/leger/" + legeId)
            .then(function(response) {
                dispatch({ type: tjenesteplanLegerConstants.REMOVE_TJENESTEPLANLEGE_SUCCEEDED, payload : response });
                dispatch(getLeger());
                dispatch(tjenesteplaner.actions.getTjenesteplan(tjenesteplanId));
            }).catch(function(error) {
                dispatch({ type: tjenesteplanLegerConstants.REMOVE_TJENESTEPLANLEGE_FAILED, error : error });
            });
    }
}