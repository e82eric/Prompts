var ReportCatalogLoadingPanelController = LoadingPanelControllerBase.extend({
    createView: function (itemsControllerView) {
        this.setView(new ReportCatalogLoadingPanelView(this, itemsControllerView));
        return this.view;
    }
});