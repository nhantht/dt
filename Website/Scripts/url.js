var URLObj = {
    settings: {
        page: 1,
        prices: [],
        urlContent: ""
    },
    getPrices: function () {
        var prices = [];
        $.each($("#lstCurrency").find("option"), function (idx, item) {
            prices.push({ CurrencyId: $(item).attr("value"), Price: URLObj.settings.prices[$(item).attr("value")] });
        });
        return prices;
    },
    getDevices: function () {
        var deviceIds = [];
        var id = 0;
        $.each($("#lstSelectingDevice").find("option"), function (idx, item) {
            id = $(item).attr("value");
            deviceIds.push({ DeviceTypeId: id, Picture: $("#file_" + id).val(), Title: $("#title_" + id).val() });
        });
        return deviceIds;
    },
    create: function () {
        Validator.clearMessages();
        if (this.validate()) {
            Util.Layout.showLoading();
            //$("#categoryId").text("1");
            this.updateCurrency();

            $.ajax({
                type: "post",
                data: {
                    URLObj: {
                        CategoryId: $("#categoryId").text()
                        , URL: $("#URL").val()
                        , Budget: $("#Budget").val()
                        , Status: $("#status").val() == "On" ? true : false
                        , AutoPictureUrl: $("#pictureResult").find("img").attr("src")
                        , DeviceTypeInputs: URLObj.getDevices()
                        , PriceInputs: URLObj.getPrices()
                        , DisplayCurrencyId: $("#lstDisplayCurrency").val() == 0 ? null : $("#lstDisplayCurrency").val()
                        , DisplayLocationId: $("#lstLocation").val() == 0 ? null : $("#lstLocation").val()
                        , DisplayTimeframeId: $("#lstTimeframe").val() == 0 ? null : $("#lstTimeframe").val()
                        , DisplayTimes: $("#lstTimes").val() == 0 ? null : $("#lstTimes").val()
                    }
                },
                url: "adv/createURL",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        ADV.clearDetail();
                        //Reload the list
                        URLObj.loadList(URLObj.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    updateCurrency:function(){
        URLObj.settings.prices[$("#lstCurrency").val()] = $(".adv-price").val();
    },
    update: function () {
        if (this.validate()) {
            Util.Layout.showLoading();
            this.updateCurrency();
            $.ajax({
                type: "post",
                data: {
                    URLObj: {
                        CategoryId: $("#categoryId").text()
                        , Id: $("#urlId").text()
                        , URL: $("#URL").val()
                        , Budget: $("#Budget").val()
                        , Status: $("#status").val() == "On" ? true : false
                        , AutoPictureUrl: $("#pictureResult").find("img").attr("src")
                        , DeviceTypeInputs: URLObj.getDevices()
                        , PriceInputs: URLObj.getPrices()
                        , DisplayCurrencyId: $("#lstDisplayCurrency").val() == 0 ? null : $("#lstDisplayCurrency").val()
                        , DisplayLocationId: $("#lstLocation").val() == 0 ? null : $("#lstLocation").val()
                        , DisplayTimeframeId: $("#lstTimeframe").val() == 0 ? null : $("#lstTimeframe").val()
                        , DisplayTimes: $("#lstTimes").val() == 0 ? null : $("#lstTimes").val()
                    }
                },
                url: "adv/editURL",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        ADV.clearDetail();
                        //Reload the list
                        URLObj.loadList(URLObj.settings.page);
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
                url: "adv/deleteURL",
                success: function (response) {
                    if (response.result.length > 0) {
                        Validator.addMessage(response.result);
                        Util.Layout.hideLoading();
                    } else {
                        //Reset values
                        Validator.clearMessages();
                        ADV.clearDetail();
                        //Reload the list
                        URLObj.loadList(URLObj.settings.page);
                    }
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    showDetail: function (id, name, forUpdate) {
        //$("#categoryId").text("1");
        if ($("#categoryId").text().length == 0)
            return;

        this.showTitle(id, name);

        if (forUpdate) {
            Util.Layout.showLoading();
            URLObj.settings.prices = [];

            $(".adv-detail-container").load("adv/URLdetail?id=" + id + "&categoryId=" + $("#categoryId").text(), function () {
                //$("#URL").focus();
                URLObj.bindEvents();
                Util.Layout.hideLoading();
            });
        } else {
            $("#ui-id-18").click();
        }
    },
    validate: function () {
        Validator.clearMessages();
        Validator.checkRequired("URL", "Your URL is invalid.");
        Validator.checkNumeric("Budget", "Budget is invalid. Budget must be numeric and use a full stop (.) for decimals.");

        return !Validator.hasErrors();
    },
    addCurrency: function () {
        window.open("currency?fromChild=true", "_blank");
    },
    addCountry: function () {
        window.open("country?fromChild=true", "_blank");
    },
    addTimeframe: function () {
        window.open("timeframe?fromChild=true", "_blank");
    },
    showCreation: function () {
        if ($("#categoryId").text().length == 0)
            return;

        Util.Layout.showLoading();
        URLObj.settings.prices = [];

        $(".adv-detail-container").load("adv/createURL", function () {
            //$("#URL").focus();
            URLObj.bindEvents();
            Util.Layout.hideLoading();
        });
    },
    showTitle: function (id, name) {
        this.clearTitle();
        $("#urlDivider").show();
        $("#urlName").text(name);
        $("#urlName").attr("href", "javascript:URLObj.showDetail(" + id + ", '" + name + "', false)");
        $("#urlId").text(id);
    },
    clearTitle: function () {
        $("#keywordDivider").hide();
        $("#keywordName").text("");
        $("#keywordId").text("");
    },
    loadList: function (page) {
        //alert("Under construction");
        //return;
        Util.Layout.showLoading();
        $("#tabs-3").load("adv/URLlist?page=" + page + "&categoryId=" + $("#categoryId").text(), function (response) {
            Util.Layout.hideLoading();
        });
    },
    addDevice: function (id, name) {
        var option = document.createElement("option");
        $(option).text(name);
        $(option).val(id);
        $("#lstSelectingDevice").find("option").add(option);
    },
    showDisplay4Device: function (deviceTypeId) {
        $("#displayImg").attr("src", $("#file_" + deviceTypeId).attr("value"));
        $("#displayTitle").text($("#title_" + deviceTypeId).val());
        $("#displayPrice").text(URLObj.settings.prices[deviceTypeId]);
    },
    bindEvents: function () {
        $(".adv-device").change(function () {
            var id = $(this).val();

            $(".adv-title-field").addClass("sr-only");
            $("#title_" + id).removeClass("sr-only");

            $(".adv-file").addClass("sr-only");
            $("#file_" + id).removeClass("sr-only");
        });
        $("#lstCurrency").focus(function () {
            SavePrice(this);
        }).change(function () {
            $(".adv-price").val(URLObj.settings.prices[$(this).val()]);
        });

        function SavePrice(f) {
            URLObj.settings.prices[$(f).val()] = $(".adv-price").val();
        }

        $("#lstSelectingDevice4Display").change(function () {
            URLObj.showDisplay4Device($(this).val());
        });
        //Upload files
        $(".adv-file").change(function () {
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
                    url: 'adv/UploadFiles',
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
    addDevice: function (f, id, name) {
        if ($(f).prop("checked")) {
            var option = document.createElement("option");
            $(option).text(name);
            $(option).val(id);
            if ($("#lstSelectingDevice").find('option[value="' + id + '"]').length == 0) {
                $("#lstSelectingDevice").append(option);
            }
        } else {
            $("#lstSelectingDevice").find('option[value="' + id + '"]').remove();
        }
    },
    getURLContent: function (f) {
        if ($.trim($(f).val()).length > 0) {
            Util.Layout.showLoading();
            $.ajax({
                type: "post",
                data: { url: $.trim($(f).val()) },
                url: "adv/GetURLContent",
                success: function (response) {
                    var img = document.createElement("img");
                    $(img).width(200);
                    $(img).attr("src", response.Image);
                    $("#pictureResult").empty().append(img);
                    $(".adv-title-field").not(".sr-only").val(response.Title);

                    Util.Layout.hideLoading();
                },
                error: function () {
                    Util.Layout.hideLoading();
                }
            });
        }
    },
    getTitle: function () {
        var div = document.createElement("div");
        div.innerHTML = URLObj.settings.urlContent;
        var title = $(div).find($("#titleClue").val()).text();
        $.each($(".adv-title-field"), function (idx, item) {
            if (!$(item).hasClass("sr-only")) {
                $(item).val(title);
            }
        });
    },
    getPicture: function () {
        var div = document.createElement("div");
        div.innerHTML = URLObj.settings.urlContent;
        $("#pictureResult").empty().append($(div).find($("#pictureClue").val()));
        $("#pictureResult").children().first().css("width", 200);
    },
    getPrice: function () {
        var div = document.createElement("div");
        div.innerHTML = URLObj.settings.urlContent;
        var price = parseFloat($(div).find($("#priceClue").val()).text());
        $(".adv-price").val(price);
    },
    refreshCurrencyList: function (list) {
        list = list.replace(/&quot;/gi, '"');
        list = JSON.parse(list);

        $.each(list, function (i, item) {
            var foundItem = $("#lstDisplayCurrency option[value='" + item.Id + "']");
            var foundItem2 = $("#lstCurrency option[value='" + item.Id + "']");
            if (foundItem.length == 0) {
                $("#lstDisplayCurrency").append($('<option>', {
                    value: item.Id,
                    text: item.Code
                }));
                $("#lstCurrency").append($('<option>', {
                    value: item.Id,
                    text: item.Code
                }));
            } else {
                foundItem.text(item.Code);
                foundItem2.text(item.Code);
            }
            //Clean
            foundItem = foundItem2 = null;
        });
    },
    refreshLocationList: function (list) {
        
        list = list.replace(/&quot;/gi, '"');
        list = JSON.parse(list);

        $.each(list, function (i, item) {
            var foundItem = $("#lstLocation option[value='" + item.Value + "']");
            if (foundItem.length == 0) {
                debugger
                $("#lstLocation").append($('<option>', {
                    value: item.Value,
                    text: item.Text
                }));
            } else {
                foundItem.text(item.Text);
            }
            //Clean
            foundItem = null;
        });
    },
    refreshTimeFrameList: function (list) {
        
        list = list.replace(/&quot;/gi, '"');
        list = JSON.parse(list);

        $.each(list, function (i, item) {
            var foundItem = $("#lstTimeframe option[value='" + item.Id + "']");
            if (foundItem.length == 0) {
                debugger
                $("#lstTimeframe").append($('<option>', {
                    value: item.Id,
                    text: item.Name
                }));
            } else {
                foundItem.text(item.Name);
            }
            //Clean
            foundItem = null;
        });
    }
}