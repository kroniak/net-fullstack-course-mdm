import {USER_LOGGED, USER_LOGOUT} from "../actions/types";

const initialState = {
    isAuth: false,
    userName: null,
    token: null,
    error: null
};

export default (state = initialState, {type, payload}) => {
    switch (type) {
        case USER_LOGGED:
            return {
                ...state,
                isAuth: true,
                token: payload.token,
                userName: payload.username,
                error: null
            };

        case USER_LOGOUT:
            return initialState;

        default:
            return state;
    }
};
