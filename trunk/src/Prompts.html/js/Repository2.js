var Repository2 = Class.extend({
    init: function (url, loadingPanel) {
        this.url = url;
        this.loadingPanel = loadingPanel;
    },

    get: function (request, successCallback) {
        this.loadingPanel.showLoading();
        $.ajax({
            type:"POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(request),
            url:this.url,
            context: this,
            success:function (result) {
                successCallback(result);
                this.loadingPanel.showLoaded();
            },
            error:function (xhr, statusText, errorThrown) {
                this.loadingPanel.showError(JSON.parse(xhr.responseText).ResponseStatus.Message);
            }
        });
    }
});