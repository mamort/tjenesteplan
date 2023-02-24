
import notificationsApi from '../Api/Notifications';

const initialState = {
    isFetching: false,
    isFailed: false,
    notifications: [],
    notificationsReadProcessing:[]
}

export function notifications(state = initialState, action) {

    switch (action.type) {
        case notificationsApi.constants.START_GET_NOTIFICATIONS:
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case notificationsApi.constants.GET_NOTIFICATIONS_SUCCEEDED:
            return {
                ...state,
                notifications: action.payload,
                isFetching: false
            };
        case notificationsApi.constants.GET_NOTIFICATIONS_FAILED:
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };
        case notificationsApi.constants.START_SAVE_NOTIFICATIONREAD:
            const notification = state.notifications.find(n => n.id === action.id);
            state.notificationsReadProcessing[notification.id] = notification;
            return {
                ...state,
                notifications: state.notifications.filter(n => n.id !== action.id),
                isFetching: true,
                isFailed: false,
            };
        case notificationsApi.constants.SAVE_NOTIFICATIONREAD_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                isFailed: false
            };
        case notificationsApi.constants.SAVE_NOTIFICATIONREAD_FAILED:
            const failedNotification = state.notificationsReadProcessing[action.id];
            state.notifications.push(failedNotification);
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };

        default:
            return state
    }
}