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
        updatePatient: (state, action) => {
            const updated = JSON.parse(JSON.stringify(action.payload));
            const index = state.patientInfo.findIndex((patient) => patient.patientId === updated.patientId);

            // Add additional check for index value
            if (index !== -1) {
                state.patientInfo[index] = updated;
                console.log(state.patientInfo)

                localStorage.setItem('patientInfo', JSON.stringify(state.patientInfo));
            } else {
                console.log("Error");
            }

        }

    }
})

export const { setPatients, clearPatients, updatePatient } = patientsSlice.actions;

export default patientsSlice.reducer;