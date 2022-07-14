import axiosInstance from './AxiosInstance';


//Lấy report processing đến hạn
export function getReportProcessing(data) {

    const searchParams = new URLSearchParams();

    for (var key in data) {
        searchParams.append(key, data[key]);
    }

    let query = searchParams.toString();

    if (query)
        return axiosInstance.get(`ReportProcessings?${query}`);
    else
        return axiosInstance.get(`ReportProcessings`);
}

export function blockAllPosts() {
    return axiosInstance.put(`ReportProcessings/BlockAllPosts`);
}

export function GetReportProcessingByPost(id) {
    return axiosInstance.get(`ReportProcessings/${id}`);
}

export function UpdateStatusReportProcessing(data) {
    const searchParams = new URLSearchParams();

    for (var key in data) {
        searchParams.append(key, data[key]);
    }

    let query = searchParams.toString();

    return axiosInstance.put(`ReportProcessings/UpdateStatusReportProcessing?${query}`);
}