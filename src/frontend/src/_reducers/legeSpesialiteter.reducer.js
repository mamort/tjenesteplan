import { legeSpesialiteterConstants } from '../Api/LegeSpesialiteter/constants';

const initialState = {
    isFetching: false,
    spesialiteter: []
}

export function legeSpesialiteter(state = initialState, action) {
    switch (action.type) {
        case legeSpesialiteterConstants.START_FETCH_LEGESPESIALITETER:
            return {
                ...state,
                isFetching: true
            };
        case legeSpesialiteterConstants.FETCH_LEGESPESIALITETER_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                spesialiteter: action.spesialiteter
            };
        case legeSpesialiteterConstants.FETCH_LEGESPESIALITETER_FAILED:
            return {
                ...state,
                isFetching: false,
                error: action.error
            };

        default:
            return state
    }
}