var ReportCatalogPanelView = function(model) {
    this.controller = model;

    this.showLoading = function () {
        this.loadingElement.show();
    };

    this.hideLoading = function () {
        this.loadingElement.hide();
    };

    this.showLoaded = function (view) {
        this.loadedElement.show();
        this.loadedElement.html(view.render());
    };

    this.hideLoaded = function () {
        this.loadedElement.hide();
    };

    this.showError = function () {
        this.errorElement.show();
    };

    this.hideError = function () {
        this.errorElement.hide();
    };

    this.setErrorMessage = function (val) {
        this.errorElement.text(val);
    };

    this.showRetry = function () {
        this.retryElement.show();
    };

    this.hideRetry = function () {
        this.retryElement.hide();
    };

    this.onRetryClick = function () {
        this.controller.load();
    };

    this.render = function(){
        this.loadingElement = $("#reportCatalog").find("#loading");
        this.loadedElement = $("#reportCatalog").find("#loaded");
        this.retryElement = $("#reportCatalog").find("#retry");
        this.errorElement = $("#reportCatalog").find("#errorMessage");

        this.retryElement.click($.proxy(this.onRetryClick, this));
        this.controller.setView(this);
        this.controller.load();
    }
}

