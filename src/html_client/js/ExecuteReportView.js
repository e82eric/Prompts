var ExecuteReportView = TemplateView.extend({
    init: function(controller) {
        this._super(controller, "executeReportTemplate");

        this.loadingElement = this.root.filter(".loading-image");
        this.errorElement = this.root.filter(".errorMessage");
        this.button = this.root.filter("button");
    },

    render: function(){
        this.button.click($.proxy(this.controller.execute, this.controller));
        return this.root;
    },

    disable: function () {
        this.button.attr('disabled','disabled');
    },

    enable: function () {
        this.button.removeAttr('disabled');
    },

    showLoading: function () {
        this.loadingElement.show();
    },

    hideLoading: function () {
        this.loadingElement.hide();
    },

    showLoaded: function () {
    },

    hideLoaded: function () {
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
    },

    hideRetry: function () {
    }
});
