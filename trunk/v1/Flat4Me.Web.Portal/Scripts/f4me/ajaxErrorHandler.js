/**
* Global ajax error handler. Notify user by toast message when ajax error handled
*/
var AjaxErrorHandler = {
    /**
    * Global registration
    */
    errorHandled: false,

    register: function () {
        $(document).ajaxError(function (event, request, settings) {
            if (AjaxErrorHandler.errorHandled) {
                AjaxErrorHandler.errorHandled = false;
                return;
            }

            var title = settings.errorTitle || "";

            //message by default
            var message = request.statusText;

            //check if server returned processed exception
            //if so, replace message to more detailed
            if (request.responseJSON && request.responseJSON.hasError && request.responseJSON.message) {
                message = request.responseJSON.message;
            }
            else if (request.responseText) {
                var contentHeader = request.getResponseHeader("Content-Type");
                if (contentHeader && contentHeader.indexOf("json") > -1) {
                    try {
                        var responseJson = jQuery.parseJSON(request.responseText);
                        message = responseJson.message ? responseJson.message : message;
                    } catch (e) { }
                }
            }

            // Tost error message to user
            toastr.error(message, title);
        });
    }
}