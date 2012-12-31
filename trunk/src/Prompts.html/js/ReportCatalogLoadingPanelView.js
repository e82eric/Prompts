var ReportCatalogLoadingPanelView = Class.extend({
    init: function(controller, itemsControllerView) {
        this.controller = controller;

        var template = $("#reportCatalogLoadingPanelTemplate").html();
        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root = $(templateHtml);

        this.loadingElement = this.root.filter("#loading");
        this.loadedElement = this.root.filter("#loaded");
        this.retryElement = this.root.filter("#retry");
        this.errorElement = this.root.filter("#errorMessage");

        this.loadedElement.append(itemsControllerView.render());
    },

    render: function(){
        this.retryElement.click($.proxy(this.onRetryClick, this));
        return this.root;
    },

    showLoading: function () {
        this.loadingElement.show();
    },

    hideLoading: function () {
        this.loadingElement.hide();
    },

    showLoaded: function () {
        this.loadedElement.show();
    },

    hideLoaded: function () {
        this.loadedElement.hide();
    },

    showError: function () {
        this.errorElement.show();
    },

    hideError: function () {
        this.errorElement.hide();
    },

    setErrorMessage: function (val) {
        this.errorElement.text(val);
    },

    showRetry: function () {
        this.retryElement.show();
    },

    hideRetry: function () {
        this.retryElement.hide();
    },

    onRetryClick: function () {
        this.controller.load();
    }
});

