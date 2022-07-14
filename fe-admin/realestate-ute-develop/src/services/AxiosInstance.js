import axios from 'axios';
import { store } from '../store/store';

const axiosInstance = axios.create({
    baseURL: process.env.REACT_APP_API_ENDPOINT,    // `http://localhost:50804/api/`,
});

axiosInstance.interceptors.request.use((config) => {
    const state = store.getState();
    const token = state.auth.auth.idToken;
    config.headers['Authorization'] = `Bearer ${token}`
    // config.params = config.params || {};
    // config.params['auth'] = token;
    return config;
});

export default axiosInstance;
