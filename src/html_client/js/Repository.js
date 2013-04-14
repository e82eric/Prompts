var Repository = Class.extend({
    init: function (url, loadingPanel, method) {
        this.url = url;
        this.loadingPanel = loadingPanel;
        this.method = method;
    },

    get: function (request, successCallback) {
        this.loadingPanel.showLoading();
        $.ajax({
            type:this.method,
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