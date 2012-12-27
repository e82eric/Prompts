var ChildPromptItemsLoadingPanelView = Class.extend({
    init: function (controller) {
        this.controller = controller;

        var template = $("#childPromptItemsLoadingPanelTemplate").html();
        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root = $(templateHtml);
        this.loadingElement = this.root.find("#loading");
        this.loadedElement = this.root.find("#loaded");
        this.retryElement = this.root.find("#retry");
        this.errorElement = this.root.find("#errorMessage");
    },

    showLoading: function () {
        this.loadingElement.show();
    },

    hideLoading: function () {
        this.loadingElement.hide();
    },

    showLoaded: function (controller) {
        this.loadedElement.show();
        this.loadedElement.html(controller.createView().render());
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
    },

    render: function(){
        this.retryElement.click($.proxy(this.onRetryClick, this));
        return this.root;
    }
});