import { Container, Card, Button } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import 'react-datetime-picker/dist/DateTimePicker.css';
import 'react-calendar/dist/Calendar.css';
import 'react-clock/dist/Clock.css';

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

    const [phys, setPhys] = useState("")
    const [date, setDate] = useState(new Date());

    //Closed on weekends
    const [weekend, setWeekend] = useState(false);
    useEffect(() => {

        if (date.getDay() == 6 || date.getDay() == 0) {
            setWeekend(true)
        } else {
            setWeekend(false)
        }

    }, [date]);

    const updateHours = (hour) => {
        const newDate = new Date(date); // Create a new Date object
        newDate.setHours(hour); // Update the hour in the new object
        setDate(newDate); // Set the new object as the state
    }

    const submitHandler = (e) => {
        console.log("submit")
    }

    return (

        <Container>


            <div className="d-flex my-5 flex-column">
                <div className="d-flex justify-content-center">
                    <Calendar onChange={setDate} value={date} minDate={new Date()}  />
                </div>
                <div className="">
                    <select className="form-select my-2" aria-label="physio" onChange={(e) => setPhys(e.target.value)}>
                        <option hidden selected disabled>Select physio</option>

                        {

                                physioInfo?.map((physioInfo) => (
                                    <option key={physioInfo.physiotherapistId}>

                                        {physioInfo.firstName} {physioInfo.lastName}

                                    </option>
                                ))
                        }

                    </select>
                </div>
            </div> 



            <h6 className="">Available Times</h6>



            <div className="card-body my-4">

                {weekend ?
                    (
                        <> 
                            <div>Closed on weekends</div>
                        </>            
                    ) : (
                        <>
                    
                            <div className="row my-2">
                                <div className="col">
                                    <Button onClick={() => updateHours(9)} className="btn-lg w-75  btn-info">9:00 AM</Button>
                                </div>
                                <div className="col">
                                    <Button onClick={() => updateHours(10)} className="btn-lg w-75  btn-info">10:00 AM</Button>
                                </div>
                                <div className="col">
                                    <Button onClick={() => updateHours(11)} className="btn-lg w-75  btn-info">11:00 AM</Button>
                                </div>
                                <div className="col">
                                    <Button onClick={() => updateHours(12)} className="btn-lg w-75  btn-info">12:00 PM</Button>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <Button onClick={() => updateHours(13)} className="btn-lg w-75  btn-info">1:00 PM</Button>
                                </div>
                                <div className="col">
                                    <Button onClick={() => updateHours(14)} className="btn-lg w-75  btn-info">2:00 PM</Button>
                                </div>
                                <div className="col">
                                    <Button onClick={() => updateHours(15)} className="btn-lg w-75  btn-info">3:00 PM</Button>
                                </div>
                                <div className="col">
                                    <Button onClick={() => updateHours(16)} className="btn-lg w-75  btn-info">4:00 PM</Button>
                                </div>
                            </div>     
                        
                        </>

                    )
                }

            </div>

            {date && phys ?
                (
                    <div className="row">
                        <div className="col">
                            Please confirm your appointment with {phys} on {date.toDateString()} at {date.getHours() }:00
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