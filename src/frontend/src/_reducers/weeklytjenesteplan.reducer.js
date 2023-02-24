import tjenesteplan from '../Api/Tjenesteplaner';

const initialState = {
    isFetching: false,
    isFailed: false,
    tjenesteplaner: []
}

export function weeklyTjenesteplan(state = initialState, action) {
    let tjenesteplaner;

    switch (action.type) {
        case tjenesteplan.constants.START_GET_WEEKLYTJENESTEPLAN:
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case tjenesteplan.constants.GET_WEEKLYTJENESTEPLAN_SUCCEEDED:
            const weeklyTjenesteplan = action.payload;

            tjenesteplaner = [...state.tjenesteplaner];
            tjenesteplaner[weeklyTjenesteplan.tjenesteplanId] = weeklyTjenesteplan;

            return {
                ...state,
                tjenesteplaner: tjenesteplaner,
                isFetching: false
            };
        case tjenesteplan.constants.GET_WEEKLYTJENESTEPLAN_FAILED:
            return {
                ...state,
                isFetching: false,
                isFailed: true
            };
        default:
            return state
    }
}