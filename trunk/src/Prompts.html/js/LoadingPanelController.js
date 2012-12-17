var LoadingPanelController = function (repository) {
    this.repository = repository;

    this.load = function (request) {
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
    };

    this.setView = function (val) {
        this.view = val;

        this.view.hideLoading();
        this.view.hideRetry();
    };

    this.showLoading = function() {
        this.view.showLoading();
        this.view.hideRetry();
        this.view.hideError();
        this.view.hideLoaded();
        this.view.setErrorMessage("");
    };

    this.showLoaded = function (view) {
        this.view.hideLoading();
        this.view.showLoaded(view);
    };

    this.showError = function(errorMessage) {
        this.view.hideLoading();
        this.view.setErrorMessage(errorMessage);
        this.view.showError();
        this.view.showRetry();
    };
};