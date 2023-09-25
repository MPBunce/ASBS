import { apiSlice } from './apiSlice';


const USERS_URL = 'Patient';
const ADMINS_URL = 'Physiotherapist';


export const usersApiSlice = apiSlice.injectEndpoints({
    endpoints: (builder) => ({

        //User  Endpoints

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
        }),
        register: builder.mutation({
            query: (data) => ({
                url: `${USERS_URL}/RegisterPatient`,
                method: 'POST',
                body: data
            })
        }),
        getAllPatients: builder.mutation({
            query: () => ({
                url: `${USERS_URL}/GetAllPatients`,
                method: 'GET',
            })
        }),
        userCreateAppointment: builder.mutation({
            query: (physioId, data) => ({
                url: `${USERS_URL}/CreateAppointment?physioId=${physioId}`,
                method: 'POST',
                body: data,
            })
        }),

        //Admin Endpoints

        adminLogin: builder.mutation({
            query: (data) => ({
                url: `${ADMINS_URL}/Login`,
                method: 'POST',
                body: data
            })
        }),
        getAdminData: builder.mutation({
            query: () => ({
                url: `${ADMINS_URL}/GetOnePhysiotherapists`,
                method: 'GET',
            })
        }),
        getAllPhysiotherapists: builder.mutation({
            query: () => ({
                url: `${ADMINS_URL}/GetAllPhysiotherapists`,
                method: 'GET',
            })
        }),
    })
})

export const {
    useLoginMutation,
    useGetUserDataMutation,
    useRegisterMutation,
    useUserCreateAppointmentMutation,
    useAdminLoginMutation,
    useGetAdminDataMutation,
    useGetAllPhysiotherapistsMutation,
    useGetAllPatientsMutation
} = usersApiSlice