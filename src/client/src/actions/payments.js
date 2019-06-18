import {fetchCard} from "./cards";

import axios from "axios";
import {PAYMENT_FAILED, PAYMENT_REPEAT, PAYMENT_SUCCESS} from "./types";
import {ROOT_URL} from "./consts";

/**
 * Проводит withdraw транзакцию
 *
 * @param {String} from
 * @param {String} to
 * @param {Integer} sum
 * @returns
 */
export const TransferMoney = (from, to, sum) => {
    // формируем транзакцию
    const transaction = {
        from,
        to,
        sum
    };

    return dispatch => {
        axios
            .post(`${ROOT_URL}/transactions`, transaction)
            .then(response => {
                if (response.status === 201) {
                    dispatch({
                        type: PAYMENT_SUCCESS,
                        payload: response.data
                    });
                    dispatch(fetchCard(from));
                }
            })
            .catch(err => {
                dispatch({
                    type: PAYMENT_FAILED,
                    payload: {
                        error: err.response.data.message
                            ? err.response.data.message
                            : err.response.data,
                        transaction
                    }
                });

                console.error(
                    err.response.data.message
                        ? err.response.data.message
                        : err.response.data
                );
            });
    };
};

export const repeatTransferMoney = () =>
    ({
        type: PAYMENT_REPEAT
    });
