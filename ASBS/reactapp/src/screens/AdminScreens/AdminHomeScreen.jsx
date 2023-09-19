import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Form, Button, Row, Col } from 'react-bootstrap';
import FormContainer from '../../components/FormContainer';
import { useDispatch, useSelector } from 'react-redux';
import { useAdminLoginMutation, useGetAdminDataMutation } from '../../slices/userApiSlice';
import { setAdminToken, setAdminCredentials } from '../../slices/authSlice';
import { toast } from 'react-toastify';
import Loader from '../../components/Loader';

const AdminHomeScreen = () => {

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [email, setEmail] = useState();
    const [password, setpassword] = useState();

    const [adminLogin, { isLoadingLogin }] = useAdminLoginMutation();
    const [getAdminData, { isLoadingUserData }] = useGetAdminDataMutation();

    const { adminInfo } = useSelector((state) => state.auth);



    useEffect(() => {
        if (adminInfo == null) {
            navigate('/');
        }
    }, navigate, adminInfo);

    const submitHandler = async (e) => {
        e.preventDefault();
        try {

            console.log("Submitted ")

            const res = await adminLogin({ email, password }).unwrap()
            dispatch(setAdminToken(res.token))

            const userData = await getAdminData().unwrap();
            dispatch(setAdminCredentials(userData))
            navigate('/')

        } catch (error) {
            toast.error(error.data || error.error)
            console.log(error)
        }
    }

    return (

        <>
            home screen
        </>


    )
}

export default AdminHomeScreen