
export function breadcrumbs(state = { crumbs: [] }, action) {
  switch (action.type) {
    case 'ADD_BREADCRUMB':
        const crumbs = [...state.crumbs];
        crumbs.push(action.crumb);

        return {
            ...state,
            crumbs: crumbs
        };
    case 'SET_BREADCRUMB':
        return {
            crumbs: action.crumbs
        };
    case 'CLEAR_BREADCRUMBS':
        return {};
    default:
      return state
  }
}