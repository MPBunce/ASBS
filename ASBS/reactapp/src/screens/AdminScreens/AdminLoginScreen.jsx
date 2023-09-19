import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Form, Button, Row, Col } from 'react-bootstrap';
import FormContainer from '../../components/FormContainer';
import { useDispatch, useSelector } from 'react-redux';
import { useAdminLoginMutation, useGetAdminDataMutation } from '../../slices/userApiSlice';
import { setAdminToken, setAdminCredentials } from '../../slices/authSlice';
import { toast } from 'react-toastify';
import Loader from '../../components/Loader';

const AdminLoginScreen = () => {

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [email, setEmail] = useState();
    const [password, setpassword] = useState();

    const [adminLogin, { isLoadingLogin }] = useAdminLoginMutation();
    const [getAdminData, { isLoadingUserData }] = useGetAdminDataMutation();

    const { adminInfo } = useSelector((state) => state.auth);



    useEffect(() => {
        if (adminInfo) {
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
            console.log(userData)
            dispatch(setAdminCredentials(userData))
            navigate('/admin/home')

        } catch (error) {
            toast.error(error.data || error.error)
            console.log(error)
        }
    }

    return (

        <FormContainer>

            <h1>Sign In</h1>

            <Form onSubmit={submitHandler}>

                <Form.Group className='my-2' controlId='email'>
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                        type='email'
                        placeholder='Enter Email'
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    >

                    </Form.Control>
                </Form.Group>

                <Form.Group className='my-2' controlId='password'>
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type='password'
                        placeholder='Enter Password'
                        value={password}
                        onChange={(e) => setpassword(e.target.value)}
                    >

                    </Form.Control>
                </Form.Group>

                {isLoadingLogin && isLoadingUserData && <Loader />}

                <Button className='mt-3' type='submit' variant='primary'>
                    Sign In
                </Button>

            </Form>
        </FormContainer>


    )
}

export default AdminLoginScreen