import Hero from "../components/Hero"
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';

const HomeScreen = () => {

    const { userInfo } = useSelector((state) => state.auth);
    const navigate = useNavigate();

    useEffect(() => {
        if (userInfo == null) {
            navigate('/Login');
        }
    }, navigate, userInfo);


    return (
        <>
            <Hero />
        </>
    )
}

export default HomeScreen