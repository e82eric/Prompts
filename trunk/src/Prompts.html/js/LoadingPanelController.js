var LoadingPanelController = function (repository) {
    this.repository = repository;

    this.load = function () {
        var self = this;

        self.showLoading();

        self.repository.GetCatalog(
            function (view) {
                self.showLoaded(view);
            },
            function (errorMessage) {
                self.showError(errorMessage);
            }
        );
    };

    this.setView = function (val) {
        this.parentView = val;
    };

    this.showLoading = function() {
        this.parentView.showLoading();
        this.parentView.hideRetry();
        this.parentView.hideError();
        this.parentView.setErrorMessage("");
    };

    this.showLoaded = function (view) {
        this.parentView.hideLoading();
        this.parentView.showLoaded(view);
    };

    this.showError = function(errorMessage) {
        this.parentView.hideLoading();
        this.parentView.setErrorMessage(errorMessage);
        this.parentView.showError();
        this.parentView.showRetry();
    };
};