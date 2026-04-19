import React, { useState } from "react";
import { ConversationService } from "../services/api-service";
import { AuthService } from "../services/auth-service";
import LoginCommand from "../models/auth/login";

export default function Login() {

    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");

    const handleUserNameChange = (event) => {
        setUserName(event.target.value);
    };

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
    }

    function handleLogin(e) {
        console.log(userName, password);
        AuthService.login(userName, password, (data) => {
            console.log("Login Successfully.");
            e.preventDefault();
        });
    }

    return (
        <div className="container">
            <form id="loginForm">
                <div data-mdb-input-init className="form-outline mb-4">
                    <label className="form-label">User Name</label>
                    <input className="form-control" value={userName} onChange={handleUserNameChange} />
                    <span className="text-danger"></span>
                </div>

                <div data-mdb-input-init className="form-outline mb-4">
                    <label className="form-label">Password</label>
                    <input type="password" className="form-control" value={password} onChange={handlePasswordChange} />
                    <span className="text-danger"></span>
                </div>

                <div className="form-check">
                    <input type="checkbox" className="form-check-input" checked />
                    <label className="form-check-label">Remember Me</label>
                </div>

                <button type="button" data-mdb-button-init data-mdb-ripple-init className="btn btn-primary btn-block mb-4" onClick={(e) => handleLogin(e)}>Sign in</button>

                <div className="row">
                    <span>
                        Not have account?
                        <a href="/sign-up">Register</a>
                    </span>
                </div>
            </form>
        </div>
    );
}