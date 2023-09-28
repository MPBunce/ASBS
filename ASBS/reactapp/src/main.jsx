import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.jsx';
import 'bootstrap/dist/css/bootstrap.min.css';
import store from './store';
import { Provider } from 'react-redux';
import { createBrowserRouter, createRoutesFromElements, Route, RouterProvider } from 'react-router-dom';


//User Screens
import HomeScreen from './screens/UserScreens/HomeScreen.jsx';
import LoginScreen from './screens/UserScreens/LoginScreen.jsx';
import RegisterScreen from './screens/UserScreens/RegisterScreen.jsx';
import ProfileScreen from './screens/UserScreens/ProfileScreen.jsx';
import CreateAppointment from './screens/UserScreens/CreateAppointment.jsx';
import Notes from './screens/UserScreens/Notes.jsx';
//Admin Screens
import AdminLoginScreen from './screens/AdminScreens/AdminLoginScreen.jsx';
import AdminHomeScreen from './screens/AdminScreens/AdminHomeScreen.jsx';



const router = createBrowserRouter(
    createRoutesFromElements(


        <Route path='/' element={<App />}>

            <Route index={true} path='/' element={<HomeScreen />} />
            <Route path='/login' element={<LoginScreen />} />
            <Route path='/register' element={<RegisterScreen />} />
            <Route path='/profile' element={<ProfileScreen />} />
            <Route path='/create' element={< CreateAppointment />} />
            <Route path='/notes/:appId' element={<Notes />}/>

            <Route path='/admin' element={<AdminLoginScreen />} />
            <Route path='/admin/home' element={<AdminHomeScreen />} />

        </Route>


    )
)


ReactDOM.createRoot(document.getElementById('root')).render(
    <Provider store={ store }>
        <React.StrictMode>
            <RouterProvider router={router} />
        </React.StrictMode>    
    </Provider>,
)
