import { vaktChangeRequestsConstants } from '../Api/VaktChangeRequests/constants';

const initialState = {
    isFetching: false,
    isError: false,
    requests: [],
    allRequests: [],
    received: {
        isFetching: false,
        isError: false,
        requests: []
    },
    suggestions: {
        isSaving: false
    }
}

export function vaktChangeRequests(state = initialState, action) {
    switch (action.type) {
        case vaktChangeRequestsConstants.START_GET_ALLVAKTCHANGEREQUESTS:
            return {
                ...state,
                isFetching: true
            };
        case vaktChangeRequestsConstants.GET_ALLVAKTCHANGEREQUESTS_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                allRequests: action.payload.requests
            };
        case vaktChangeRequestsConstants.GET_ALLVAKTCHANGEREQUESTS_FAILED:
            return {
                ...state,
                isFetching: false,
                isError: true
            };
        case vaktChangeRequestsConstants.START_GET_VAKTCHANGEREQUESTS:
            return {
                ...state,
                isFetching: true
            };
        case vaktChangeRequestsConstants.GET_VAKTCHANGEREQUESTS_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                requests: action.payload.requests
            };
        case vaktChangeRequestsConstants.GET_VAKTCHANGEREQUESTS_FAILED:
            return {
                ...state,
                isFetching: false,
                isError: true
            };
        case vaktChangeRequestsConstants.START_GET_VAKTCHANGEREQUESTSRECEIVED:
            return {
                ...state,
                received: {
                    isFetching: true,
                    isError: false,
                    requests: []
                }
            };
        case vaktChangeRequestsConstants.GET_VAKTCHANGEREQUESTSRECEIVED_SUCCEEDED:
            return {
                ...state,
                received: {
                    isFetching: false,
                    isError: false,
                    requests: action.payload.requests
                }
            };
        case vaktChangeRequestsConstants.GET_VAKTCHANGEREQUESTSRECEIVED_FAILED:
            return {
                ...state,
                received: {
                    isFetching: false,
                    isError: true,
                    requests: []
                }
            };

        case vaktChangeRequestsConstants.START_SAVE_VAKTCHANGESUGGESTIONS:
            return {
                ...state,
                suggestions: {
                    isSaving: true
                }
            };
        case vaktChangeRequestsConstants.SAVE_VAKTCHANGESUGGESTIONS_SUCCEEDED:
            return {
                ...state,
                suggestions: {
                    isSaving: false
                }
            };
        case vaktChangeRequestsConstants.SAVE_VAKTCHANGESUGGESTIONS_FAILED:
            return {
                ...state,
                suggestions: {
                    isSaving: false
                }
            };


        default:
            return state
    }
}