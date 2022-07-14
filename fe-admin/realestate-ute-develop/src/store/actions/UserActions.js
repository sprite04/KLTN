import {
    getUsers,
    lockunlock,
    getUserById,
    createUser,
    updateUser,
    changeInfo,                                  //thuc hien ben auth
    changePassword                               //thuc hien ben auth
} from '../../services/UsersService';

import { ToastContainer, toast } from 'react-toastify';

//Đang chỉnh sửa ở đây

export const GET_USERS = '[User Action] Get Users';
export const CONFIRMED_GET_USERS = '[User Action] Confirmed Get Users';
export const FAILED_GET_USERS_ACTION = '[User Action] Failed Get Users';
export const CREATE_USER_ACTION = '[User Action] Create User';
export const CONFIRMED_CREATE_USER_ACTION = '[User Action] Confirmed Create User';
export const EDIT_USER_ACTION = '[User Action] Edit User';
export const CONFIRMED_EDIT_USER_ACTION = '[User Action] Confirmed Edit User';
export const CONFIRMED_LOCK_UNLOCK_USER_ACTION = '[User Action] Confirmed Lock Unlock User';


export function getUsersAction(query) {
    return (dispatch, getState) => {
        getUsers(query).then((response) => {
            dispatch(confirmedGetUsersAction(response.data));
        })
            .catch((error) => {
                dispatch(getUsersFailedAction(error));
            });
    };
}

export function confirmedGetUsersAction(users) {
    return {
        type: CONFIRMED_GET_USERS,
        payload: users,
    };
}

export function getUsersFailedAction(data) {
    return {
        type: FAILED_GET_USERS_ACTION,
        payload: data,
    };
}

export function updateLockUnlockUserAction(userId) {
    
    return (dispatch, getState) => {
        lockunlock(userId).then((response) => {
            console.log(response)
            //có thể xem lại response lại ở đây để xác định lock unlock thực tế
            dispatch(confirmLockUnlockUserAction(userId, response.data.lock))
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

export function confirmLockUnlockUserAction(userId, status) {
    let data = {
        id: userId,
        lock: status
    }
    return {
        type: CONFIRMED_LOCK_UNLOCK_USER_ACTION,
        payload: data,
    };
}

export function createUserAction(userData, history) {

    return (dispatch, getState) => {
        console.log("userData", userData)
        createUser(userData).then((response) => {
            console.log(response);
            if (response.data.succeeded) {


                dispatch(confirmedCreateUserAction(response.data.user));
                

                toast.success("Tạo tài khoản thành công", {
                    type: toast.TYPE.SUCCESS,
                    position: 'top-right',
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                })

                history.push('/customer-list');
            }
            else {
                toast.error("Tạo tài khoản thất bại",
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
            
        });
    };
}

export function confirmedCreateUserAction(singleUser) {

    return {
        type: CONFIRMED_CREATE_USER_ACTION,
        payload: singleUser,
    };
}

export function confirmedUpdateUserAction(singleUser) {

    console.log("singleUser", singleUser)
    return {
        type: CONFIRMED_EDIT_USER_ACTION,
        payload: singleUser,
    };
}

// export function updatePostAction(post, history) {
//     return (dispatch, getState) => {
//         updatePost(post, post.id).then((reponse) => {
//             //console.log(reponse);
//             dispatch(confirmedUpdatePostAction(post));
//             history.push('/postpage');
//         });

//     };
// }

// export function deletePostAction(postId, history) {
//     return (dispatch, getState) => {
//         deletePost(postId).then((response) => {
//             dispatch(confirmedDeletePostAction(postId));
//             history.push('/postpage');
//         });
//     };
// }



// export function updateStatusPostAction(postId, statusId) {
//     let data = {
//         id: postId,
//         statusID: statusId
//     }
//     return (dispatch, getState) => {
//         updateStatusPost(data).then((response) => {
//             dispatch(confirmedUpdateStatusPostAction(data))
//         })
//             .catch((error) => {
//                 console.log("error------------", error);
//             })
//     };
// }







// export function confirmedUpdateStatusPostAction(data) {
//     return {
//         type: CONFIRMED_UPDATE_STATUS_POST,
//         payload: data,
//     };
// }

// export function confirmedUpdatePostAction(post) {

//     return {
//         type: CONFIRMED_EDIT_POST_ACTION,
//         payload: post,
//     };
// }

// export function confirmedDeletePostAction(postId) {
//     return {
//         type: CONFIRMED_DELETE_POST_ACTION,
//         payload: postId,
//     };
// }
