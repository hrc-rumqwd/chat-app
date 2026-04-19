import LoginCommand from "../models/auth/login";
import apiClient from "./api-service";

export const AuthService = {
    TOKEN_KEY: "secretToken",

    isAuthenticated() {
        return localStorage.getItem(this.TOKEN_KEY)
            ? true 
            : false;
    },

    login(userName, password, onSuccess, onFailed) {
        debugger;
        apiClient.post('/login', new LoginCommand(userName, password))
        .then(data => {
            debugger;
            localStorage.setItem(this.TOKEN_KEY, data.data.accessToken);
            if(onSuccess)
                onSuccess(data);
        })
        .catch(err => console.error(err));
    },
}