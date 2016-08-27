var Category = {
    settings: {
        page: 1
    },
    create: function () {
        if (this.validate()) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { CampaignId: $("#campaignId").text(), Name: $("#Name").val(), Budget: $("#Budget").val(), Status: $("#status").val() == "On" ? true : false },
                url: "adv/createcategory",
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
                        Category.loadList(Category.settings.page);
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
                url: "adv/editCategory",
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
                        Category.loadList(Category.settings.page);
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
                url: "adv/deleteCategory",
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
                        Category.loadList(Category.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    showDetail: function (id, name, forUpdate) {
        if ($("#campaignId").text().length == 0)
            return;

        this.showTitle(id, name);
        if (forUpdate) {

            Util.Layout.showLoading();

            $(".adv-detail-container").load("adv/Categorydetail?id=" + id + "&campaignId=" + $("#campaignId").text(), function () {
                $("#Name").focus();
                Util.Layout.hideLoading();
            });
        } else {
            $("#ui-id-17").click();
        }
    },
    showTitle: function (id, name) {
        this.clearTitle();
        $("#categoryDivider").show();
        $("#categoryName").text(name);
        $("#categoryName").attr("href", "javascript:Category.showDetail(" + id + ", '" + name + "', false)");
        $("#categoryId").text(id);
    },
    clearTitle: function (id, name) {
        $("#urlDivider").hide();
        $("#urlName").text("");
        $("#urlId").text("");

        $("#keywordDivider").hide();
        $("#keywordName").text("");
        $("#keywordId").text("");
    },
    validate: function () {
        Validator.clearMessages();
        Validator.checkRequired("Name", "You must name the add group.");
        Validator.checkNumeric("Budget", "Budget is invalid. Budget must be numeric and use a full stop (.) for decimals.");

        return !Validator.hasErrors();
    },
    showCreation: function () {
        if ($("#campaignId").text().length == 0)
            return;

        Util.Layout.showLoading();
        $(".adv-detail-container").load("adv/createCategory", function () {
            $("#Name").focus();
            Util.Layout.hideLoading();
        });
    },
    loadList: function (page) {
        Util.Layout.showLoading();
        $("#tabs-2").load("adv/Categorylist?page=" + page + "&campaignId=" + $("#campaignId").text(), function (response) {
            Util.Layout.hideLoading();
            ADV.resizeFooter();
        });
    }
}