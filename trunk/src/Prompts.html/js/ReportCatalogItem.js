var ReportCatalogItem = function(model) {
    this.model = model;
    this.onSelected = undefined;
    this.onUnSelected = undefined;
    this.selected = false;
    this.reportCatalog = undefined;

    this.setReportCatalog = function(val) {
        this.reportCatalog = val;
    };

    this.changeSelect = function() {
        this.reportCatalog.Select(this);
    };

    this.Select = function () {
        this.view.onSelected();
    };

    this.UnSelect = function () {
        this.view.onUnSelected();
    };

    this.CreateView = function() {
        this.setView(new CatalogItemView("#itemTemplate", this));
        return this.view;
    };

    this.setView = function (val) {
        this.view = val;
    }
};