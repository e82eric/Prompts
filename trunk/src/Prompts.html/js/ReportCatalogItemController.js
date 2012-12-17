var ReportCatalogItemController = function(model, promptsController) {
    this.model = model;
    this.promptsController = promptsController;

    this.setRepository = function(val) {
        this.reportCatalog = val;
    };

    this.changeSelect = function() {
        this.reportCatalog.Select(this);
    };

    this.Select = function () {
        this.view.onSelected();
        this.promptsController.load({Path: model.Path});
    };

    this.UnSelect = function () {
        this.view.onUnSelected();
    };

    this.CreateView = function() {
        this.setView(new ReportCatalogItemView(this));
        return this.view;
    };

    this.setView = function (val) {
        this.view = val;
    }
};