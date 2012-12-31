var ReportCatalogBuilder = Class.extend({
    init: function(promptsController) {
        this.promptsController = promptsController;
    },

    build: function() {
        var selector = new TreeSingleSelector(new SingleSelector(), new HierarchyFlattener());
        var rootItemsController = new RootReportCatalogItemsController(selector);

        var folderCatalogItemBuilder = new FolderCatalogItemBuilder();
        var catalogItemBuilder = new ReportCatalogItemBuilder(
            folderCatalogItemBuilder,
            this.promptsController,
            rootItemsController);

        var catalogItemsBuilder = new ReportCatalogItemsBuilder(catalogItemBuilder);
        folderCatalogItemBuilder.setChildItemsBuilder(catalogItemsBuilder);

        var loadingPanel = new ReportCatalogLoadingPanelController();

        var repository = new Repository("/Prompts.Service/api/reports", loadingPanel, "GET");

        var reportCatalogRequester = new ReportCatalogRequester(repository, catalogItemsBuilder);

        return new ReportCatalogController(reportCatalogRequester, rootItemsController, loadingPanel);
    }
});