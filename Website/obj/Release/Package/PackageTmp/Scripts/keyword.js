var Keyword = {
    settings: {
        page: 1
    },
    create: function () {
        if (this.validate()) {
            Util.Layout.showLoading();
            var devices = [];
            $.each($(".nagetive-march"), function (idx, item) {
                devices.push({ DeviceId: $(item).attr("data-value"), Name: $(item).attr("data-text") })
            });
            $.ajax({
                type: "post",
                data: {
                    Keyword: $("#Keyword").val()
                    , Budget: $("#Budget").val()
                    , Marching: $("#lstMarching").val()
                    , Status: $("#status").val() == "On" ? true : false
                    , NegativeMarchs: devices
                    , Bid: $("#Bid").val()
                    , URLId: $("#urlId").text()
                },
                url: "adv/createKeyword",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        ADV.clearDetail();
                        //Reload the list
                        Keyword.loadList(Keyword.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    update: function () {
        if (this.validate()) {
            Util.Layout.showLoading();
            var devices = [];
            $.each($(".nagetive-march"), function (idx, item) {
                devices.push({ DeviceId: $(item).attr("data-value"), Name: $(item).attr("data-text") })
            });
            $.ajax({
                type: "post",
                data: {
                    Id: $("#Id").val()
                    , Keyword: $("#Keyword").val()
                    , Budget: $("#Budget").val()
                    , Marching: $("#lstMarching").val()
                    , Status: $("#status").val() == "On" ? true : false
                    , NegativeMarchs: devices
                    , Bid: $("#Bid").val()
                },
                url: "adv/editKeyword",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        ADV.clearDetail();
                        //Reload the list
                        Keyword.loadList(Keyword.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    destroy: function (id) {
        if (confirm("Would you like to delete it? Please click Yes to delete or No to cancel.")) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { Id: $("#Id").val() },
                url: "adv/deleteKeyword",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        ADV.clearDetail();
                        //Reload the list
                        Keyword.loadList(Keyword.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    showDetail: function (id, name, forUpdate) {
        if ($("#urlId").text().length == 0)
            return;

        this.showTitle(id, name);
        if (forUpdate) {

            Util.Layout.showLoading();

            $(".adv-detail-container").load("adv/Keyworddetail?id=" + id, function () {
                $("#Keyword").focus();
                Util.Layout.hideLoading();
            });
        } else {
            $("#ui-id-19").click();
        }
    },
    showTitle: function (id, name) {
        $("#keywordDivider").show();
        $("#keywordName").text(name);
        $("#keywordId").text(id);
    },
    validate: function () {
        Validator.clearMessages();
        Validator.checkRequired("Keyword", "You must add a keyword (pharse).");
        Validator.checkNumeric("Budget", "Budget is invalid. Budget must be numeric and use a full stop (.) for decimals.");

        return !Validator.hasErrors();
    },
    showCreation: function () {
        if ($("#urlId").text().length == 0)
            return;

        Util.Layout.showLoading();
        $(".adv-detail-container").load("adv/createKeyword", function () {
            $("#Name").focus();
            Util.Layout.hideLoading();
        });
    },
    loadList: function (page) {
        Util.Layout.showLoading();
        $("#tabs-4").load("adv/Keywordlist?page=" + page + "&URLId=" + $("#urlId").text(), function (response) {
            Util.Layout.hideLoading();
        });
    },
    addNegativeMarch: function (f) {
        var value = $(f).val();
        var text = $(f).find("option:selected").text();

        this.addNegativeMarchItem(value, text);
    },
    addNegativeMarchItem: function (value, text) {
        if ($('.nagetive-march[data-value="' + value + '"]').length == 0) {
            var id = Util.Tool.getNewGuid();
            var a = document.createElement("a");
            $(a).attr("id", id);
            $(a).attr("data-value", value);
            $(a).attr("data-text", text);
            $(a).attr("href", "javascript:Keyword.removeNagetiveMarch('" + id + "')");
            a.className = "nagetive-march";
            var span = document.createElement("span");
            $(span).text("-");
            span.className = "nagetive-march-icon";
            a.appendChild(span);
            span = document.createElement("span");
            $(span).addClass("nagetive-march-text");
            $(span).text(text);
            a.appendChild(span);
            $("#divNagetiveMarch").append(a);
        }
    },
    removeNagetiveMarch: function (id) {
        $("#" + id).remove();
    }
}