$(function () {
    var promptsController = new PromptsController(new ItemsDisposer());
    var promptBuilder = new PromptControllerBuilder();
    var promptsBuilder = new ItemsBuilder(promptBuilder);
    var promptingLoadingPanel = new LoadingPanelController(function (controller){
        return new PromptingLoadingPanelView(
            promptingLoadingPanel, 
            promptsController.createView(function (pController) { return new PromptsView(pController) }));
    });
    var promptsRepository = new Repository("/prompts.service/api/prompts", promptingLoadingPanel, "POST");
    var promptsRequester = new PromptsRequester(promptsRepository, promptsBuilder);
    
    var reportRenderer = new ReportRenderer();

    var executeReportController = new ExecuteReportController(reportRenderer);
    
    var executeReportRepository = new Repository(
        "/prompts.service/api/prompts/set_prompt_selections", 
        executeReportController, 
        "POST",
        "text");

    var promptingController = new PromptingController(
        promptsRequester, 
        promptsController, 
        promptingLoadingPanel,
        executeReportController);

    executeReportController.setPromptingController(promptingController);
    executeReportController.setRepository(executeReportRepository);

    promptBuilder.setPromptingController(promptingController);

    var promptingView = promptingController.createView();
    promptingView.render();

    var reportCatalogBuilder = new ReportCatalogBuilder(promptingController);
    reportCatalogController = reportCatalogBuilder.build();
    reportCatalogView = reportCatalogController.createView();
    reportCatalogView.render();
});
