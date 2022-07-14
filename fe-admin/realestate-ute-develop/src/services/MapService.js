import axios from 'axios';

export function getLocation(address) {
    // return axios.get(`${process.env.REACT_APP_API_MAP}json?address=${address}&key=${process.env.REACT_APP_TOKEN_MAP}`);
    return axios.get(`https://maps.googleapis.com/maps/api/geocode/json?address=${address}&key=${process.env.REACT_APP_TOKEN_MAP}`);
}