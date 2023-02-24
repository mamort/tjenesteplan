import { userConstants } from '../_constants';
import tjenesteplan from '../Api/Tjenesteplaner';

const initialState = {
  loading: false,
  items: null,
  leger: []
};

export function users(state = initialState, action) {
  switch (action.type) {
    case userConstants.GETALL_REQUEST:
      return {
        ...state,
        loading: true
      };
    case userConstants.GETALL_SUCCESS:
      return {
        ...state,
        loading: false,
        items: action.users
      };
    case userConstants.GETALL_FAILURE:
      return {
        ...state,
        loading: false,
        error: action.error.msg
      };
    case userConstants.DELETE_REQUEST:
      // add 'deleting:true' property to user being deleted
      return {
        ...state,
        items: state.items.map(user =>
          user.id === action.id
            ? { ...user, deleting: true }
            : user
        )
      };
    case userConstants.DELETE_SUCCESS:
      // remove deleted user from state
      return {
        ...state,
        loading: false,
        items: state.items.filter(user => user.id !== action.id)
      };
    case userConstants.DELETE_FAILURE:
      // remove 'deleting:true' property and add 'deleteError:[error]' property to user
      return {
        ...state,
        loading: false,
        items: state.items.map(user => {
          if (user.id === action.id) {
            // make copy of user without 'deleting:true' property
            const { deleting, ...userCopy } = user;
            // return copy of user with 'deleteError:[error]' property
            return { ...userCopy, deleteError: action.error.msg };
          }

          return user;
        })
      };

      case userConstants.START_GET_LEGER:
      return {
        ...state,
        loading: true
      };
      case userConstants.GET_LEGER_SUCCEEDED:
        return {
          ...state,
          loading: false,
          leger: action.payload
        };
      case userConstants.GET_LEGER_FAILED:
        return {
          ...state,
          loading: false,
          error: action.error
        };
        case userConstants.START_GET_ALLELEGER:
      return {
        ...state,
        loading: true
      };
      case userConstants.GET_ALLELEGER_SUCCEEDED:
        return {
          ...state,
          loading: false,
          alleLeger: action.payload
        };
      case userConstants.GET_ALLELEGER_FAILED:
        return {
          ...state,
          loading: false,
          error: action.error
        };
        case userConstants.GETCURRENTUSER_REQUEST:
        return {
          ...state,
          loading: true
        };
        case userConstants.GETCURRENTUSER_SUCCESS:
        return {
          ...state,
          loading: false,
          currentUser: action.user
        };
        case userConstants.GETCURRENTUSER_FAILURE:
        return {
          ...state,
          loading: false,
          error: action.error.msg
        };
        case userConstants.LOGOUT:
        case userConstants.LOGIN_REQUEST:
          return {
            ...state,
            currentUser: null
        };
        case userConstants.START_GET_LEGE:
          return {
            ...state,
            loading: true
          };
          case userConstants.GET_LEGE_SUCCEEDED:
            return {
              ...state,
              loading: false,
              lege: action.payload
            };
          case userConstants.GET_LEGE_FAILED:
            return {
              ...state,
              loading: false,
              error: action.error
            };
    default:
      return state
  }
}