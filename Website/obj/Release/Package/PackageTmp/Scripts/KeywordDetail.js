var KeywordDetail = {
    settings: {
        page: 1
    },
    loadList: function (page) {
        Util.Layout.showLoading();
        $("#tabs-5").load("adv/KeywordDetailList?page=" + page + "&keywordId=" + $("#keywordId").text(), function (response) {
            Util.Layout.hideLoading();
        });
    }
}