import { createSlice } from '@reduxjs/toolkit';

const initialState = {

    patientInfo: localStorage.getItem('physioInfo') ? JSON.parse(localStorage.getItem('physioInfo')) : null,

}

const physioSlice = createSlice({
    name: 'physios',
    initialState,
    reducers: {

        setPatients: (state, action) => {
            state.userToken = action.payload;
            localStorage.setItem('physioInfo', JSON.stringify(action.payload))
        },

    }
})

export const { setPhysios } = physioSlice.actions;

export default physioSlice.reducer;