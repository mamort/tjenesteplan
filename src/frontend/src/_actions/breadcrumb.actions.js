export const breadcrumbActions = {
    add,
    set,
    clear
};

function add(crumb) {
    return (dispatch) => {
        dispatch({ type: "ADD_BREADCRUMB", crumb });
    }
}

function set(crumbs) {
    return (dispatch) => {
        dispatch({ type: "SET_BREADCRUMB", crumbs });
    }
}

function clear() {
    return { type: "CLEAR_BREADCRUMBS" };
}