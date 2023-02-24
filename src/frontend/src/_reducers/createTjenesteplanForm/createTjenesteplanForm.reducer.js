import { combineReducers } from 'redux';
import { config } from './tjenesteplanConfigForm.reducer';

const createTjenesteplanForm = combineReducers({
    config
});

export default createTjenesteplanForm;