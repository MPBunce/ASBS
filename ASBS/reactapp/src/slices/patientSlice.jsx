import { createSlice } from '@reduxjs/toolkit';

const initialState = {

    patientInfo: localStorage.getItem('patientInfo') ? JSON.parse(localStorage.getItem('patientInfo')) : null,

}

const patientsSlice = createSlice({
    name: 'patients',
    initialState,
    reducers: {

        setPatients: (state, action) => {
            state.patientInfo = action.payload;
            localStorage.setItem('patientInfo', JSON.stringify(action.payload))
        },
        clearPatients: (state, action) => {
            state.patientInfo = null;
            localStorage.setItem('patientInfo', JSON.stringify(null));
        },

    }
})

export const { setPatients, clearPatients } = patientsSlice.actions;

export default patientsSlice.reducer;