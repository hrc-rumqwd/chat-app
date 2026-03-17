function onSubmitLoginForm(e) {
    const form = $('form#loginForm') 
    const formObj = form.serializeObjects();

    $.ajax({
        url: '/login',
        type: 'POST',
        dataType: 'json',
        data: formObj,
        success: function (data) {
            debugger;
            if (data.isSuccess) {
                window.location.href = data.data.returnUrl ?? "/";
            }
            else {
                console.error(data.error);
            }
        },
    })
}

