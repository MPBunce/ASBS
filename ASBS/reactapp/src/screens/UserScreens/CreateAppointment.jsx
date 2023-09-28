import { Container, Card, Button } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import 'react-datetime-picker/dist/DateTimePicker.css';
import 'react-calendar/dist/Calendar.css';
import 'react-clock/dist/Clock.css';
import { useUserCreateAppointmentMutation } from '../../slices/userApiSlice';
import Calendar from 'react-calendar';

const CreateAppointment = () => {

    const { adminInfo } = useSelector((state) => state.auth);
    const { userInfo } = useSelector((state) => state.auth);
    const { patientInfo } = useSelector((state) => state.patients);
    const { physioInfo } = useSelector((state) => state.physio);

    const navigate = useNavigate();
    const dispatch = useDispatch();

    useEffect(() => {

        if (adminInfo) {
            navigate('/admin/home')
        }
        if (userInfo == null) {
            navigate('/Login');
        }

    }, [navigate, userInfo, adminInfo, physioInfo, patientInfo]);

    const [phys, setPhys] = useState(null)

    var minDate = new Date();
    minDate.setDate(minDate.getDate() + 1)    
    minDate.setHours(0)
    minDate.setMinutes(0)
    minDate.setSeconds(0)
    const [date, setDate] = useState(minDate);

    //Closed on weekends
    const [weekend, setWeekend] = useState(false);
    useEffect(() => {

        if (date.getDay() == 6 || date.getDay() == 0) {
            setWeekend(true)
        } else {
            setWeekend(false)
        }
        console.log(date)
    }, [date]);

    const updateHours = (hour) => {
        const newDate = new Date(date); // Create a new Date object
        newDate.setHours(hour); // Update the hour in the new object
        setDate(newDate); // Set the new object as the state
        console.log(newDate)
        setDate(newDate)
        
    }

    const [create, { isLoadingCreate }] = useUserCreateAppointmentMutation();

    const submitHandler = async (e) => {


        const appointmentId = "";
        const physiotherapist = {
            "physiotherapistId": "string",
            "firstName": "string",
            "lastName": "string",
            "contactNumber": "string",
            "email": "string",
            "password": "string",
            "specialization": "string"
        }

        const appointmentDateTime = date
        const duration = 60;
        const notes = ""

        var info = { appointmentId, physiotherapist, appointmentDateTime, duration, notes }
        var data = { phys, info }
        console.log(data)
        try {
            const res = await create(data).unwrap()
            console.log(res)
            navigate('/')

        } catch (error) {
            console.log(error)
        }

    }

    const [nine, setNine] = useState(false)
    const [ten, setTen] = useState(false)
    const [eleven, setEleven] = useState(false)
    const [twelve, setTwelve] = useState(false)

    const [thirteen, setThirteen] = useState(false)
    const [fourteen, setFourteen] = useState(false)
    const [fifteen, setFifteen] = useState(false)
    const [sixteen, setSixteen] = useState(false) 

    return (

        <Container>

            <div className="d-flex my-5 flex-column">
                <div className="d-flex justify-content-center">

                    <Calendar onChange={setDate} value={date} minDate={minDate} />
                </div>
                <div className="">
                    <select className="form-select my-2" aria-label="physio" onChange={(e) => setPhys(e.target.value)}>
                        <option hidden selected disabled>Select physio</option>

                        {

                            physioInfo?.map((physioInfo) => (
                                <option key={physioInfo.physiotherapistId} value={physioInfo.physiotherapistId}>

                                    {physioInfo.firstName} {physioInfo.lastName}

                                </option>
                            ))
                        }

                    </select>
                </div>
            </div> 

            <h6 className="">Available Times</h6>

            <div className="card-body my-4">

                {weekend ? (<div>Closed on weekends</div>) : null}
                {!weekend && !phys? (<div>Select Physio</div>) : null}

                {weekend || !phys ?
                    (
                        null           
                    ) : (

                        <>
                    
                            <div className="row my-2">
                                <div className="col">
                                    <Button disabled={nine} onClick={() => updateHours(9)} className="btn-lg w-75  btn-dark">9:00 AM</Button>
                                </div>
                                <div className="col">
                                    <Button disabled={ten} onClick={() => updateHours(10)} className="btn-lg w-75  btn-dark">10:00 AM</Button>
                                </div>
                                <div className="col">
                                    <Button disabled={eleven} onClick={() => updateHours(11)} className="btn-lg w-75  btn-dark">11:00 AM</Button>
                                </div>
                                <div className="col">
                                    <Button disabled={twelve} onClick={() => updateHours(12)} className="btn-lg w-75  btn-dark">12:00 PM</Button>
                                </div>
                            </div>
                            <div className="row my-2">
                                <div className="col">
                                    <Button disabled={thirteen} onClick={() => updateHours(13)} className="btn-lg w-75  btn-dark">1:00 PM</Button>
                                </div>
                                <div className="col">
                                    <Button disabled={fourteen} onClick={() => updateHours(14)} className="btn-lg w-75  btn-dark">2:00 PM</Button>
                                </div>
                                <div className="col">
                                    <Button disabled={fifteen} onClick={() => updateHours(15)} className="btn-lg w-75  btn-dark">3:00 PM</Button>
                                </div>
                                <div className="col">
                                    <Button disabled={sixteen} onClick={() => updateHours(16)} className="btn-lg w-75  btn-dark">4:00 PM</Button>
                                </div>
                            </div>     
                        
                        </>

                    )
                }

            </div>

            {date && phys && !weekend && date.getHours() >= 9 ?
                (
                    
                    <div className="row pt-4">
                        <div className="col">
                            Please confirm your appointment on {date.toDateString()} at {date.getHours() }:00
                        </div>
                        <div className="col">
                            <Button onClick={() => submitHandler()} className="btn-lg w-50">Confirm</Button>
                        </div>
                    </div> 
                ) : null
            }

        </Container>
    )
}

export default CreateAppointment