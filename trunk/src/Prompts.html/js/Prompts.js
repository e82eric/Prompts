$(function () {
    var reportCatalogBuilder = new ReportCatalogBuilder();
    var catalogRepository = new CatalogRepository(reportCatalogBuilder);
    var loadingPanel = new LoadingPanel(catalogRepository);

    var view = new ReportCatalogPanelView(loadingPanel);
    loadingPanel.setView(view);
    view.render();
});