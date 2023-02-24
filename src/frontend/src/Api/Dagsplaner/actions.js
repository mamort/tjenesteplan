import { dagsplanerConstants } from './constants';
import { apiService } from '../../_services';

export const dagsplanerActions = {
    getDagsplaner
};

function getDagsplaner() {
    return (dispatch) => {
        dispatch({ type: dagsplanerConstants.START_FETCH_DAGSPLANER });

        return apiService.get('dagsplaner')
            .then(dagsplaner => dispatch({
                type: dagsplanerConstants.FETCH_DAGSPLANER_SUCCEEDED,
                dagsplaner
            }))
            .catch(error => dispatch({ type: dagsplanerConstants.FETCH_DAGSPLANER_FAILED, error }));
    };
}