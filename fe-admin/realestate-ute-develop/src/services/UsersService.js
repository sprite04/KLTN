import axiosInstance from './AxiosInstance';

export function getUsers(data) {
    const searchParams = new URLSearchParams();

    for (var key in data) {
        searchParams.append(key, data[key]);
    }

    let query = searchParams.toString();
    console.log(query)

    if (query)
        return axiosInstance.get(`UserManagements/GetUsers?${query}`);
    else
        return axiosInstance.get(`UserManagements/GetUsers`);
}

export function lockunlock(id) {
    return axiosInstance.delete(`UserManagements/LockUnlock?userId=${id}`);
}

export function getUserById(id) {
    return axiosInstance.get(`UserManagements/GetUserById?id=${id}`);
}

export function createUser(userData) {
    return axiosInstance.post(`UserManagements`, userData);
}

export function updateUser(userData) {
    return axiosInstance.put(`UserManagements`, userData);
}

export function changeInfo(userData) {
    return axiosInstance.post(`Auths/ChangeInfo`, userData);
}

export function changePassword(userData) {
    return axiosInstance.post(`Auths/ChangePassword`, userData);
}

// export function formatPosts(postsData) {
//     let posts = [];

//     for (let key in postsData) {
//         posts.push({ ...postsData[key], id: key });
//     }

//     return posts;
// }