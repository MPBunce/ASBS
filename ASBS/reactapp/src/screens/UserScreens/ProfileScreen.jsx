import Hero from "../../components/Hero"
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';

const ProfileScreen = () => {

    const { userInfo } = useSelector((state) => state.auth);
    const navigate = useNavigate();

    useEffect(() => {
        if (userInfo == null) {
            navigate('/Login');
        }
    }, navigate, userInfo);


    return (
        <>
            <div>Profile screen</div>

            {userInfo.firstName }
        </>
    )
}

export default ProfileScreen