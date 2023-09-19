import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Form, Button, Row, Col } from 'react-bootstrap';
import FormContainer from '../../components/FormContainer';
import { useDispatch, useSelector } from 'react-redux';
import { useLoginMutation, useGetUserDataMutation } from '../../slices/userApiSlice';
import { setCredentials, setToken } from '../../slices/authSlice';
import { toast } from 'react-toastify';
import Loader from '../../components/Loader';

const LoginScreen = () => {

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [email, setEmail] = useState();
    const [password, setpassword] = useState();

    const [login, { isLoadingLogin } ] = useLoginMutation();
    const [getUserData, { isLoadingUserData }] = useGetUserDataMutation();

    const { userInfo } = useSelector((state) => state.auth);
    const { adminInfo } = useSelector((state) => state.auth);



    useEffect(() => {
        if (userInfo) {
            navigate('/');
        }
        if (adminInfo) {
            navigate('/admin/home')
        }
    }, navigate, userInfo);

    const submitHandler = async (e) => {
        e.preventDefault();
        try {

            console.log("Submitted ")

            const res = await login({ email, password }).unwrap()
            dispatch(setToken(res.token))

            const userData = await getUserData().unwrap();
            dispatch(setCredentials(userData))
            navigate('/')

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
                        onChange={ (e) => setEmail(e.target.value)}
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

                <Row className='py-3'>
                    <Col>
                        New Patient? <Link to='/register'>Register</Link>
                    </Col>
                </Row>

                {/*<Row className='py-3'>*/}
                {/*    <Col>*/}
                {/*        Admin? <Link to='/admin'>Admin Portal</Link>*/}
                {/*    </Col>*/}
                {/*</Row>*/}

            </Form>
        </FormContainer>


    )
}

export default LoginScreen