import {
    CONFIRMED_CREATE_POST_ACTION,
    CONFIRMED_DELETE_POST_ACTION,
    CONFIRMED_EDIT_POST_ACTION,
    CONFIRMED_GET_POSTS,
    CONFIRMED_UPDATE_STATUS_POST,
    CREATE_POST_ACTION,
    FAILED_GET_POSTS_ACTION
} from '../actions/PostTypes';

const initialState = {
    posts: [],
    totalSize: 0,
    pageNumber: 0,
    status: 'idle',
    error: null,
    action: ''
};

export default function PostsReducer(state = initialState, actions) {
    if (actions.type === CREATE_POST_ACTION) {
        const post = {
            id: Math.random(),
            title: 'Post Title 2asdasd',
            description: 'Sample Description 2asdasdas',
        };

        const posts = [...state.posts];
        const totalSize = state.totalSize + 1;
        posts.push(post);
        return {
            ...state,
            posts,
            totalSize
        };
    }

    if (actions.type === CONFIRMED_DELETE_POST_ACTION) {
        const posts = [...state.posts];
        const postIndex = posts.findIndex(
            (post) => post.id === actions.payload,
        );

        posts.splice(postIndex, 1);
        const totalSize = state.totalSize - 1;

        return {
            ...state,
            posts,
            totalSize
        };
    }

    if (actions.type === CONFIRMED_EDIT_POST_ACTION) {
        const posts = [...state.posts];
        const post = posts.find(
            (post) => post.id === actions.payload.id,
        );

        post.statusID = actions.payload.statusID
        return {
            ...state,
            posts,
        };
    }

    if (actions.type === CONFIRMED_CREATE_POST_ACTION) {
        const posts = [...state.posts];
        posts.push(actions.payload);

        return {
            ...state,
            posts,
        };
    }

    if (actions.type === CONFIRMED_GET_POSTS) {
        //console.log(actions.payload);
        return {
            ...state,
            ...actions.payload,
            status: 'succeeded'
        };
    }

    if (actions.type === CONFIRMED_UPDATE_STATUS_POST) {
        const posts = [...state.posts];
        const post = posts.find(
            (post) => post.id === actions.payload.id,
        );

        post.statusID = actions.payload.statusID
        return {
            ...state,
            posts,
        };
    }

    if (actions.type === FAILED_GET_POSTS_ACTION) {
        return {
            ...state,
            error: actions.payload,
            status: 'failed'
        };
    }
    return state;
}
