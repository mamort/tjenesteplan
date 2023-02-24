import { vakansvaktRequestsConstants } from '../Api/VakansvaktRequests/constants';

const initialState = {
    isFetching: false,
    isError: false,
    requests: [],
    availableRequests: [],
    approvedRequests: []
}

export function vakansvaktRequests(state = initialState, action) {
    switch (action.type) {
        case vakansvaktRequestsConstants.START_GET_VAKANSVAKTREQUESTS:
            return {
                ...state,
                isFetching: true
            };
        case vakansvaktRequestsConstants.GET_VAKANSVAKTREQUESTS_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                requests: action.payload
            };
        case vakansvaktRequestsConstants.GET_VAKANSVAKTREQUESTS_FAILED:
            return {
                ...state,
                isFetching: false,
                isError: true
            };
        case vakansvaktRequestsConstants.START_GET_APPROVEDVAKANSVAKTREQUESTS:
            return {
                ...state,
                isFetching: true
            };
        case vakansvaktRequestsConstants.GET_APPROVEDVAKANSVAKTREQUESTS_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                approvedRequests: action.payload
            };
        case vakansvaktRequestsConstants.GET_APPROVEDVAKANSVAKTREQUESTS_FAILED:
            return {
                ...state,
                isFetching: false,
                isError: true
            };
        case vakansvaktRequestsConstants.START_GET_AVAILABLEVAKANSVAKTREQUESTS:
            return {
                ...state,
                isFetching: true
            };
        case vakansvaktRequestsConstants.GET_AVAILABLEVAKANSVAKTREQUESTS_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                availableRequests: action.payload
            };
        case vakansvaktRequestsConstants.GET_AVAILABLEVAKANSVAKTREQUESTS_FAILED:
            return {
                ...state,
                isFetching: false,
                isError: true
            };
        case vakansvaktRequestsConstants.START_GET_VAKANSVAKTREQUEST:
            return {
                ...state,
                isFetching: true
            };
        case vakansvaktRequestsConstants.GET_VAKANSVAKTREQUEST_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                request: action.payload
            };
        case vakansvaktRequestsConstants.GET_VAKANSVAKTREQUEST_FAILED:
            return {
                ...state,
                isFetching: false,
                isError: true,
                error: action.error
            };

        default:
            return state
    }
}