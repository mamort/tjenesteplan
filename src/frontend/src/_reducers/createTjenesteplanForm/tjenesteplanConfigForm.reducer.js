import { tjenesteplanConstants } from '../../Tjenesteplan/tjenesteplan.constants';

const initialState = {
  startDate: "",
  name: "",
  numberOfLeger: 0
}

export function config(state = initialState, action) {
  switch (action.type) {
    case tjenesteplanConstants.TJENESTEPLANCONFIG_CHANGED:
      return {
          ...state,
          ...action.config
      };
    default:
      return state
  }
}