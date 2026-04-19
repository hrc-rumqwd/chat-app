import App from "./App";
import Home from "./pages/home";
import Login from "./pages/login";
import SignUp from "./pages/sign-up";

import { Routes, Route } from "react-router-dom";
import ProtectedRoutes from "./utils/protected-routes";

const ReactRouters = () => {
    return (
        <>
            <Routes>
                <Route element={ <ProtectedRoutes/> }>
                    <Route path="/" element={ <Home /> } />
                </Route>
                <Route path="/Login" element={ <Login /> } />
                <Route path="/SignUp" element={ <SignUp /> } />
            </Routes>
        </>
    )
}

export default ReactRouters;