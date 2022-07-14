import axiosInstance from '../services/AxiosInstance';

export function getPosts(data) {
    const searchParams = new URLSearchParams();

    for (var key in data) {
        searchParams.append(key, data[key]);
    }

    let query = searchParams.toString();
    console.log("post", query)

    if (query)
        return axiosInstance.get(`PostManagements?${query}`);
    else
        return axiosInstance.get(`PostManagements`);
}

export function updateStatusPost(data) {
    return axiosInstance.put(`PostManagements`, data);
}

export function getPostById(postId) {
    return axiosInstance.get(`Posts/${postId}`);
}

export function createPost(postData) {
    return axiosInstance.post(`posts.json`, postData);
}

export function updatePost(post, postId) {
    return axiosInstance.put(`posts/${postId}.json`, post);
}

export function deletePost(postId) {
    return axiosInstance.delete(`posts/${postId}.json`);
}

export function formatPosts(postsData) {
    let posts = [];

    for (let key in postsData) {
        posts.push({ ...postsData[key], id: key });
    }

    return posts;
}

export function report4(id) {
    return axiosInstance.get(`PostManagements/Report4?id=${id}`)
}

export function report1() {
    return axiosInstance.get(`PostManagements/Report1`)
}

