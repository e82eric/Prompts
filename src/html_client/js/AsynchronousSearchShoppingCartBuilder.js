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

    var selectedItemControllerBuilder = new PromptItemControllerBuilder();
    var selectedItemControllersBuilder = new ItemsBuilder(selectedItemControllerBuilder);
    var selectedItemsController = new SelectedItemsController(multiSelector, selectedItemControllersBuilder);
    selectedItemControllerBuilder.setAvailableItemsController(selectedItemsController);
    var availableItemsController = new AsynchronousSearchAvailableItemsController(multiSelector);

    var searchStringParser = new AsynchronousSearchStringParser(
      new SearchStringParser(),
      model.Name,
      model.PromptLevelInfo.ParameterName);

    var loadingPanel = new LoadingPanelControllerBase(function (controller) { 
      return new AsynchronousSearchLoadingPanelView(
        controller, 
        availableItemsController.createView(), 
        selectedItemsController.createView()); 
    });

    var availableItemControllerBuilder = new PromptItemControllerBuilder();
    var availableItemControllersBuilder = new ItemsBuilder(availableItemControllerBuilder);
    availableItemControllerBuilder.setAvailableItemsController(availableItemsController);

    var repository = new Repository("/Prompts.Service/api/prompts/child_items", loadingPanel, "POST");
    var searchRequester = new AsynchronousSearchRequester(searchStringParser, repository, availableItemControllersBuilder);

    var controller = new AsynchronousSearchShoppingCartController(
      model,
      searchRequester,
      availableItemsController,
      selectedItemsController,
      loadingPanel,
      promptsController);

    if(model.DefaultValues.length > 0) {
      selectedItemsController.setDefaults(model.DefaultValues);
      loadingPanel.setIsLoaded(true);
      controller.onSearchStringSet(model.DefaultValues[0].Label);
    }

    return controller;
  }
});