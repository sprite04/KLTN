import {
    CONFIRMED_GET_USERS,
    FAILED_GET_USERS_ACTION,
    CONFIRMED_CREATE_USER_ACTION,
    CONFIRMED_EDIT_USER_ACTION,
    CONFIRMED_DELETE_USER_ACTION,
    CONFIRMED_LOCK_UNLOCK_USER_ACTION

} from '../actions/UserActions';

const initialState = {
    users: [],
    totalSize: 0,
    pageSize: 0,
    pageNumber: 0,
    status: 'idle',
    error: null,
    action: ''
};

export default function UsersReducer(state = initialState, actions) {
    // if (actions.type === CREATE_POST_ACTION) {
    //     const post = {
    //         id: Math.random(),
    //         title: 'Post Title 2asdasd',
    //         description: 'Sample Description 2asdasdas',
    //     };

    //     const posts = [...state.posts];
    //     const totalSize = state.totalSize + 1;
    //     posts.push(post);
    //     return {
    //         ...state,
    //         posts,
    //         totalSize
    //     };
    // }

    // if (actions.type === CONFIRMED_DELETE_POST_ACTION) {
    //     const posts = [...state.posts];
    //     const postIndex = posts.findIndex(
    //         (post) => post.id === actions.payload,
    //     );

    //     posts.splice(postIndex, 1);
    //     const totalSize = state.totalSize - 1;

    //     return {
    //         ...state,
    //         posts,
    //         totalSize
    //     };
    // }

    // if (actions.type === CONFIRMED_EDIT_POST_ACTION) {
    //     const posts = [...state.posts];
    //     const post = posts.find(
    //         (post) => post.id === actions.payload.id,
    //     );

    //     post.statusID = actions.payload.statusID
    //     return {
    //         ...state,
    //         posts,
    //     };
    // }

    // if (actions.type === CONFIRMED_UPDATE_STATUS_POST) {
    //     const posts = [...state.posts];
    //     const post = posts.find(
    //         (post) => post.id === actions.payload.id,
    //     );

    //     post.statusID = actions.payload.statusID
    //     return {
    //         ...state,
    //         posts,
    //     };
    // }

    // if (actions.type === CONFIRMED_CREATE_POST_ACTION) {
    //     const posts = [...state.posts];
    //     posts.push(actions.payload);

    //     return {
    //         ...state,
    //         posts,
    //     };
    // }

    if (actions.type === CONFIRMED_GET_USERS) {
        return {
            ...state,
            ...actions.payload,
            status: 'succeeded'
        };
    }

    if (actions.type === FAILED_GET_USERS_ACTION) {
        return {
            ...state,
            error: actions.payload,
            status: 'failed'
        };
    }

    if (actions.type === CONFIRMED_LOCK_UNLOCK_USER_ACTION) {
        const users = [...state.users];
        const user = users.find(
            (user) => user.id === actions.payload.id,
        );

        user.lock = actions.payload.lock
        return {
            ...state,
            users,
        };
    }


    if (actions.type === CONFIRMED_EDIT_USER_ACTION) {
        const users = [...state.users];
        let user = users.find(
            (user) => user.id === actions.payload.id,
        );

        user = actions.payload
        return {
            ...state,
            users,
        };
    }

    if (actions.type === CONFIRMED_CREATE_USER_ACTION) {
        console.log("teoeteo",state.pageSize)
        const users = [actions.payload, ...state.users];
        if (users.length > state.pageSize)
            users.splice(users.length-1, 1)
        // let user = users.find(
        //     (user) => user.id === actions.payload.id,
        // );
        console.log("user...", users)

        // user = actions.payload
        return {
            ...state,
            users,
        };
    }

    return state;
}