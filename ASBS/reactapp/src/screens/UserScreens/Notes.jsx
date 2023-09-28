import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Navbar, Nav, Container, NavDropdown } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import { FaArrowLeft } from 'react-icons/fa';
import { LinkContainer } from 'react-router-bootstrap';

const NotesScreen = () => {

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [email, setEmail] = useState();
    const [password, setpassword] = useState();

    const { userInfo } = useSelector((state) => state.auth);
    const { adminInfo } = useSelector((state) => state.auth);

    const { appId } = useParams();

    useEffect(() => {
        if (adminInfo) {
            navigate('/admin/home')
        }

        if (!userInfo) {
            navigate('/login');
        }
        
    }, navigate, userInfo);


    const specificAppointment = userInfo.appointments.filter(appointment => appointment.appointmentId === appId)

    console.log(specificAppointment)

    return (

        <Container>
            <div>

                <LinkContainer to='/'>
                    <Nav.Link>

                        <FaArrowLeft /> Back
                    </Nav.Link>
                </LinkContainer>


            </div>
       
            <div className="card my-4">

                <div className="card-body">
                    <h2 className="card-title">Physiotherapist: {specificAppointment[0]?.physiotherapist.firstName} {specificAppointment[0]?.physiotherapist.lastName}</h2>
                    <h3 className="card-title">Specialization: {specificAppointment[0]?.physiotherapist.specialization}</h3>
                    <h5 className="card-title">Date: {specificAppointment[0]?.appointmentDateTime}</h5>
                    <h6 className="card-title">Duration: {specificAppointment[0]?.duration} Minutes</h6>


                    <h6 className="my-4">Notes:</h6>
                    <p classNames="card-text">{specificAppointment[0]?.physiotherapist.notes}</p>
                </div>
            </div>

        </Container>



    )
}

export default NotesScreen