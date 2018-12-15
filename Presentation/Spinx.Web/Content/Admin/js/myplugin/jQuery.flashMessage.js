(function ($) {

    $.fn.showalert = function (message, alerttype) {
        var target = this;

        var icon = "times";
        switch (alerttype.toString().toLowerCase()) {
            case "error": icon = "times"; break;
            case "success": icon = "check"; break;
            case "info": icon = "info"; break;
            case "warning": icon = "warning"; break;
        }

        target.append('<div class="alert alert-' +
            alerttype.toString().toLowerCase() +
            ' fade in removealert">'
            + '<a class="close" data-dismiss="alert">&times;</a><span>' +
            "<i class=\"fa-fw fa fa-" + icon + "\"></i>" +
            message +
            "</span></div>");
        $(".removealert").delay(5000).fadeOut(400);
    }


    $.fn.flashMessage = function (options) {
        var target = this;
        options = $.extend({ timeout: 3000, alert: "info" }, options);

        if (!options.message) {
            setFlashMessageFromCookie(options);
        }


        // Get the first alert message read from the cookie
        function setFlashMessageFromCookie() {
            $.each(new Array("Success", "Danger", "Warning", "Info"), function (i, alert) {

                var cookie = $.cookie("Flash." + alert);

                if (!jQuery.isEmptyObject(cookie)) {
                    options.message = cookie;
                    options.alert = alert;
                    target.showalert(options.message, options.alert);
                    deleteFlashMessageCookie(alert);
                    return;
                }
            });
        }

        // Delete the named flash cookie
        function deleteFlashMessageCookie(alert) {
            //$.cookie("Flash." + alert, null, { path: '/' });
            $.removeCookie("Flash." + alert, { path: "/" });
        }
    };


}(jQuery));