var ChildPromptItemsLoadingPanelView = TemplateView.extend({
    init: function (controller, availableItemsView) {
        this._super(controller, "childPromptItemsLoadingPanelTemplate");
        this.loadingElement = this.root.filter("#loading");
        this.loadedElement = this.root.filter("#loaded");
        this.retryElement = this.root.filter("#retry");
        this.errorElement = this.root.filter("#errorMessage");
        this.loadedElement.html(availableItemsView.render());
    },

    showLoading: function () {
        this.loadingElement.show();
    },

    hideLoading: function () {
        this.loadingElement.hide();
    },

    showLoaded: function (controller) {
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

    render: function(){
        return this.root;
    }
});