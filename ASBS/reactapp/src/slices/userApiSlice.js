import { apiSlice } from './apiSlice';


const USERS_URL = 'Patient';

export const usersApiSlice = apiSlice.injectEndpoints({
    prepareHeaders: (headers, { getState }) => {
        const token = getState().auth.userToken
        if (token) {
            console.log(token)
            // include token in req header
            headers.set('authorization', `Bearer ${token}`)
            return headers
        }
    },
    endpoints: (builder) => ({
        login: builder.mutation({
            query: (data) => ({
                url: `${USERS_URL}/Login`,
                method: 'POST',
                body: data
            })
        }),
        getUserData: builder.mutation({
            query: () => ({
                url: `${USERS_URL}/GetUserAndAppointment`,
                method: 'GET',
            })
        })
    })
})

export const {
    useLoginMutation,
    useGetUserDataMutation,
} = usersApiSlice