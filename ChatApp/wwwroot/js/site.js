// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function ($) {
    $.fn.serializeObjects = function () {
        const formData = this.serializeArray();
        const formObject = {};

        $.each(formData, function () {
            formObject[this.name] = this.value;
        });

        return formObject;
    }
})(jQuery)

//const api = {
//    post = function (url, payload, success, error) {
//        $.ajax({
//            type: "POST",
//            url: url,
//            data: data,
//            success: success,
//            error: error
//        })
//    },

//    get = function (url, success, error) {
//        $.ajax({
//            type: "GET",
//            url: url,
//            success: success,
//            error
//        })
//    }
//}