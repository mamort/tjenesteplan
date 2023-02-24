import { notificationsConstants } from './constants';
import { apiService } from '../../_services';
import { history } from '../../_helpers';
import { alertActions } from '../../_actions';

export const notificationsActions = {
    getNotifications,
    setNotificationRead
};

export function getNotifications() {
    return (dispatch) => {
        dispatch({ type: notificationsConstants.START_GET_NOTIFICATIONS });

        apiService.get("notifications")
            .then(function(response) {
                dispatch({ type: notificationsConstants.GET_NOTIFICATIONS_SUCCEEDED, payload : response });
            }).catch(function(error) {
                dispatch({ type: notificationsConstants.GET_NOTIFICATIONS_FAILED, error : error });
            });
    }
}

export function setNotificationRead(id) {
    return (dispatch) => {
        dispatch({ type: notificationsConstants.START_SAVE_NOTIFICATIONREAD, id: id });

        apiService.put("Notifications/"+id+"/read")
        .then(function(response) {
            dispatch({ type: notificationsConstants.SAVE_NOTIFICATIONREAD_SUCCEEDED, payload : response });
        }).catch(function(error) {
            dispatch({ type: notificationsConstants.SAVE_NOTIFICATIONREAD_FAILED, error : error, id });
            dispatch(alertActions.error('Kunne ikke lagre påminnelse pga en feil'));
        });

    }
}