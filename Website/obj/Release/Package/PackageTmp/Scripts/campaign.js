var Campaign = {
    settings: {
        page: 1
    },
    create: function () {
        if (this.validate()) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { Name: $("#Name").val(), Budget: $("#Budget").val(), Status: $("#status").val() == "On" ? true : false },
                url: "adv/createcampaign",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        $("#Name").val("");
                        $("#Budget").val(0);
                        ADV.clearDetail();
                        //Reload the list
                        Campaign.loadList(Campaign.settings.page);
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
            $.ajax({
                type: "post",
                data: { Id: $("#Id").val(), Name: $("#Name").val(), Budget: $("#Budget").val(), Status: $("#status").val() == "On" ? true : false },
                url: "adv/editcampaign",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        $("#Name").val("");
                        $("#Budget").val(0);
                        ADV.clearDetail();
                        //Reload the list
                        Campaign.loadList(Campaign.settings.page);
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
                url: "adv/deletecampaign",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        ADV.clearDetail();
                        Campaign.clearTitle();
                        //Reload the list
                        Campaign.loadList(Campaign.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    showDetail: function (id, name, forUpdate) {
        Campaign.showTitle(id, name);
        if (forUpdate) {
            Util.Layout.showLoading();
            $(".adv-detail-container").load("adv/campaigndetail?id=" + id, function () {
                $("#Name").focus();
                Util.Layout.hideLoading();
            });
        } else {
            $("#ui-id-16").click();
        }
    },
    showTitle: function (id, name) {
        this.clearTitle();
        $("#campaignDivider").show();
        $("#campaignName").text(name);
        $("#campaignName").attr("href", "javascript:Campaign.showDetail(" + id + ", '" + name + "', false)");
        $("#campaignId").text(id);
    },
    clearTitle: function (id, name) {
        $("#categoryDivider").hide();
        $("#categoryName").text("");
        $("#categoryId").text("");

        $("#urlDivider").hide();
        $("#urlName").text("");
        $("#urlId").text("");

        $("#keywordDivider").hide();
        $("#keywordName").text("");
        $("#keywordId").text("");
    },
    validate: function () {
        Validator.clearMessages();
        Validator.checkRequired("Name", "You must name the campaign.");
        Validator.checkNumeric("Budget", "Budget is invalid. Budget must be numeric and use a full stop (.) for decimals.");

        return !Validator.hasErrors();
    },
    showCreation: function () {
        Util.Layout.showLoading();
        $(".adv-detail-container").load("adv/createcampaign", function () {
            $("#Name").focus();
            Util.Layout.hideLoading();
        });
    },
    loadList: function (page) {
        Util.Layout.showLoading();
        $("#tabs-1").load("adv/campaignlist?page=" + page, function () {
            Util.Layout.hideLoading();
            ADV.resizeFooter();
        });
    }
}