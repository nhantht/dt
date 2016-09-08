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
                        Index.loadScheduleList(1);
                        break;
                    }
                }
            }
        });

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
                case "ui-id-16": {
                    Index.showScheduleCreation();
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
    showScheduleCreation: function () {
        Util.Layout.showLoading();
        $(".adv-detail-container").load("index/createschedule", function () {
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
    updateSchedule: function () {
        if (this.validateSchedule()) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { Id: $("#Id").val(), FilePath: $("#FilePath").val(), Hour: $("#Hour").val(), Minute: $("#Minute").val(), Active: $("#Active").prop("checked") },
                url: "index/updateschedule",
                success: function (response) {
                    if (response.Message.length > 0) {
                        Validator.addMessage(response.Message);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        //Reload the list
                        Index.loadScheduleList(Index.settings.page);
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
                data: { URL: $("#URL").val().trim},
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
    createSchedule: function () {
        if (this.validateSchedule()) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { FilePath: escape($("#FilePath").val()), Hour: $("#Hour").val(), Minute: $("#Minute").val(), Active: $("#Active").prop("checked")},
                url: "index/createschedule",
                success: function (response) {
                    if (response.Message.length > 0) {
                        Validator.addMessage(response.Message);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        $("#FilePath").val("");
                        $("#Hour").val(0);
                        $("#Minute").val(0);
                        $("#Active").prop("checked", false);

                        Index.clearDetail();
                        //Reload the list
                        Index.loadScheduleList(Index.settings.page);
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
                data: { id: $("#Id").val() },
                url: "index/delete",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
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
    destroySchedule: function (id) {
        if (confirm("Would you like to delete it? Please click Yes to delete or No to cancel.")) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { id: $("#Id").val() },
                url: "index/deleteschedule",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        //Reload the list
                        Index.loadScheduleList(Index.settings.page);
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
        Validator.checkRequired("URL", "You must enter URL.");

        return !Validator.hasErrors();
    },
    validateSchedule: function () {
        Validator.clearMessages();
        Validator.checkRequired("FilePath", "You must enter URL.");
        Validator.checkNumeric("Hour", "Hour is invalid. Hour must be integer.");
        Validator.checkRangeNumeric("Hour", "Hour is between 0 and 23.", 0, 23);
        Validator.checkNumeric("Minute", "Minute is invalid. Minute must be integer.");
        Validator.checkRangeNumeric("Minute", "Minute is between 0 and 59.", 0, 59);

        return !Validator.hasErrors();
    },
    showDetail: function (id) {
        Util.Layout.showLoading();
        $(".adv-detail-container").load("index/detail?id=" + id, function () {
            Index.bindEvents();
            Util.Layout.hideLoading();
        });
    },
    showScheduleDetail: function (id) {
        Util.Layout.showLoading();
        $(".adv-detail-container").load("index/scheduledetail?id=" + id, function () {
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
    loadScheduleList: function (page) {
        Util.Layout.showLoading();
        $("#tabs-2").load("index/schedulelist?page=" + page, function () {
            Index.showScheduleCreation();
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