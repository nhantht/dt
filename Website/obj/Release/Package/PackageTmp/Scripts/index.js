var Index = {
    settings: {
        page: 1
    },
    init: function () {
        //Init tabs
        $("#tabList").tabs({
            activate: function (event, ui) {
                var id = ui.newTab.find("a").first().attr("id");
                Index.clearDetail();
                switch (id) {
                    case "ui-id-15": {
                        Index.loadList(1);
                        break;
                    }
                    case "ui-id-16": {
                        break;
                    }
                }
            }
        });
        $(".adv-plus-icon").last().remove();

        //Load index list
        Index.loadList(1);
    },
    bindEvents: function () {
        $("#tabList").find(".adv-plus-icon").bind("click", function () {
            var id = $(this).parent().find("a").attr("id");
            switch (id) {
                case "ui-id-15": {
                    Index.showCreation();
                    break;
                }
            }
        });
        //Upload files
        $(".adv-file").change(function () {
            debugger
            // Checking whether FormData is available in browser  
            if (window.FormData !== undefined) {

                var fileUpload = $(this).get(0);
                var files = fileUpload.files;

                // Create FormData object  
                var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }

                $.ajax({
                    url: 'adv/UploadLinkFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            } else {
                alert("FormData is not supported.");
            }
        });
    },
    showCreation: function () {
        Util.Layout.showLoading();
        $(".adv-detail-container").load("index/create", function () {
            //Reset height
            $(".middle-content").css("height", "auto");
            Index.resizeFooter();
            //Bind events
            Index.bindEvents();
            Util.Layout.hideLoading();
        });
    },
    update: function () {
        if (this.validate()) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { Id: $("#Id").val(), URL: $("#URL").val(), Budget: $("#Budget").val(), Title: $("#Title").val(), ShortDescription: $("#ShortDescription").val(), Price: $("#Price").val(), Picture: $(".adv-file").val() },
                url: "index/update",
                success: function (response) {
                    if (response.Message.length > 0) {
                        Validator.addMessage(response.Message);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        //Reload the list
                        Index.loadList(Index.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    create: function () {
        if (this.validate()) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { URL: $("#URL").val(), Budget: $("#Budget").val(), Title: $("#Title").val(), ShortDescription: $("#ShortDescription").val(), Price: $("#Price").val(), Picture: $(".adv-file").val() },
                url: "index/create",
                success: function (response) {
                    if (response.Message.length > 0) {
                        Validator.addMessage(response.Message);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        $("#URL").val("");
                        $("#Title").val("");
                        $("#ShortDescription").val("");
                        $("#Picture").val("");

                        $("#Budget").val(0);
                        Index.clearDetail();
                        //Reload the list
                        Index.loadList(Index.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    validate: function () {
        Validator.clearMessages();
        Validator.checkRequired("URL", "You must name the URL.");
        Validator.checkRequired("Title", "You must enter Title.");
        Validator.checkRequired("ShortDescription", "You must enter Short Description.");
        Validator.checkNumeric("Price", "Price is invalid. Price must be numeric and use a full stop (.) for decimals.");

        return !Validator.hasErrors();
    },
    showDetail: function (id) {
        Util.Layout.showLoading();
        $(".adv-detail-container").load("index/detail?id=" + id, function () {
            Index.bindEvents();
            Util.Layout.hideLoading();
        });
    },
    loadList: function (page) {
        Util.Layout.showLoading();
        $("#tabs-1").load("index/list?page=" + page, function () {
            Index.showCreation();
        });
    },
    resizeFooter: function () {
        if ($("body").height() <= $(window).height()) {
            $("#footer").removeClass("narbar");
            $("#footer").removeClass("footer");
        } else {
            $("#footer").addClass("narbar");
            $("#footer").addClass("footer");
        }
    },
    clearDetail: function () {
        $(".adv-detail-container").empty();
    }
}