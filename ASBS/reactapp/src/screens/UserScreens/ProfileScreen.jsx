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






            <div className="card">
                    <div className="card-body">
                        <h2 className="card-title my-4">User Info</h2>
                        <h6>{userInfo.firstName}</h6>
                        <h6>{userInfo.lastName}</h6>
                        <h6>{userInfo.phoneNumber}</h6>
                        <h6>{userInfo.email}</h6>
                        <a href="#" className="btn btn-primary my-4">Update user info</a>
                    </div>
            </div>

        </>
    )
}

export default ProfileScreen