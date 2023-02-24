import { tjenesteplanerConstants } from './constants';
import { apiService } from '../../_services';
import { history } from '../../_helpers';
import { alertActions } from '../../_actions';

export const tjenesteplanerActions = {
    getTjenesteplaner,
    saveTjenesteplan,
    getWeeklyTjenesteplan,
    getTjenesteplan,
    changeTjenesteplanDateForLege
};

export function getTjenesteplaner() {
    return (dispatch) => {
        dispatch({ type: tjenesteplanerConstants.START_GET_TJENESTEPLANER });

        apiService.get("tjenesteplaner")
            .then(function(response) {
                dispatch({ type: tjenesteplanerConstants.GET_TJENESTEPLANER_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: tjenesteplanerConstants.GET_TJENESTEPLANER_FAILED, error : error });
            });
    }
}

export function getTjenesteplan(id) {
    return (dispatch) => {
        dispatch({ type: tjenesteplanerConstants.START_GET_TJENESTEPLAN });

        return apiService.get("tjenesteplaner/" + id)
            .then(function(response) {
                dispatch({ type: tjenesteplanerConstants.GET_TJENESTEPLAN_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: tjenesteplanerConstants.GET_TJENESTEPLAN_FAILED, error : error });
                throw error;
            });
    }
}

export function getWeeklyTjenesteplan(id) {
    return (dispatch) => {
        dispatch({ type: tjenesteplanerConstants.START_GET_WEEKLYTJENESTEPLAN });

        return apiService.get("tjenesteplaner/" + id + "/weekly")
            .then(function(response) {
                dispatch({ type: tjenesteplanerConstants.GET_WEEKLYTJENESTEPLAN_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: tjenesteplanerConstants.GET_WEEKLYTJENESTEPLAN_FAILED, error : error });
            });
    }
}

export function changeTjenesteplanDateForLege(tjenesteplanId, legeId, moment, dagsplan) {
    return (dispatch) => {
        dispatch({ type: tjenesteplanerConstants.START_CHANGE_TJENESTEPLANLEGEDATE });

        const payload = {
            newDagsplan: dagsplan.id
        };

        return apiService.put(
                "tjenesteplaner/" + tjenesteplanId + "/leger/" + legeId + "/datoer/" + moment.format("YYYY-MM-DD"),
                JSON.stringify(payload)
            )
            .then(function(response) {
                dispatch({ type: tjenesteplanerConstants.CHANGE_TJENESTEPLANLEGEDATE_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: tjenesteplanerConstants.CHANGE_TJENESTEPLANLEGEDATE_FAILED, error : error });
            });
    }
}

export function saveTjenesteplan(avdelingId, id, name, startDate, numberOfLeger, weeks) {
    return (dispatch) => {
        const tjenesteplan = {
            avdelingId,
            id,
            name,
            startDate,
            numberOfLeger,
            weeks: weeks,
            leger: []
        };

        dispatch({ type: tjenesteplanerConstants.START_SAVE_TJENESTEPLAN, tjenesteplan });

        let request;
        if(!id) {
            request = apiService.post("tjenesteplaner", JSON.stringify(tjenesteplan));
        }else{
            tjenesteplan.id = id;
            request = apiService.put("tjenesteplaner", JSON.stringify(tjenesteplan));
        }

        return request.then(function(response) {
            dispatch({ type: tjenesteplanerConstants.SAVE_TJENESTEPLAN_SUCCEEDED, payload : response, tjenesteplan });
            dispatch(alertActions.success('Tjenesteplan lagret'));

            if(!id){
                if(history.location.pathname.endsWith('/')) {
                    history.push("../../tjenesteplaner/" + response.id);
                } else {
                    history.push("../tjenesteplaner/" + response.id);
                }
            }

        }).catch(function(error) {
            dispatch({ type: tjenesteplanerConstants.SAVE_TJENESTEPLAN_FAILED, error : error });
            dispatch(alertActions.error('Kunne ikke lagre tjenesteplan pga en feil'));
        });

    }
}