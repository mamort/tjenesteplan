
import sykehusApi from '../Api/Sykehus';

const initialState = {
    isFetching: false,
    isFailed: false,
    sykehus: []
}

export function sykehus(state = initialState, action) {

    switch (action.type) {
        case sykehusApi.constants.START_GET_SYKEHUS:
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case sykehusApi.constants.GET_SYKEHUS_SUCCEEDED:
            return {
                ...state,
                sykehus: action.payload,
                isFetching: false
            };
        case sykehusApi.constants.GET_SYKEHUS_FAILED:
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };

        default:
            return state
    }
}