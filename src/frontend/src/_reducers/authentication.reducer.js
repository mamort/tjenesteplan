import { userConstants } from '../_constants';
import { userService } from '../_services';

let user = userService.getUser();
const initialState = user
  ? { loggedIn: true, user }
  : { loggedIn: false, user: null };

export function authentication(state = initialState, action) {
  switch (action.type) {
    case userConstants.LOGIN_REQUEST:
      return {
        ...state,
        loggingIn: true,
        user: action.user
      };
    case userConstants.LOGIN_SUCCESS:
      return {
        ...state,
        loggedIn: true,
        loggingIn: false,
        user: action.user
      };
    case userConstants.LOGIN_FAILURE:
      return {
        ...state,
        loggedIn: false,
        loggingIn: false,
        user: null
      };
    case userConstants.LOGOUT:
      return {
        ...state,
        loggedIn: false,
        user: null
      };
    case userConstants.RESETPASSWORD_REQUEST:
      return {
        ...state,
        resettingPassword: true,
        isReset: false
      };
    case userConstants.RESETPASSWORD_SUCCESS:
      return {
        ...state,
        resettingPassword: false,
        isReset: true
      };
    case userConstants.RESETPASSWORD_FAILURE:
      return {
        ...state,
        resettingPassword: false
      };
    case userConstants.RESETPASSWORD_RESET:
      return {
        ...state,
        resettingPassword: false,
        isReset: false
      };

    case userConstants.NEWPASSWORD_REQUEST:
    return {
      ...state,
      isSavingNewPassord: true,
      isNewPasswordSaved: false
    };
  case userConstants.NEWPASSWORD_SUCCESS:
    return {
      ...state,
      isSavingNewPassord: false,
      isNewPasswordSaved: true
    };
  case userConstants.NEWPASSWORD_FAILURE:
    return {
      ...state,
      isSavingNewPassord: false
    };
  case userConstants.NEWPASSWORD_RESET:
    return {
      ...state,
      isSavingNewPassord: false,
      isNewPasswordSaved: false
    };
    default:
      return state
  }
}