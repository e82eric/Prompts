$(function () {
    var promptsBuilder = new PromptsViewBuilder();
    var promptsRepository = new PromptsRepository(promptsBuilder);
    var promptsPanelView = new PromptsPanelView();
    var promptsPanelController = new LoadingPanelController(promptsRepository);
    promptsPanelView.render();
    promptsPanelController.setView(promptsPanelView);

    var reportCatalogBuilder = new ReportCatalogBuilder(promptsPanelController);
    reportCatalogController = reportCatalogBuilder.build();
    reportCatalogView = reportCatalogController.createView();
    reportCatalogView.render();
});