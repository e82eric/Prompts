$(function () {
    var promptsBuilder = new PromptsViewBuilder();
    var promptsRepository = new PromptsRepository(promptsBuilder);
    var promptsPanelView = new PromptsPanelView();
    var promptsPanelController = new LoadingPanelController(promptsRepository);
    promptsPanelView.render();
    promptsPanelController.setView(promptsPanelView);


    var reportCatalogBuilder = new ReportCatalogBuilder(promptsPanelController);
    var catalogRepository = new CatalogRepository(reportCatalogBuilder);
    var loadingPanel = new LoadingPanelController(catalogRepository);

    var view = new ReportCatalogPanelView(loadingPanel);
    view.render();
    loadingPanel.setView(view);

    loadingPanel.load({});
});