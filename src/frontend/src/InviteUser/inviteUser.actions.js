import { inviteUserConstants } from './inviteUser.constants';
import { apiService } from '../_services';

export function getInvitations() {
    return (dispatch) => {
        dispatch({ type: inviteUserConstants.START_GET_INVITATIONS });

        apiService.get("userinvitations")
            .then(function(response) {
                dispatch({ type: inviteUserConstants.GET_INVITATIONS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: inviteUserConstants.GET_INVITATIONS_FAILED, error : error });
            });
    }
}

export function registerInvitation(invitation) {
    return (dispatch) => {
        dispatch({ type: inviteUserConstants.START_REGISTER_INVITATION });

        return apiService.post("userinvitations", JSON.stringify(invitation))
            .then(function(response) {
                dispatch({ type: inviteUserConstants.REGISTER_INVITATION_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: inviteUserConstants.REGISTER_INVITATION_FAILED, error : error });
                throw error;
            });
    }
}