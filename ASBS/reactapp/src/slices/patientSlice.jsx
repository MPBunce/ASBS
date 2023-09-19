import { createSlice } from '@reduxjs/toolkit';

const initialState = {

    patientInfo: localStorage.getItem('patientInfo') ? JSON.parse(localStorage.getItem('patientInfo')) : null,

}

const patientsSlice = createSlice({
    name: 'patients',
    initialState,
    reducers: {

        setPatients: (state, action) => {
            state.userToken = action.payload;
            localStorage.setItem('patientInfo', JSON.stringify(action.payload))
        },

    }
})

export const { setPatients } = patientsSlice.actions;

export default patientsSlice.reducer;