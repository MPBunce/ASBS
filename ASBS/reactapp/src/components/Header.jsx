// import { Navbar, Nav, Container, NavDropdown, Badge } from 'react-bootstrap';
import { Navbar, Nav, Container, NavDropdown } from 'react-bootstrap';
import { FaSignInAlt, FaSignOutAlt } from 'react-icons/fa';
import { LinkContainer } from 'react-router-bootstrap';
import { useSelector, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { logout, logoutAdmin } from '../slices/authSlice';
import { clearPatients } from '../slices/patientSlice';
import { clearPhysios } from '../slices/physioSlice';

const Header = () => {

    const { userInfo } = useSelector((state) => state.auth);
    const { adminInfo } = useSelector((state) => state.auth);

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const logoutHandler = () => {
        try {
            dispatch(logout());
            dispatch(clearPatients())
            dispatch(clearPhysios())
            navigate('/login');
        } catch (err) {
            console.error(err);
        }
    };

    const adminLogoutHandler = () => {
        try {
            dispatch(logoutAdmin())
            dispatch(clearPatients())
            dispatch(clearPhysios())
        } catch (err) {
            console.error(err);
            navigate('/admin');
        }
    }

    return (
        <header>
            <Navbar bg='dark' variant='dark' expand='lg' collapseOnSelect>
                <Container>
                    <LinkContainer to='/'>
                        <Navbar.Brand>Fast Physio</Navbar.Brand>
                    </LinkContainer>
                    /<Navbar.Toggle aria-controls='basic-navbar-nav' />
                    <Navbar.Collapse id='basic-navbar-nav'>
                        <Nav className='ms-auto'>
                            {userInfo ? (
                                <>
                                    <NavDropdown title={userInfo.name} id='username'>
                                        <LinkContainer to='/profile'>
                                            <NavDropdown.Item>Profile</NavDropdown.Item>
                                        </LinkContainer>
                                        <NavDropdown.Item onClick={logoutHandler}>
                                            Logout
                                        </NavDropdown.Item>
                                    </NavDropdown>
                                </>
                            ): adminInfo ? (
                                <>
                                    <NavDropdown title={adminInfo.firstName} id='username'>
                                        <LinkContainer to='/profile'>
                                            <NavDropdown.Item>Admin Profile</NavDropdown.Item>
                                        </LinkContainer>
                                        <NavDropdown.Item onClick={adminLogoutHandler}>
                                            Admin Logout
                                        </NavDropdown.Item>
                                    </NavDropdown>
                            </>
                            ) : (
                                <>
                                    <LinkContainer to='/login'>
                                        <Nav.Link>
                                            <FaSignInAlt /> Sign In
                                        </Nav.Link>
                                    </LinkContainer>
                                    <LinkContainer to='/register'>
                                        <Nav.Link>
                                            <FaSignOutAlt /> Sign Up
                                        </Nav.Link>
                                    </LinkContainer>
                                </>
                            )}
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </header>
    );
};

export default Header;