import Hero from "../../components/Hero"
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';

const HomeScreen = () => {


    const { adminInfo } = useSelector((state) => state.auth);
    const { userInfo } = useSelector((state) => state.auth);
    const navigate = useNavigate();



    useEffect(() => {

        if (adminInfo) {
            navigate('/admin/home')
        }
        if (userInfo == null) {
            navigate('/Login');
        }

    }, navigate, userInfo, adminInfo);


    return (
        <>
            <Hero />
        </>
    )
}

export default HomeScreen