import tjenesteplan from '../Api/Tjenesteplaner';

const initialState = {
    isFetching: false,
    isFailed: false,
    tjenesteplaner: []
}

export function tjenesteplanInfo(state = initialState, action) {
    let tjenesteplaner;

    switch (action.type) {
        case tjenesteplan.constants.START_GET_TJENESTEPLAN:
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case tjenesteplan.constants.GET_TJENESTEPLAN_SUCCEEDED:
            const tjenesteplanInfo = action.payload;

            tjenesteplaner = [...state.tjenesteplaner];
            tjenesteplaner[tjenesteplanInfo.id] = tjenesteplanInfo;

            return {
                ...state,
                tjenesteplaner: tjenesteplaner,
                isFetching: false
            };
        case tjenesteplan.constants.GET_TJENESTEPLAN_FAILED:
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };
        default:
            return state
    }
}