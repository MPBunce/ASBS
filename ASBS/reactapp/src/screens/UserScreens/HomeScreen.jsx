import Hero from "../../components/Hero"
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import {useGetAllPhysiotherapistsMutation, useGetAllPatientsMutation } from '../../slices/userApiSlice';
import { setPatients } from '../../slices/patientSlice';
import { setPhysios } from '../../slices/physioSlice';
import { useDispatch } from 'react-redux';

import { Container, Card, Button } from 'react-bootstrap';
import { Navbar, Nav, NavDropdown } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import { FaPlus, FaRegHandPointRight } from 'react-icons/fa';

const HomeScreen = () => {

    const { adminInfo } = useSelector((state) => state.auth);
    const { userInfo } = useSelector((state) => state.auth);
    const { patients } = useSelector((state) => state.patients);
    const { physio } = useSelector((state) => state.physio);

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [getPhysios, loadingPhysios] = useGetAllPhysiotherapistsMutation();
    const [getPatients, loadingPatients] = useGetAllPatientsMutation();

    const [userData, setUserData] = useState([{ firstName: '', lastName: '', appointments: null }]);

    const getData = async () => {

        console.log("here")

        try {

            const newPatients = await getPatients().unwrap();
            dispatch(setPatients(newPatients))

            const newPhysios = await getPhysios().unwrap();
            dispatch(setPhysios(newPhysios))

        } catch (err) {
            console.log(err)
        }
    }

    useEffect(() => {

        if (adminInfo) {
            navigate('/admin/home')
        }
        if (userInfo == null) {
            navigate('/Login');
        }
        getData()
        setUserData(userInfo)

    }, navigate, userInfo, adminInfo, patients, physio);


    function checkUpcoming(appointment) {
        const todaysDate = new Date();
        todaysDate.setHours(0);
        todaysDate.setMinutes(0);
        todaysDate.setSeconds(0);

        //console.log(appointment.appointmentDateTime)
        var testDate = new Date(appointment.appointmentDateTime);
        if (testDate.getTime() > todaysDate.getTime()) {
            return appointment
        }
        
    }

    const upcomingAppointments = userInfo.appointments.filter(checkUpcoming);

    function checkPast(appointment) {
        const todaysDate = new Date();
        todaysDate.setHours(0);
        todaysDate.setMinutes(0);
        todaysDate.setSeconds(0);

        //console.log(appointment.appointmentDateTime)
        var testDate = new Date(appointment.appointmentDateTime);
        if (testDate.getTime() < todaysDate.getTime()) {
            return appointment
        }
            
    }

    const pastAppointments = userInfo.appointments.filter(checkPast);

    return (
        <Container>

            <div className="d-flex justify-content-between my-5">
                <div className="flex">
                    <h1>Welcome {userData.firstName} { userData.lastName}</h1>
                </div>
                <div className="flex mt-3">
                    <LinkContainer className="pt-80" to='/create'>
                        <Nav.Link >
                            New Appointment < FaPlus />
                        </Nav.Link>
                    </LinkContainer>
                </div>
            </div>

            <h6 className="mb-4">Upcoming Appointments</h6>

            {
                upcomingAppointments.map((appointment) => (
                <div key={appointment.appointmentId}>
                    <p>Appointment ID: {appointment.appointmentId}</p>
                    <p>Appointment Date: {appointment.appointmentDateTime}</p>
                     Add more appointment details here 
                </div>
                ))
            }

            <h6 className="mb-4">Past Appointments</h6>

            {
                pastAppointments.map((appointment) => (
                    <div key={appointment.appointmentId}>
                        <p>Appointment ID: {appointment.appointmentId}</p>
                        <p>Appointment Date: {appointment.appointmentDateTime}</p>
                         Add more appointment details here 
                    </div>
                ))
            }


        </Container>
    )
}

export default HomeScreen