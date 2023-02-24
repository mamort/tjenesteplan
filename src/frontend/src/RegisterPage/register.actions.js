import { registerConstants } from './register.constants';
import { apiService } from '../_services';

export function getInvitation(id) {
    return (dispatch) => {
        dispatch({ type: registerConstants.START_GET_INVITATION });

        return apiService.get("userinvitations/"+id)
            .then(function(response) {
                return dispatch({ type: registerConstants.GET_INVITATION_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: registerConstants.GET_INVITATION_FAILED, error : error });
            });
    }
}