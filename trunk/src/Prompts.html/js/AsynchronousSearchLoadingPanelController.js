var AsynchronousSearchLoadingPanelController = Class.extend({
    init: function () {
    },

    showLoading: function() {
        this.view.showLoading();
        this.view.hideRetry();
        this.view.hideError();
        this.view.hideLoaded();
        this.view.setErrorMessage("");
    },

    showLoaded: function () {
        this.view.hideLoading();
        this.view.showLoaded();
    },

    showError: function(errorMessage) {
        this.view.hideLoading();
        this.view.setErrorMessage(errorMessage);
        this.view.showError();
        this.view.showRetry();
    },

    setView: function (val) {
        this.view = val;
        this.view.hideLoaded();
        this.view.hideLoading();
        this.view.hideRetry();
    },

    createView: function (availableItemsView, selectedItemsView) {
        this.setView(new AsynchronousSearchLoadingPanelView(this, availableItemsView, selectedItemsView));
        return this.view;
    }
});