var ReportCatalogBuilder = Class.extend({
    init: function(promptsController) {
        this.promptsController = promptsController;
    },

    build: function() {
        var selector = new TreeSingleSelector(new SingleSelector(), new HierarchyFlattener());
        var rootItemsController = new SelectableItemsController(selector, new ItemsDisposer());
        var folderCatalogItemBuilder = new FolderCatalogItemBuilder();
        var catalogItemBuilder = new ReportCatalogItemBuilder(
            folderCatalogItemBuilder,
            this.promptsController,
            rootItemsController);

        var catalogItemsBuilder = new ItemsBuilder(catalogItemBuilder);
        folderCatalogItemBuilder.setChildItemsBuilder(catalogItemsBuilder);

        var loadingPanel = new LoadingPanelController(function (controller) {
            return new PromptingLoadingPanelView(
                loadingPanel, 
                rootItemsController.createView(function (controller) { return new ItemsView(this, "rootItems") ;}));
        });

        var repository = new Repository("/Prompts.Service/api/reports", loadingPanel, "GET");

        var reportCatalogRequester = new ReportCatalogRequester(repository, catalogItemsBuilder);

        return new ReportCatalogController(reportCatalogRequester, rootItemsController, loadingPanel);
    }
});
