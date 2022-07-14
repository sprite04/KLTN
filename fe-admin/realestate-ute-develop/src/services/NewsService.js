import axiosInstance from '../services/AxiosInstance';

export function createNews(newsData) {
    return axiosInstance.post(`News`, newsData);
}

export function updateNews(newsData) {
    return axiosInstance.put(`News`, newsData);
}

export function deleteNews(id) {
    return axiosInstance.delete(`News?id=${id}`);
}

export function GetNews(data) {
    const searchParams = new URLSearchParams();

    for (var key in data) {
        searchParams.append(key, data[key]);
    }

    let query = searchParams.toString();

    if (query)
        return axiosInstance.get(`News?${query}`);
    else
        return axiosInstance.get(`News`);
}

export function GetNewsById(id) {
    return axiosInstance.get(`News/${id}`);
}

export function UpdateStatusNews(id, display) {
    console.log("display", display)
    return axiosInstance.put(`News/UpdateStatusNews?id=${id}&display=${display}`);
}