var ChildPromptItemsLoadingPanelController = Class.extend({
    init: function (repository) {
        this.repository = repository;
    },

    load: function (request) {
        var self = this;

        self.showLoading();

        self.repository.Get(
            request,
            function (view) {
                self.showLoaded(view);
            },
            function (errorMessage) {
                self.showError(errorMessage);
            }
        );
    },

    setView: function (val) {
        this.view = val;
        this.view.hideLoading();
        this.view.hideRetry();
    },

    showLoading: function() {
        this.view.showLoading();
        this.view.hideRetry();
        this.view.hideError();
        this.view.hideLoaded();
        this.view.setErrorMessage("");
    },

    showLoaded: function (view) {
        this.view.hideLoading();
        this.view.showLoaded(view);
    },

    showError: function(errorMessage) {
        this.view.hideLoading();
        this.view.setErrorMessage(errorMessage);
        this.view.showError();
        this.view.showRetry();
    },

    createView: function () {
        this.setView(new ChildPromptItemsLoadingPanelView(this));
        return this.view;
    }
});