var Util = {
    Layout:{
        showLoading: function (obj) {
            try {
                var container = (obj == null) ? 'body' : obj;
                if (container != "none") {
                    if ($(container).find('#ajaxLoader').length == 0) {
                        $(container).append('<div id="ajaxLoader" class="custom-animsition-loading" > <div id="popupContainer"><div id="popup" class="image-loading"></div></div></div>');
                        //change language
                        $('#popup').attr('data-content', 'Loading');
                    }
                }
                // Clean
                container = null;
            } catch (e) { }
        },
        hideLoading: function () {
            try {
                $('#ajaxLoader').remove();
            } catch (e) { }
        }
    },
    Convert: {
        setSystemDate: function (value, objId) {
            var ngay = new Date(value);
            ngay = ngay.getFullYear() + "/" + (ngay.getMonth() + 1) + "/" + ngay.getDate();
            $('#' + objId).val(ngay);
        }
    }
};