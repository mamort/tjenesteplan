import moment from 'moment';
import 'moment/locale/nb';
import minTjenesteplan from '../Api/MinTjenesteplan';

const initialState = {
    datoer: [],
    minetjenesteplaner: {
        isFetching: false,
        tjenesteplaner: []
    }
}

export function mintjenesteplan(state = initialState, action) {
    switch (action.type) {
        case minTjenesteplan.constants.START_FETCH_MINTJENESTEPLAN:
            return {
                ...state,
                isFetching: true
            };
        case minTjenesteplan.constants.FETCH_MINTJENESTEPLAN_SUCCEEDED:
            return {
                ...state,
                isFetching: false,
                isAssigned: true,
                sykehusId: action.tjenesteplan.sykehusId,
                avdelingId: action.tjenesteplan.avdelingId,
                name: action.tjenesteplan.name,
                datoer: action.tjenesteplan.dates.map(d => {
                    const dateMoment = moment.utc(d.date);
                    const existingDate = state.datoer.find(d2 => d2.date.isSame(dateMoment, 'day'));
                    if(existingDate &&
                        existingDate.dagsplanId == d.dagsplan &&
                        existingDate.isHoliday == d.isHoliday) {
                        return existingDate;
                    } else {
                        return {
                            date: dateMoment,
                            dagsplanId: d.dagsplan,
                            isHoliday: d.isHoliday,
                            description: d.description
                        }
                    }
                })
            };
        case minTjenesteplan.constants.FETCH_MINTJENESTEPLAN_NOTFOUND:
            return {
                ...state,
                isFetching: false,
                datoer: [],
                isAssigned: false
            };

        case minTjenesteplan.constants.START_FETCH_MINETJENESTEPLANER:
            return {
                ...state,
                minetjenesteplaner: {
                    isFetching: true,
                    tjenesteplaner: []
                }
            };
        case minTjenesteplan.constants.FETCH_MINETJENESTEPLANER_SUCCEEDED:
            return {
                ...state,
                minetjenesteplaner: {
                    isFetching: false,
                    tjenesteplaner: action.tjenesteplaner
                }
            };
        default:
            return state
    }
}