var Repository = Class.extend({
    init: function (url, loadingPanel, method, dataType) {
        this.url = url;
        this.loadingPanel = loadingPanel;
        this.method = method;

        this._dataType = dataType;

        if(this._dataType == undefined) {
            this._dataType = "json";
        }
    },

    get: function (request, successCallback) {
        this.loadingPanel.showLoading();
        $.ajax({
            type:this.method,
            contentType: "application/json; charset=utf-8",
            dataType: this._dataType,
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