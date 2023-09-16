import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';


export const apiSlice = createApi({
    baseQuery: fetchBaseQuery({
        baseUrl: '',
        prepareHeaders: (headers, { getState }) => {
            const token = getState().auth.userToken
            console.log("Token    " + token)
            if (token) {
                console.log(token)
                // include token in req header
                headers.set('authorization', `Bearer ${token}`)
                return headers
            }
        },
    }),
    tagTypes: ['User'],
    endpoints: (builder) => ({

    }),
})