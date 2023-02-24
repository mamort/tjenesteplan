import { combineReducers } from 'redux';

import { authentication } from './authentication.reducer';
import { registration } from './registration.reducer';
import { users } from './users.reducer';
import { alert } from './alert.reducer';
import { breadcrumbs } from './breadcrumb.reducer';
import { sykehus } from './sykehus.reducer';
import { avdelinger } from './avdelinger.reducer';
import { tjenesteplaner } from './tjenesteplaner.reducer';
import { tjenesteplanInfo } from './tjenesteplanInfo.reducer';
import { weeklyTjenesteplan } from './weeklytjenesteplan.reducer';
import { mintjenesteplan } from './mintjenesteplan.reducer';
import { dagsplaner } from './dagsplaner.reducer';
import { invitations } from './invitations.reducer';
import { vaktChangeRequests } from './vaktChangeRequests.reducer';
import { vakansvaktRequests } from './vakansvaktRequests.reducer';
import { notifications } from './notifications.reducer';
import { legeSpesialiteter } from './legeSpesialiteter.reducer';
import { tjenesteplanChanges } from './tjenesteplanChanges.reducer';
import createTjenesteplanForm from './createTjenesteplanForm/createTjenesteplanForm.reducer';

const rootReducer = combineReducers({
  authentication,
  registration,
  users,
  alert,
  breadcrumbs,
  sykehus,
  avdelinger,
  tjenesteplaner,
  tjenesteplanInfo,
  tjenesteplanChanges,
  mintjenesteplan,
  weeklyTjenesteplan,
  dagsplaner,
  invitations,
  vaktChangeRequests,
  vakansvaktRequests,
  notifications,
  createTjenesteplanForm,
  legeSpesialiteter
});

export default rootReducer;