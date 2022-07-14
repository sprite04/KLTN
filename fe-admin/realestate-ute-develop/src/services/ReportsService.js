import axiosInstance from './AxiosInstance';

export function getReports(data) {
    const searchParams = new URLSearchParams();

    for (var key in data) {
        searchParams.append(key, data[key]);
    }

    let query = searchParams.toString();

    if (query) 
        return axiosInstance.get(`Reports?${query}`);
    else
        return axiosInstance.get(`Reports`);
}

export function GetReportByPost(postId, data) {
    const searchParams = new URLSearchParams();

    for (var key in data) {
        searchParams.append(key, data[key]);
    }

    let query = searchParams.toString();
    console.log("reportpost", query)

    if (query)
        return axiosInstance.get(`Reports/${postId}?${query}`);
    else
        return axiosInstance.get(`Reports/${postId}`);
}

