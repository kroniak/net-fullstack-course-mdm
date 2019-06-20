import {
    ACTIVE_CARD_CHANGED,
    CARDS_FETCH_FAILED,
    TRANS_FETCH_FAILED,
    TRANS_FETCH_STARTED,
    TRANS_FETCH_SUCCESS
} from "../actions/types";

const initialState = {
    data: [],
    error: null,
    isLoading: true,
    skip: 0,
    count: 0
};

export default (state = initialState, {type, payload}) => {
    switch (type) {
        case TRANS_FETCH_STARTED:
            return {
                ...state,
                isLoading: state.data.length === 0 ? true : false
            };

        case TRANS_FETCH_SUCCESS:
            return {
                ...state,
                data: payload.data,
                skip: payload.skip,
                error: null,
                isLoading: false,
                count: payload.data.length
            };

        case TRANS_FETCH_FAILED:
            return {
                ...state,
                error: payload.error,
                skip: payload.skip,
                isLoading: false
            };

        case CARDS_FETCH_FAILED:
            return initialState;

        case ACTIVE_CARD_CHANGED:
            return {
                ...state,
                skip: 0
            };

        default:
            return state;
    }
};
