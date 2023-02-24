import { dagsplanerConstants } from '../Api/Dagsplaner/constants';

const initialState = {
    isFetching: false,
    alleDagsplaner: []
}

export function dagsplaner(state = initialState, action) {
    switch (action.type) {
        case dagsplanerConstants.START_FETCH_DAGSPLANER:
            return {
                ...state,
                isFetching: true
            };
        case dagsplanerConstants.FETCH_DAGSPLANER_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                alleDagsplaner: action.dagsplaner.map(k => {
                    return {
                        name: k.name,
                        id: k.id,
                        isRolling: k.isRolling,
                        isSystemDagsplan: k.isSystemDagsplan
                    }
                })
            };
        case dagsplanerConstants.FETCH_DAGSPLANER_FAILED:
            return {
                ...state,
                isFetching: false
            };

        default:
            return state
    }
}