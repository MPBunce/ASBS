import { createSlice } from '@reduxjs/toolkit';

const initialState = {

    physioInfo: localStorage.getItem('physioInfo') ? JSON.parse(localStorage.getItem('physioInfo')) : null,

}

const physioSlice = createSlice({
    name: 'physios',
    initialState,
    reducers: {

        setPhysios: (state, action) => {
            state.physioInfo = action.payload;
            localStorage.setItem('physioInfo', JSON.stringify(action.payload))
        },
        clearPhysios: (state, action) => {
            state.physioInfo = null;
            localStorage.setItem('physioInfo', JSON.stringify(null))
        }

    }
})

export const { setPhysios, clearPhysios } = physioSlice.actions;

export default physioSlice.reducer;