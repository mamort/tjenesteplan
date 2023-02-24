import { minTjenesteplanConstants } from './constants';
import { apiService } from '../../_services';
import { alertActions } from '../../_actions';

export const minTjenesteplanActions = {
    getMinTjenesteplan,
    getMineTjenesteplaner
};

export function getMinTjenesteplan(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: minTjenesteplanConstants.START_FETCH_MINTJENESTEPLAN });

        return apiService.get('minetjenesteplaner/' + tjenesteplanId)
            .then(tjenesteplan => dispatch({ type: minTjenesteplanConstants.FETCH_MINTJENESTEPLAN_SUCCEEDED, tjenesteplan }))
            .catch(error => {
                if(error.httpStatusCode === 404) {
                    dispatch({ type: minTjenesteplanConstants.FETCH_MINTJENESTEPLAN_NOTFOUND, error });
                } else{
                    dispatch({ type: minTjenesteplanConstants.FETCH_MINTJENESTEPLAN_FAILED, error });
                    dispatch(alertActions.error('Kunne ikke hente ut tjenesteplanen din pga av en feil.'));
                }
            });
    };
}

export function getMineTjenesteplaner() {
    return (dispatch) => {
        dispatch({ type: minTjenesteplanConstants.START_FETCH_MINETJENESTEPLANER });

        return apiService.get('minetjenesteplaner')
            .then(tjenesteplaner => dispatch({ type: minTjenesteplanConstants.FETCH_MINETJENESTEPLANER_SUCCEEDED, tjenesteplaner }))
            .catch(error => {
                dispatch({ type: minTjenesteplanConstants.FETCH_MINETJENESTEPLANER_FAILED, error });
                dispatch(alertActions.error('Kunne ikke hente ut tjenesteplanene dine pga av en feil.'));
            });
    };
}