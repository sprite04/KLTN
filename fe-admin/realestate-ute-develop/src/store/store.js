import { applyMiddleware, combineReducers, compose,createStore,} from 'redux';
import PostsReducer from './reducers/PostsReducer';
import ProvincesReducer from './reducers/ProvincesReducer';
import thunk from 'redux-thunk';
import { AuthReducer } from './reducers/AuthReducer';
//import rootReducers from './reducers/Index';
import todoReducers from './reducers/Reducers';
import { reducer as reduxFormReducer } from 'redux-form';
import UsersReducer from './reducers/UsersReducer';

const middleware = applyMiddleware(thunk);

const composeEnhancers =
    window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const reducers = combineReducers({
    users: UsersReducer,
    posts: PostsReducer,
    provinces: ProvincesReducer,
    auth: AuthReducer,
		todoReducers,
	form: reduxFormReducer,	
});

//const store = createStore(rootReducers);

export const store = createStore(reducers,  composeEnhancers(middleware));
