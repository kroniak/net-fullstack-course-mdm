import axios from "axios";
import {TRANS_FETCH_FAILED, TRANS_FETCH_STARTED, TRANS_FETCH_SUCCESS} from "./types";
import {ROOT_URL} from "./consts";

/**
 * Вытаскивает транзакции по картам пользователя
 *
 * @returns
 */
export const fetchTransactions = (number, skip = 0) => {
    const skipParam = skip <= 0 ? "" : `?skip=${skip}`;

    return dispatch => {
        dispatch({
            type: TRANS_FETCH_STARTED
        });

        axios
            .get(`${ROOT_URL}/transactions/${number}${skipParam}`)
            .then(response => {
                if (response.status === 200) {
                    dispatch({
                        type: TRANS_FETCH_SUCCESS,
                        payload: {data: response.data, skip}
                    });
                }
            })
            .catch(err => {
                dispatch({
                    type: TRANS_FETCH_FAILED,
                    payload: {
                        error: err.response.data.message
                            ? err.response.data.message
                            : err.response.data,
                        skip
                    }
                });
                console.error(
                    err.response.data.message
                        ? err.response.data.message
                        : err.response.data
                );
            })
    };
};
