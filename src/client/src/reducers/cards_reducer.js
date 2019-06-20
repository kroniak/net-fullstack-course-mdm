import {
    ACTIVE_CARD_CHANGED,
    CARD_ADD_FAILED,
    CARD_FETCH_FAILED,
    CARD_FETCH_SUCCESS,
    CARDS_FETCH_FAILED,
    CARDS_FETCH_STARTED,
    CARDS_FETCH_SUCCESS, USER_LOGOUT
} from "../actions/types";

/**
 * Возвращает массив с обновленным элементом
 *
 * @param {Array} array
 * @param {Object} action
 * @returns {Array}
 */
const addOrUpdateObjectInArray = (array, newItem, field = "number") => {
    let isNew = true;
    const newArray = array.map(item => {
        if (item[field] !== newItem[field]) return item;
        isNew = false;
        return newItem;
    });
    if (isNew) newArray.push(newItem);
    return newArray;
};

const initialState = {
    data: [],
    error: null,
    isLoading: false,
    activeCardNumber: null
};

export default (state = initialState, {type, payload}) => {
    switch (type) {
        case USER_LOGOUT: return initialState;

        case CARDS_FETCH_STARTED:
            return {
                ...state,
                isLoading: state.data.length === 0
            };

        case CARD_ADD_FAILED:
            return {
                ...state,
                error: payload
            };

        case CARDS_FETCH_SUCCESS:
            return {
                ...state,
                data: payload,
                error: null,
                isLoading: false
            };

        case CARDS_FETCH_FAILED:
            return {
                ...state,
                error: payload,
                isLoading: false
            };

        case ACTIVE_CARD_CHANGED:
            return {
                ...state,
                activeCardNumber: payload
            };

        case CARD_FETCH_SUCCESS:
            return {
                ...state,
                data: addOrUpdateObjectInArray(state.data, payload)
            };

        case CARD_FETCH_FAILED:
            return {
                ...state,
                error: payload
            };

        default:
            return state;
    }
};
