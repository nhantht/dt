var User = {
    settings:{
        messageObjectId: "lblMessage"
    },
    resetPassword: function (id) {
        Util.Layout.showLoading(null);
        $.ajax({
            type: "post",
            url: "../resetpassword",
            data: { id: id },
            success: function (response) {
                $("#" + User.settings.messageObjectId).text(response.result);
                Util.Layout.hideLoading();
            },
            error: function () {
                Util.Layout.hideLoading();
            }
        });
    }
}