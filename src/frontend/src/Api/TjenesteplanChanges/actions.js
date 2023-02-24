import { tjenesteplanChangesConstants } from './constants';
import { apiService } from '../../_services';

export const sykehusActions = {
    getTjenesteplanChanges
};

export function getTjenesteplanChanges(tjenesteplanId) {
    return (dispatch) => {
        dispatch({ type: tjenesteplanChangesConstants.START_GET_TJENESTEPLANCHANGES });

        return apiService.get("tjenesteplaner/" + tjenesteplanId + "/changes")
            .then(function(response) {
                return dispatch({ type: tjenesteplanChangesConstants.GET_TJENESTEPLANCHANGES_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: tjenesteplanChangesConstants.GET_TJENESTEPLANCHANGES_FAILED, error : error });
            });
    }
}
