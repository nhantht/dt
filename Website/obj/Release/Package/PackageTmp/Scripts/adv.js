var ADV = {
    init: function () {
        //Init tabs
        $("#tabList").tabs({
            activate: function (event, ui) {
                var id = ui.newTab.find("a").first().attr("id");
                ADV.clearDetail();
                switch (id) {
                    case "ui-id-15": {
                        Campaign.loadList(1);
                        break;
                    }
                    case "ui-id-16": {
                        Category.loadList(1);
                        break;
                    }
                    case "ui-id-17": {
                        URLObj.loadList(1);
                        break;
                    }
                    case "ui-id-18": {
                        Keyword.loadList(1);
                        break;
                    }
                    case "ui-id-19": {
                        KeywordDetail.loadList(1);
                        break;
                    }
                    default: {
                        Util.Layout.hideLoading();
                    }
                }
            }
        });
        $(".adv-plus-icon").last().remove();

        //Load campagn list
        Campaign.loadList(1);
        //Hide dividers
        $(".divider").hide();
        //$("#campaingId").text("5");
        //$("#categoryId").text("7");
        //URLObj.showDetail(6, "PC", true);
        //Bind events
        $("#tabList").find(".adv-plus-icon").bind("click", function () {
            var id = $(this).parent().find("a").attr("id");
            switch (id) {
                case "ui-id-15": {
                    Campaign.showCreation();
                    break;
                }
                case "ui-id-16": {
                    Category.showCreation();
                    break;
                }
                case "ui-id-17": {
                    URLObj.showCreation();
                    break;
                }
                case "ui-id-18": {
                    Keyword.showCreation();
                    break;
                }
                default: {
                    Util.Layout.hideLoading();
                }
            }
        });
    },
    resizeFooter:function(){
        if ($("body").height() <= $(window).height()) {
            $("#footer").css("top", $(window).height() - 65 - $("#footer").position().top);
        }
    },
    dismiss: function () {
        if (confirm("Would you like to abandon the changes? Please click Yes to abandon or No to keep the changes.")) {
            this.clearDetail();
        }
    },
    clearDetail: function () {
        $(".adv-detail-container").empty();
    }
}