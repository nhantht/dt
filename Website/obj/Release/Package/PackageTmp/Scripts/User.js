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
    },
    initPhoneField: function (codes, countryNames) {
        //Set border
        $("#Phone").focus(function () {
            $("#lstCountry").find("a").first().addClass("input-border");
        });
        $("#Phone").blur(function () {
            $("#lstCountry").find("a").first().removeClass("input-border");
        });
        //Hide signin
        $("#btnLogin").hide();
        $("#logoContainer").css("padding", "0 0 20px 0");
        //Set country names
        var iElement = $(".bfh-selectbox-option").find("i");
        $(".bfh-selectbox-option").empty();
        $(".bfh-selectbox-option").append(iElement);
        $(".bfh-selectbox-option").html($(".bfh-selectbox-option").html() + "Vietnam (+84)");
        //Clean
        iElement = null;

        var languageCodes = "";
        var codes = codes.split(",");
        var countryNames = countryNames.split(",");
        $.each($(".bfh-selectbox-options").find("ul").first().find("a"), function (idx, item) {
            iElement = $(item).find("i");
            $(item).empty();
            $(item).append(iElement);
            console.log(countryNames[idx]);
            $(item).html($(item).html() + countryNames[idx] + " (+" + codes[idx] + ")");
            //Clean
            iElement = null;
        });

        $(".caret").remove();
        var caret = document.createElement("span");
        $(caret).addClass("caret");
        $(caret).attr("id", "phoneCaret");
        $(".bfh-selectbox-option").append(caret);
        $("#Phone").focus(function () {
            $("#phoneCaret").hide();
        });
        $("#Phone").blur(function () {
            if ($.trim($(this).val()).length == 0) {
                $("#phoneCaret").show();
            }
        });
    },
    setCountryCode: function () {
        var countryData = $("#Phone").intlTelInput("getSelectedCountryData");
        $("#CountryCode").val(countryData.iso2);
    },
    detectCountryCode: function () {
        $.get("http://ipinfo.io", function () { }, "jsonp").always(function (resp) {
            var countryCode = (resp && resp.country) ? resp.country : "";
            $("#Phone").intlTelInput("setCountry", countryCode);
        });
    }
}