var AsynchronousSearchShoppingCartBuilder = Class.extend({
  build: function (model, promptsController) {
    var singleSelector = new SingleSelector();
    var rangeSelector = new RangeSelector();
    var inverseSelector = new InverseSelector();
    var multiSelector = new MultiSelector(
      singleSelector,
      rangeSelector,
      inverseSelector
    );

    var availableItemControllerBuilder = new PromptItemControllerBuilder();
    var availableItemControllersBuilder = new ItemsBuilder(availableItemControllerBuilder);

    var loadingPanel = new LoadingPanelControllerBase(function (controller) { 
      return new AsynchronousSearchLoadingPanelView(
        controller, 
        availableItemsController.createView(function (controller) { return new ItemsView(controller, "rootItems"); } ), 
        selectedItemsController.createView()); 
    });

    var searchStringParser = new AsynchronousSearchStringParser(
      new SearchStringParser(),
      model.Name,
      model.PromptLevelInfo.ParameterName);

    var repository = new Repository("/Prompts.Service/api/prompts/child_items", loadingPanel, "POST");
    var searchRequester = new ServerSideSearch(searchStringParser, repository, availableItemControllersBuilder);

    var availableItemsController = new AvailableItemsController(
      multiSelector,
      searchRequester,
      new ItemsDisposer());

    var selectedItemControllerBuilder = new PromptItemControllerBuilder();
    var selectedItemControllersBuilder = new ItemsBuilder(selectedItemControllerBuilder);
    var selectedItemsController = new SelectedItemsController(multiSelector, selectedItemControllersBuilder);
    selectedItemControllerBuilder.setAvailableItemsController(selectedItemsController);

    availableItemControllerBuilder.setAvailableItemsController(availableItemsController);

    var controller = new MultiSelectPromptController(
      model,
      availableItemsController,
      selectedItemsController,
      promptsController,
      function (controller) {
        return new AsynchronousSearchShoppingCartView(controller, loadingPanel.createView());
      });

    if(model.DefaultValues.length > 0) {
      selectedItemsController.setDefaults(model.DefaultValues);
      loadingPanel.setIsLoaded(true);
      controller.onSearchStringSet(model.DefaultValues[0].Label);
    }

    return controller;
  }
});
