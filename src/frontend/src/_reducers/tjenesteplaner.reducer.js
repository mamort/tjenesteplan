
import tjenesteplan from '../Api/Tjenesteplaner';

const initialState = {
    isFetching: false,
    isFailed: false,
    tjenesteplaner: []
}

export function tjenesteplaner(state = initialState, action) {

    switch (action.type) {
        case tjenesteplan.constants.START_GET_TJENESTEPLANER:
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case tjenesteplan.constants.GET_TJENESTEPLANER_SUCCEEDED:
            return {
                ...state,
                tjenesteplaner: action.payload,
                isFetching: false
            };
        case tjenesteplan.constants.GET_TJENESTEPLANER_FAILED:
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };
        case tjenesteplan.constants.START_SAVE_TJENESTEPLAN:
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case tjenesteplan.constants.SAVE_TJENESTEPLAN_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                isFailed: false
            };
        case tjenesteplan.constants.SAVE_TJENESTEPLAN_FAILED:
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };

        default:
            return state
    }
}