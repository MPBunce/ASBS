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


    var dateVariable = Date().toLocaleString()


    useEffect(() => {

        if (adminInfo) {
            navigate('/admin/home')
        }
        if (userInfo == null) {
            navigate('/Login');
        }
        getData()
        setUserData(userInfo)
        console.log(dateVariable)
    }, navigate, userInfo, adminInfo, patients, physio);



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



            <h6 className="mb-4">Past Appointments</h6>




        </Container>
    )
}

export default HomeScreen