import {
    formatError,
    displayError,
    login,
    runLogoutTimer,
    saveTokenInLocalStorage,
    signUp,
} from '../../services/AuthService';

import { isExpired, decodeToken } from "react-jwt";

export const SIGNUP_CONFIRMED_ACTION = '[signup action] confirmed signup';
export const SIGNUP_FAILED_ACTION = '[signup action] failed signup';
export const LOGIN_CONFIRMED_ACTION = '[login action] confirmed login';
export const LOGIN_FAILED_ACTION = '[login action] failed login';
export const LOADING_TOGGLE_ACTION = '[Loading action] toggle loading';
export const LOGOUT_ACTION = '[Logout action] logout action';

export function signupAction(email, password, history) {
    return (dispatch) => {
        signUp(email, password)
            .then((response) => {
                saveTokenInLocalStorage(response.data);
                runLogoutTimer(
                    dispatch,
                    response.data.expiresIn * 1000,
                    history,
                );
                dispatch(confirmedSignupAction(response.data));
                history.push('/');
            })
            .catch((error) => {
                const errorMessage = formatError(error.response.data);
                dispatch(signupFailedAction(errorMessage));
            });
    };
}

export function logout(history) {
    localStorage.removeItem('userDetails');
    history.push('/login');
    return {
        type: LOGOUT_ACTION,
    };
}

export function loginAction(email, password, history) {
    return (dispatch) => {
        login(email, password)
            .then((response) => {

                //     dispatch(loginConfirmedAction(response.data));
                //     history.push('/dashboard');
                // }
                
                if (response.data.accessToken && response.data.refreshToken)
                {
                    response.data.email = email;
                    response.data.idToken = response.data.accessToken;
                    response.data.expiresIn = 12000;
                    


                    
                    

                    const myDecodedToken = decodeToken(response.data.accessToken);
                    response.data.id = myDecodedToken.id;
                    response.data.name = myDecodedToken.name;
                    response.data.rolename = myDecodedToken.rolename;
                    response.data.avatar = myDecodedToken.avatar;
                    saveTokenInLocalStorage(response.data);

                    let expireDate = new Date(myDecodedToken.exp * 1000);
                    let todaysDate = new Date();

                    let interval = expireDate > todaysDate ? myDecodedToken.exp * 1000 - todaysDate.getTime() : 0;
                    runLogoutTimer(
                        dispatch,
                        interval,
                        history,
                    );
                    dispatch(loginConfirmedAction(response.data));
                    history.push('/dashboard');
                }
                else
                {
                    const errorMessage = displayError();
                    dispatch(loginFailedAction(errorMessage));
                }





                //window.location.reload();

                //history.pushState('/index');

            })
            .catch((error) => {
                // const errorMessage = formatError(error.response.data);
                // dispatch(loginFailedAction(errorMessage));
                const errorMessage = displayError();
                dispatch(loginFailedAction(errorMessage));
            });
    };
}

export function loginFailedAction(data) {
    return {
        type: LOGIN_FAILED_ACTION,
        payload: data,
    };
}

export function loginConfirmedAction(data) {
    console.log(data);
    return {
        type: LOGIN_CONFIRMED_ACTION,
        payload: data,
    };
}

export function confirmedSignupAction(payload) {
    return {
        type: SIGNUP_CONFIRMED_ACTION,
        payload,
    };
}

export function signupFailedAction(message) {
    return {
        type: SIGNUP_FAILED_ACTION,
        payload: message,
    };
}

export function loadingToggleAction(status) {
    return {
        type: LOADING_TOGGLE_ACTION,
        payload: status,
    };
}
