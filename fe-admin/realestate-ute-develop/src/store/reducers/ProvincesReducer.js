import {
    CONFIRMED_GET_PROVINCES,
    FAILED_GET_PROVINCES_ACTION
} from '../actions/ProvinceActions';

const initialState = {
    provinces: [],
    status: 'idle',
    error: null
};

export default function ProvincesReducer(state = initialState, actions) {

    if (actions.type === CONFIRMED_GET_PROVINCES) {
        console.log(actions.payload);
        return {
            ...state,
            provinces: actions.payload,
            error: null,
            status: 'succeeded'
        };
    }

    if (actions.type === FAILED_GET_PROVINCES_ACTION) {
        return {
            ...state,
            error: actions.payload,
            status: 'failed'
        };
    }

    return state;
}
