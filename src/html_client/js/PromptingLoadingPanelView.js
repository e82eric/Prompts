var PromptingLoadingPanelView = TemplateView.extend({
    init: function(controller, itemsControllerView) {
        this._super(controller, "promptingLoadingPanelTemplate");

        this.retryElement = this.root.filter(".retry");
        this.loadingElement = this.root.filter(".loading");
        this.loadedElement = this.root.filter(".loaded");
        this.errorElement = this.root.filter(".errorMessage");

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
    }
});

