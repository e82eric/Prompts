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

  var promptItemControllersProvider = new PromptItemControllersProvider();
  var selectedItemsController = new SelectedItemsController(multiSelector, promptItemControllersProvider);
  var availableItemsController = new AsynchronousSearchAvailableItemsController(multiSelector);
  promptItemControllersProvider.setAvailableItemsController(selectedItemsController);
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
  
  var repository = new Repository("/Prompts.Service/api/prompts/child_items", loadingPanel, "POST");
  var searchRequester = new AsynchronousSearchRequester(searchStringParser, repository);

  return new AsynchronousSearchShoppingCartController(
    model,
    searchRequester,
    availableItemsController,
    selectedItemsController,
    loadingPanel,
    promptsController);
  }
});