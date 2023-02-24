import { tjenesteplanConstants } from './tjenesteplan.constants';
import { dagsplanerActions } from '../Api/Dagsplaner';

export function initialize() {
    return (dispatch) => {
        dispatch(dagsplanerActions.getDagsplaner());
    }
}

export function tjenesteplanConfigChanged(config){
    return (dispatch) => {
        dispatch({ type: tjenesteplanConstants.TJENESTEPLANCONFIG_CHANGED, config});
    }
}
