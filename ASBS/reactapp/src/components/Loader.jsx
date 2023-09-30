import { Spinner } from "react-bootstrap";

const Loader = () => {
    return (
        <div className="my-4">
        
            <Spinner
                animation="border"
                role="status"
                style={{
                    width: '100px',
                    height: '100px',
                    margin: 'auto',
                    display: 'block',
                }}
            >
            </Spinner>        

            <h1 className="my-4 text-center">Loading . . . . .</h1>
        </div>

    )
}

export default Loader;