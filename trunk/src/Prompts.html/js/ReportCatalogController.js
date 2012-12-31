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

    createView: function () {
        var itemsView = this.itemsController.createView();
        var loadingPanelView = this.loadingPanelController.createView(itemsView);
        this.setView(new ReportCatalogView(this, loadingPanelView));
        return this.view;
    }
});

