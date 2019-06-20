import {fetchTransactions} from "./transactions";

import axios from "axios";

import {
    ACTIVE_CARD_CHANGED,
    CARD_ADD_FAILED,
    CARD_ADD_STARTED,
    CARD_ADD_SUCCESS,
    CARD_FETCH_FAILED,
    CARD_FETCH_SUCCESS,
    CARDS_FETCH_FAILED,
    CARDS_FETCH_STARTED,
    CARDS_FETCH_SUCCESS
} from "./types";

import {ROOT_URL} from "./consts";

/**
 * Добавляет новую карту пользователю
 *
 */
export const addCard = (currency, type, name) => dispatch => {
    dispatch({
        type: CARD_ADD_STARTED
    });

    const data = {
        name,
        currency,
        type
    };

    axios
        .post(`${ROOT_URL}/cards`, data)
        .then(response => {
            if (response.status === 201) {
                dispatch({
                    type: CARD_ADD_SUCCESS
                });
                dispatch(fetchCards());
            } else
                dispatch({
                    type: CARD_ADD_FAILED,
                    payload: "Что то пошло не так..."
                });
        })
        .catch(err => {
            console.error(
                err.response.data.message
                    ? err.response.data.message
                    : err.response.data
            );
            dispatch({
                type: CARD_ADD_FAILED,
                payload: err.response.data.message
                    ? err.response.data.message
                    : err.response.data
            });
        });
};

/**
 * Вытаскивает данные по картам пользователя
 *
 */
export const fetchCards = () => dispatch => {
        dispatch({
            type: CARDS_FETCH_STARTED
        });

        axios
            .get(`${ROOT_URL}/cards`)
            .then(response => {
                    if (response.status === 200) {
                        dispatch({
                            type: CARDS_FETCH_SUCCESS,
                            payload: response.data
                        });

                        if (response.data.length > 0)
                            if (response.data[0].number)
                                dispatch(changeActiveCard(response.data[0].number));
                    }
                }
            )
            .catch(err => {
                console.error(
                    err.response.data.message
                        ? err.response.data.message
                        : err.response.data
                );
                dispatch({
                    type: CARDS_FETCH_FAILED,
                    payload: err.response.data.message
                        ? err.response.data.message
                        : err.response.data
                });
            });
    }
;

/**
 * Вытаскивает данные по карте пользователя
 *
 * @param {String} number
 */
export const fetchCard = number => dispatch => {
    axios
        .get(`${ROOT_URL}/cards/${number}`)
        .then(response => {
            if (response.status === 200) {
                dispatch({
                    type: CARD_FETCH_SUCCESS,
                    payload: response.data
                })
            }
        })
        .catch(err => {
            console.error(
                err.response.data.message
                    ? err.response.data.message
                    : err.response.data
            );
            dispatch({
                type: CARD_FETCH_FAILED,
                payload: err.response.data.message
                    ? err.response.data.message
                    : err.response.data
            });
        });
};

export const changeActiveCard = number => (dispatch, getState) => {
    const {activeCardNumber} = getState().cards;

    if (activeCardNumber === number) return;

    dispatch({
        type: ACTIVE_CARD_CHANGED,
        payload: number
    });

    dispatch(fetchCard(number));
    dispatch(fetchTransactions(number));
};
