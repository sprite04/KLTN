import axiosInstance from './AxiosInstance';

export function getCategories() {

    return axiosInstance.get(`Categories`);
}