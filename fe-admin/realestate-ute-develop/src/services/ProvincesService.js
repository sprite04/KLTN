import axios from 'axios';

export function getProvince(id) {

    return axios.get(`${process.env.REACT_APP_API_PROVINCES}/p/${id}?depth=3`);
}

export function getAllProvinces() {

    return axios.get(`${process.env.REACT_APP_API_PROVINCES}?depth=3`);
}
