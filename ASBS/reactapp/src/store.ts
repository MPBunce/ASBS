import { configureStore } from '@reduxjs/toolkit';
import { getDefaultMiddleware } from '../node_modules/@reduxjs/toolkit/dist/getDefaultMiddleware';
import authReducer from './slices/authSlice';
import { apiSlice } from './slices/apiSlice';


const store = configureStore({
    reducer: {
        auth: authReducer,
        [apiSlice.reducerPath]: apiSlice.reducer
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(apiSlice.middleware),
    devTools: true
})

export default store;