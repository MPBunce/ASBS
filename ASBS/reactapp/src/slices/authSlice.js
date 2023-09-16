import { createSlice } from '@reduxjs/toolkit';

const initialState = {

    userInfo: localStorage.getItem('userInfo') ? JSON.parse(localStorage.getItem('userInfo')) : null,
    userToken: null,

}

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        setToken: (state, action) => {
            state.userToken = action.payload;
            localStorage.setItem('userToken', JSON.stringify(action.payload))
        },
        setCredentials: (state, action) => {
            state.userInfo = action.payload;
            localStorage.setItem('userInfo', JSON.stringify(action.payload))
        },
        logout: (state, action) => {
            state.userInfo = null;
            localStorage.setItem('userInfo');
        },
    }
})

export const { setCredentials, logout, setToken } = authSlice.actions;

export default authSlice.reducer;