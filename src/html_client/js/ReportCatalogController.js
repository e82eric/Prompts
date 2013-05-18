var ReportCatalogController = Class.extend({
    init: function (reportCatalogRequester, itemsController, loadingPanelController) {
        this.reportCatalogRequester = reportCatalogRequester;
        this.itemsController = itemsController;
        this.loadingPanelController = loadingPanelController;
    },

    setView: function(val) {
        this.view = val;
        this.reportCatalogRequester.execute(this.itemsController);
    },

    onRetryClick: function () {
        this.reportCatalogRequester.execute(this.itemsController);
    },

    createView: function () {
        var itemsView = this.itemsController.createView(function (controller) { return new ItemsView(this, "rootItems") ;} );
        var loadingPanelView = this.loadingPanelController.createView(itemsView);
        this.setView(new ReportCatalogView(this, loadingPanelView));
        return this.view;
    }
});

