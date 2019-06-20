import {ACTIVE_CARD_CHANGED, PAYMENT_FAILED, PAYMENT_REPEAT, PAYMENT_SUCCESS, USER_LOGOUT} from "../actions/types";

const initialState = {
    stage: "contract",
    transaction: null,
    error: null
};

export default (state = initialState, {type, payload}) => {
    switch (type) {
        case USER_LOGOUT: return initialState;

        case PAYMENT_REPEAT:
            return initialState;

        case PAYMENT_SUCCESS:
            return {
                ...state,
                stage: "success",
                transaction: payload,
                error: null
            };

        case PAYMENT_FAILED:
            return {
                ...state,
                stage: "error",
                transaction: payload.transaction,
                error: payload.error
            };

        case ACTIVE_CARD_CHANGED:
            return {
                ...state,
                stage: "contract",
                transaction: null,
                error: null
            };

        default:
            return state;
    }
};
