
import avdelingerApi from '../Api/Avdelinger';

const initialState = {
    isFetching: false,
    isFailed: false,
    avdelinger: []
}

export function avdelinger(state = initialState, action) {

    switch (action.type) {
        case avdelingerApi.constants.START_GET_AVDELINGER:
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case avdelingerApi.constants.GET_AVDELINGER_SUCCEEDED:
            return {
                ...state,
                avdelinger: action.payload,
                isFetching: false
            };
        case avdelingerApi.constants.GET_AVDELINGER_FAILED:
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };

        default:
            return state
    }
}