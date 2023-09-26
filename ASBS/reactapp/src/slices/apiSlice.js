import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';


export const apiSlice = createApi({
    baseQuery: fetchBaseQuery({
        baseUrl: '',
        prepareHeaders: (headers, { getState }) => {
            const token = getState().auth.userToken
            if (token) {
                console.log(token)
                // include token in req header
                headers.set('authorization', `Bearer ${token}`)
                headers.set('Content-Type', 'application/json')
                return headers
            }
            const adminToken = getState().auth.adminToken
            if (adminToken) {
                console.log(adminToken)
                // include token in req header
                headers.set('authorization', `Bearer ${adminToken}`)
                headers.set('Content-Type', 'application/json');
                return headers
            }
        },
    }),
    tagTypes: ['User'],
    endpoints: (builder) => ({

    }),
})