import {
    getProvince,
    getAllProvinces
} from '../../services/ProvincesService';

export const GET_PROVINCE = '[Province Action] Get Province';
export const GET_ALL_PROVINCES = '[Province Action] Get All Provinces';
export const CONFIRMED_GET_PROVINCES = '[Province Action] Confirmed Get Provinces';
export const FAILED_GET_PROVINCES_ACTION = '[Province Action] Failed Get Provinces';

export function getProvincesAction() {
    return (dispatch, getState) => {
        getAllProvinces().then((response) => {
            //console.log(response.data);
            //let posts = formatPosts(response.data);
            dispatch(confirmedGetProvincesAction(response.data));
        })
        .catch((error) => {
            dispatch(getProvincesFailedAction(error));
        });
    };
}

export function confirmedGetProvincesAction(provinces) {
    return {
        type: CONFIRMED_GET_PROVINCES,
        payload: provinces,
    };
}

export function getProvincesFailedAction(data) {
    return {
        type: FAILED_GET_PROVINCES_ACTION,
        payload: data,
    };
}
