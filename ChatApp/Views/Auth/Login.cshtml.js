function onSubmitRegisterForm(e) {
    const form = $('form#loginForm') 
    const formObj = form.serializeObjects();

    api.post('/login', formObj,
        success: function (data) {

        },
        error: function (data) {

        }
    );
}

