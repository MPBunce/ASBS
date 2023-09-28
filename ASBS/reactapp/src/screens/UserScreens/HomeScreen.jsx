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
        todaysDate.setDate(todaysDate.getDate() + 1)
        todaysDate.setHours(0);
        todaysDate.setMinutes(0);
        todaysDate.setSeconds(0);

        //console.log(appointment.appointmentDateTime)
        var testDate = new Date(appointment.appointmentDateTime);
        if (testDate.getTime() > todaysDate.getTime()) {
            return appointment
        }
        
    }

    const upcomingAppointments = userInfo.appointments.filter(checkUpcoming).sort(function (a, b) { return new Date(a.appointmentDateTime) - new Date(b.appointmentDateTime) })

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

    const pastAppointments = userInfo.appointments.filter(checkPast).sort(function (a, b) { return new Date(a.appointmentDateTime) - new Date(b.appointmentDateTime) });

    function checkToday(appointment) {
        const todaysDate = new Date();
        todaysDate.setHours(0);
        todaysDate.setMinutes(0);
        todaysDate.setSeconds(0);

        const futureDate = new Date();
        futureDate.setDate(futureDate.getDate() + 1)
        futureDate.setHours(0);
        futureDate.setMinutes(0);
        futureDate.setSeconds(0);

        //console.log(appointment.appointmentDateTime)
        var testDate = new Date(appointment.appointmentDateTime);
        if (testDate.getTime() > todaysDate.getTime() && testDate.getTime() < futureDate.getTime()) {
            return appointment
        }

    }

    const todayAppointments = userInfo.appointments.filter(checkToday).sort(function (a, b) { return new Date(a.appointmentDateTime) - new Date(b.appointmentDateTime) });


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

            <h6 className="mb-4">Todays Appointments</h6>

            {
                todayAppointments.map((appointment) => (

                    <div key={appointment.appointmentId} className="card my-2">
                        <div className="card-body">
                            <h5 className="card-title">{appointment.appointmentDateTime}</h5>
                            <h6 className="card-subtitle mb-2 text-muted">{appointment.physiotherapist.firstName}  {appointment.physiotherapist.lastName}</h6>
                        </div>
                    </div>

                ))
            }


            <h6 className="mb-4">Upcoming Appointments</h6>

            {
                upcomingAppointments.map((appointment) => (
                    <div key={appointment.appointmentId} className="card my-2">
                        <div className="card-body">


                            <div className="row mx-md-n5 justify-content-between">
                                <div className="col-9">
                                    <h5 className="card-title">{appointment.appointmentDateTime}</h5>
                                    <h6 className="card-subtitle mb-2 text-muted">{appointment.physiotherapist.firstName}  {appointment.physiotherapist.lastName}</h6>
                                </div>
                                <div className="col-2">
                                    <Button className="btn-danger">Delete</Button>
                                </div>
                            </div>

                        </div>
                    </div>
                ))
            }


            <h6 className="mb-4">Past Appointments</h6>

            {
                pastAppointments.map((appointment) => (
                    <div key={appointment.appointmentId} className="card my-2">
                        <div className="card-body">
                            <div className="row mx-md-n5 justify-content-between">
                                <div className="col-9">
                                    <h5 className="card-title">{appointment.appointmentDateTime}</h5>
                                    <h6 className="card-subtitle mb-2 text-muted">{appointment.physiotherapist.firstName}  {appointment.physiotherapist.lastName}</h6>
                                </div>
                                <div className="col-2">
                                    <Button className="btn-info">Notes</Button>
                                </div>
                            </div>
                        </div>
                    </div>
                ))
            }


        </Container>
    )
}

export default HomeScreen