var Validator = {
    settings: {
        messagePanelId: "messagePanel"
    },
    checkRequired: function (id, message) {
        if ($.trim($("#" + id).val()).length == 0) {
            this.addMessage(message);
            $("#" + id).focus();
            return false;
        }
        return true;
    },
    checkNumeric: function (id, message) {
        if ($.trim($("#" + id).val()).length > 0
            && !$.isNumeric($.trim($("#" + id).val()))) {
            this.addMessage(message);
            $("#" + id).focus();
            return false;
        }
        return true;
    },
    checkRangeNumeric: function (id, message, from, to) {
        if (parseInt($("#" + id).val()) < from
            || parseInt($("#" + id).val()) > to) {
            this.addMessage(message);
            $("#" + id).focus();
            return false;
        }
        return true;
    },
    addMessage: function (message) {
        var li = document.createElement("li");
        $(li).text(message);
        $("#" + this.settings.messagePanelId).children().first().append(li);
    },
    clearMessages: function () {
        $("#" + this.settings.messagePanelId).children().first().find("li").remove();
    },
    hasErrors: function () {
        return $("#" + this.settings.messagePanelId).find("li").length > 0;
    }
}