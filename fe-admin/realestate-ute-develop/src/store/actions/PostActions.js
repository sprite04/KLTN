import {
    createPost,
    formatPosts,
    getPosts,
    updateStatusPost,
    deletePost,
    updatePost,
} from '../../services/PostsService';
import {
    CONFIRMED_CREATE_POST_ACTION,
    CONFIRMED_DELETE_POST_ACTION,
    CONFIRMED_EDIT_POST_ACTION,
    CONFIRMED_GET_POSTS,
    FAILED_GET_POSTS_ACTION,
    CONFIRMED_UPDATE_STATUS_POST
} from './PostTypes';

import { ToastContainer, toast } from 'react-toastify';




export function createPostAction(postData, history) {
   
	return (dispatch, getState) => {
        createPost(postData).then((response) => {
			//console.log(response);
            const singlePost = {
                ...postData,
                id: response.data.name,
            };
            dispatch(confirmedCreatePostAction(singlePost));
            history.push('/postpage');
        });
    };
}

export function updatePostAction(post, history) {
    return (dispatch, getState) => {
        updatePost(post, post.id).then((reponse) => {
            //console.log(reponse);
            dispatch(confirmedUpdatePostAction(post));
            history.push('/postpage');
        });

    };
}

export function deletePostAction(postId, history) {
    return (dispatch, getState) => {
        deletePost(postId).then((response) => {
            dispatch(confirmedDeletePostAction(postId));
            history.push('/postpage');
        });
    };
}

export function getPostsAction(query) {
    return (dispatch, getState) => {
        getPosts(query).then((response) => {
            dispatch(confirmedGetPostsAction(response.data));
        })
        .catch((error) => {
            dispatch(getPostsFailedAction(error));
        });
    };
}

export function updateStatusPostAction(postId, statusId) {
    let data = {
        id: postId,
        statusID: statusId
    }
    return (dispatch, getState) => {
        updateStatusPost(data).then((response) => {
            console.log("meme", response)
            if (response.data) {
                toast.success("Thực hiện thành công", {
                    type: toast.TYPE.SUCCESS,
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                })
                dispatch(confirmedUpdateStatusPostAction(data))
            }
            else {
                toast.error("Thực hiện thất bại",
                    {
                        position: 'top-right',
                        autoClose: 5000,
                        hideProgressBar: false,
                        closeOnClick: true,
                        pauseOnHover: true,
                        draggable: true,
                        progress: undefined
                    }
                )
            }    
        })
        .catch((error) => {
            console.log("error------------", error);
            toast.error("Thực hiện thất bại",
                {
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined
                }
            )
        })
    };
}

export function getPostsFailedAction(data) {
    return {
        type: FAILED_GET_POSTS_ACTION,
        payload: data,
    };
}

export function confirmedCreatePostAction(singlePost) {
	
    return {
        type: CONFIRMED_CREATE_POST_ACTION,
        payload: singlePost,
    };
}

export function confirmedGetPostsAction(posts) {
    return {
        type: CONFIRMED_GET_POSTS,
        payload: posts,
    };
}

export function confirmedUpdateStatusPostAction(data) {
    
    return {
        type: CONFIRMED_UPDATE_STATUS_POST,
        payload: data,
    };
}

export function confirmedUpdatePostAction(post) {

    return {
        type: CONFIRMED_EDIT_POST_ACTION,
        payload: post,
    };
}

export function confirmedDeletePostAction(postId) {
    return {
        type: CONFIRMED_DELETE_POST_ACTION,
        payload: postId,
    };
}

