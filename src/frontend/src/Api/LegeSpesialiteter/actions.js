import { legeSpesialiteterConstants } from './constants';
import { apiService } from '../../_services';

export const legeSpesialiteterActions = {
    getLegeSpesialiteter
};

export function getLegeSpesialiteter() {
    return (dispatch) => {
        dispatch({ type: legeSpesialiteterConstants.START_FETCH_LEGESPESIALITETER });

        return apiService.get('lege-spesialiteter')
            .then(spesialiteter => dispatch({
                type: legeSpesialiteterConstants.FETCH_LEGESPESIALITETER_SUCCEEDED,
                spesialiteter
            }))
            .catch(error => dispatch({ type: legeSpesialiteterConstants.FETCH_LEGESPESIALITETER_FAILED, error }));
    };
}