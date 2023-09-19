import { configureStore } from '@reduxjs/toolkit';
import { getDefaultMiddleware } from '../node_modules/@reduxjs/toolkit/dist/getDefaultMiddleware';
import authReducer from './slices/authSlice';
import { apiSlice } from './slices/apiSlice';
import patientsReducer  from './slices/patientSlice';
import physioReducer from './slices/physioSlice';

const store = configureStore({
    reducer: {
        auth: authReducer,
        patients: patientsReducer,
        physio: physioReducer,
        [apiSlice.reducerPath]: apiSlice.reducer
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(apiSlice.middleware),
    devTools: true
})

export default store;