import { Navigate, Outlet } from "react-router-dom";
import { AuthService } from "../services/auth-service";

const ProtectedRoutes = () => {
    let isAuthenticated = AuthService.isAuthenticated();
    return isAuthenticated ? <Outlet /> : <Navigate to='/login' />
}

export default ProtectedRoutes;