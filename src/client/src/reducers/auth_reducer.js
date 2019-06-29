import {USER_LOGIN_FAILED, USER_LOGIN_SUCCESS, USER_LOGOUT} from "../actions/types";

const initialState = {
    isAuth: false,
    userName: null,
    token: null,
    error: null
};

export default (state = initialState, {type, payload}) => {
    switch (type) {
        case USER_LOGIN_FAILED:
            return {
                ...state,
                isAuth: false,
                token: null,
                userName: null,
                error: payload
            };
        case USER_LOGIN_SUCCESS:
            return {
                ...state,
                isAuth: true,
                token: payload.token,
                userName: payload.userName,
                error: null
            };

        case USER_LOGOUT:
            return initialState;

        default:
            return state;
    }
};
