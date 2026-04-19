
function SignUp()
{
    return (
        <div className="container">
            <form id="signUpForm">
                <div id="CustomErrorContainer">
                </div>

                <div className="form-outline mb-4">
                    <label className="form-label">Full Name</label>
                    <input className="form-control" />
                    <span className="text-danger"></span>
                </div>

                <div className="form-outline mb-4">
                    <label className="form-label">Date of Birth</label>
                    <input className="form-control" type="date" />
                    <span className="text-danger"></span>
                </div>

                <div className="form-outline mb-4">
                    <label className="form-label">User Name</label>
                    <input className="form-control" />
                    <span className="text-danger"></span>
                </div>

                <div className="form-outline mb-4">
                    <label className="form-label">Password</label>
                    <input className="form-control" type="password" />
                    <span className="text-danger"></span>
                </div>

                <div className="form-outline mb-4">
                    <label className="form-label">Repeat Password</label>
                    <input className="form-control" type="password" />
                    <span className="text-danger"></span>
                </div>

                <button type="button" className="btn btn-primary btn-block mb-3">Sign in</button>
            </form>
        </div>
    );
}

export default SignUp;