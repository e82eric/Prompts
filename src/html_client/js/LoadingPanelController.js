var LoadingPanelController = Class.extend({
    init: function (createViewFunc) {
        this.createViewFunc = createViewFunc;
    },

    setIsLoaded: function (val) {
        this.isLoaded = val;
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
        this.view.hideRetry();
        if(this.isLoaded === true) {
            this.showLoaded();
        } else {
            this.view.hideLoaded();
            this.view.hideLoading();
        }
    },

    createView: function () {
        var view = this.createViewFunc(this);
        this.setView(view);
        return view;
    },

    hideAll: function () {
        this.view.hideLoaded();
        this.view.hideLoading();
        this.view.hideRetry();
    },
});
