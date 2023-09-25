import { createSlice } from '@reduxjs/toolkit';

const initialState = {

    userInfo: localStorage.getItem('userInfo') ? JSON.parse(localStorage.getItem('userInfo')) : null,
    userToken: localStorage.getItem('userToken') ? JSON.parse(localStorage.getItem('userToken')) : null,

    adminInfo: localStorage.getItem('adminInfo') ? JSON.parse(localStorage.getItem('adminInfo')) : null,
    adminToken: localStorage.getItem('adminToken') ? JSON.parse(localStorage.getItem('adminToken')) : null,

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
            localStorage.setItem('userInfo', JSON.stringify(null));
            state.userToken = null;
            localStorage.setItem('userToken', JSON.stringify(null));
        },
        addAppointment: (state, action) => {
            state.userInfo.appointments.push(action.payload);
            localStorage.setItem('userInfo', JSON.stringify(state.userInfo))
        },
        setAdminToken: (state, action) => {
            state.adminToken = action.payload;
            localStorage.setItem('adminToken', JSON.stringify(action.payload))
        },
        setAdminCredentials: (state, action) => {
            state.adminInfo = action.payload;
            localStorage.setItem('adminInfo', JSON.stringify(action.payload))
        },
        logoutAdmin: (state, action) => {
            state.adminInfo = null;
            localStorage.setItem('adminInfo', JSON.stringify(null));
            state.adminToken = null;
            localStorage.setItem('adminToken', JSON.stringify(null));
        },
    }
})

export const { setCredentials, logout, setToken, setAdminToken, setAdminCredentials, logoutAdmin } = authSlice.actions;

export default authSlice.reducer;