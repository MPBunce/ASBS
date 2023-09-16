import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Form, Button, Row, Col } from 'react-bootstrap';
import FormContainer from '../components/FormContainer';


const RegisterScreen = () => {

    const [firstName, setFirstName] = useState();
    const [lastName, setLastName] = useState();
    const [phone, setPhone] = useState();

    const [email, setEmail] = useState();
    const [pwd, setPwd] = useState();
    const [pwdCheck, setPwdCheck] = useState();

    const submitHandler = async (e) => {
        e.preventDefault();
        console.log("submit")
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
                        value={phone}
                        onChange={(e) => setPhone(e.target.value)}
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
                        value={pwd}
                        onChange={(e) => setPwd(e.target.value)}
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