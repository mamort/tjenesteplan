
import tjenesteplanChangesApi from '../Api/TjenesteplanChanges';

const initialState = {
    isFetching: false,
    isFailed: false,
    changes: []
}

export function tjenesteplanChanges(state = initialState, action) {

    switch (action.type) {
        case tjenesteplanChangesApi.constants.START_GET_TJENESTEPLANCHANGES:
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case tjenesteplanChangesApi.constants.GET_TJENESTEPLANCHANGES_SUCCEEDED:
            return {
                ...state,
                changes: action.payload,
                isFetching: false
            };
        case tjenesteplanChangesApi.constants.GET_TJENESTEPLANCHANGES_FAILED:
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };

        default:
            return state
    }
}