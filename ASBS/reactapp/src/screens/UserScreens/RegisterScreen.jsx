import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Form, Button, Row, Col } from 'react-bootstrap';
import FormContainer from '../../components/FormContainer';
import { useRegisterMutation, useGetUserDataMutation } from '../../slices/userApiSlice';
import { setCredentials, setToken } from '../../slices/authSlice';
import { useDispatch, useSelector } from 'react-redux';
import { toast } from 'react-toastify';
import Loader from '../../components/Loader';


const RegisterScreen = () => {

    const [firstName, setFirstName] = useState();
    const [lastName, setLastName] = useState();
    const [phoneNumber, setPhoneNumber] = useState();

    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [pwdCheck, setPwdCheck] = useState();

    const [register, { isLoadingRegister }] = useRegisterMutation();
    const [getUserData, { isLoadingUserData }] = useGetUserDataMutation();

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const { userInfo } = useSelector((state) => state.auth);
    const { adminInfo } = useSelector((state) => state.auth);

    useEffect(() => {
        if (userInfo) {
            navigate('/');
        }
        if (adminInfo) {
            navigate('/admin/home')
        }
    }, navigate, userInfo, adminInfo);

    const submitHandler = async (e) => {
        e.preventDefault();

        if (password !== pwdCheck) {
            console.log("Passwords dont match");
            return;
        } 

        try {
            const res = await register({ firstName, lastName, phoneNumber, email, password }).unwrap();
            dispatch(setToken(res.token))
            dispatch(setCredentials(res.user))
            navigate('/')

        } catch (error) {
            console.log(error)
            toast.error(error.data|| error.error)
        }

    }


    return (

        <FormContainer>

            <h1>Sign Up</h1>

            <Form onSubmit={submitHandler}>

                <Form.Group className='my-2' controlId='firstName'>
                    <Form.Label>First Name</Form.Label>
                    <Form.Control
                        type='text'
                        placeholder='Enter First Name'
                        value={firstName}
                        onChange={(e) => setFirstName(e.target.value)}
                    >

                    </Form.Control>
                </Form.Group>

                <Form.Group className='my-2' controlId='lastName'>
                    <Form.Label>Last Name</Form.Label>
                    <Form.Control
                        type='text'
                        placeholder='Enter Last Name'
                        value={lastName}
                        onChange={(e) => setLastName(e.target.value)}
                    >

                    </Form.Control>
                </Form.Group>

                <Form.Group className='my-2' controlId='phone'>
                    <Form.Label>Phone Number</Form.Label>
                    <Form.Control
                        type='tel'
                        placeholder='Enter Phone Number'
                        value={phoneNumber}
                        onChange={(e) => setPhoneNumber(e.target.value)}
                    >

                    </Form.Control>
                </Form.Group>

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
                        onChange={(e) => setPassword(e.target.value)}
                    >

                    </Form.Control>
                </Form.Group>

                <Form.Group className='my-2' controlId='passwordCheck'>
                    <Form.Label>Confirm Password</Form.Label>
                    <Form.Control
                        type='password'
                        placeholder='Confirm Password'
                        value={pwdCheck}
                        onChange={(e) => setPwdCheck(e.target.value)}
                    >

                    </Form.Control>
                </Form.Group>

                {isLoadingRegister && isLoadingUserData && <Loader />}

                <Button className='mt-3' type='submit' variant='primary'>
                    Register
                </Button>

                <Row className='py-3'>
                    <Col>
                        Existing Patient? <Link to='/login'>Login</Link>
                    </Col>
                </Row>

            </Form>
        </FormContainer>


    )
}

export default RegisterScreen;