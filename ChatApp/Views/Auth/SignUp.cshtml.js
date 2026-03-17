function OnSubmitRegisterForm(e) {
    e.preventDefault();
    const fullName = $("input#FullName").val();
    const email = $("input#Email").val();
    const userName = $("input#UserName").val();
    const password = $("input#Password").val();
    const repeatPassword = $("input#RepeatPassword").val();
    const dob = new Date($("input#Dob").val()).toISOString();

    if (password !== repeatPassword) {
        const customErrorContainer = $('#CustomErrorContainer');
        customErrorContainer.addClass('is-show');

        const $p = $("</p>")
        $p.addClass('text-danger');
        $p.text('Password and repeat password is not match');
        customErrorContainer.append($p);
    }

    $.ajax({
        url: '/sign-up',
        type: 'POST',
        dataType: 'json',
        data: {
            fullName,
            email,
            userName,
            password,
            repeatPassword,
            dob
        },
        success: function (data) {
            if (data.isSuccess) {
                window.location.href = '/login';
            }
            else {
                // Show error message
                console.error(data.error);
            }
        }
    })
}