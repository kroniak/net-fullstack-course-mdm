import axios from "axios";

import {AUTH_URL, VERIFY_URL} from "./consts";
import {
    USER_LOGIN_FAILED,
    USER_LOGIN_SUCCESS,
    USER_LOGOUT,
    USER_TOKEN_VERIFY_COMPLETE,
    USER_TOKEN_VERIFY_START
} from "./types";

import {fetchCards} from "./cards";

export const verifyToken = () => {
    return async dispatch => {
        dispatch({
            type: USER_TOKEN_VERIFY_START
        });

        let token = localStorage.getItem('token');
        if (!token) return;

        axios
            .get(VERIFY_URL, {
                headers: {
                    authorization: 'Bearer ' + token
                }
            })
            .then(response => {
                dispatch({
                    type: USER_LOGIN_SUCCESS,
                    payload: {
                        userName: response.data.userName,
                        token: token
                    }
                });

                dispatch(fetchCards());
            })
            .catch(err => {
                dispatch(logoutUser());
            })
            .finally(() => {
                dispatch({
                    type: USER_TOKEN_VERIFY_COMPLETE
                });
            });
    };
};


export const logUser = (userName, password) => dispatch => {
    const data = {
        userName,
        password
    };

    axios
        .post(`${AUTH_URL}`, data)
        .then(response => {
            if (response.status === 200) {
                if (response.data.token) {
                    localStorage.setItem('token', response.data.token);
                    dispatch({
                        type: USER_LOGIN_SUCCESS,
                        payload: {
                            token: response.data.token,
                            userName: userName
                        }
                    });
                    dispatch(fetchCards());
                }
            } else
                dispatch({
                    type: USER_LOGIN_FAILED,
                    payload: "Что то пошло не так..."
                });
        })
        .catch(err => {
            if (err.response) {
                console.error(
                    err.response.data.message
                        ? err.response.data.message
                        : err.response.data
                );
                dispatch({
                    type: USER_LOGIN_FAILED,
                    payload: err.response.data.message
                        ? err.response.data.message
                        : err.response.data
                });
            }
        });
};

export const logoutUser = () => dispatch => {
    localStorage.removeItem('token');
    dispatch({
        type: USER_LOGOUT,
    });
};
