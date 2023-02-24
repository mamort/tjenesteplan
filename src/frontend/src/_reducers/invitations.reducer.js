const initialState = {
    isFetching: false,
    isFailed: false,
    invitations: {},
    items: []
}

export function invitations(state = initialState, action) {
    switch (action.type) {
        case 'START_GET_INVITATION':
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case 'GET_INVITATION_SUCCEEDED':{
            const invitation = action.payload;

            const invitations = {...state.invitations};
            invitations[invitation.id] = invitation;

            return {
                ...state,
                invitations,
                isFetching: false
            };
        }
        case 'GET_INVITATION_FAILED':
            return {
                ...state,
                isFetching: false,
                isFailed: true,
                error: action.error
            };
        case 'START_GET_INVITATIONS':
            return {
                ...state,
                isFetching: true,
                isFailed: false
            };
        case 'GET_INVITATIONS_SUCCEEDED':{
            return {
                ...state,
                items: action.payload,
                isFetching: false
            };
        }
        case 'GET_INVITATIONS_FAILED':
            return {
                ...state,
                isFetching: false,
                isFailed: true,
                error: action.error
            };
        case 'START_REGISTER_INVITATION':
            return {
                ...state,
                isRegistering: true,
                isFailed: false
            };
        case 'REGISTER_INVITATION_SUCCEEDED':
            return {
                ...state,
                isRegistering: false,
                isFailed: false
            };
        case 'REGISTER_INVITATION_FAILED':
            return {
                ...state,
                isRegistering: false,
                isFailed: true,
                error: action.error
            };
        default:
            return state
    }
}