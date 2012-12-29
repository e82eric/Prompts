var AsynchronousSearchShoppingCartBuilder = Class.extend({
   build: function (model) {
       var availableItemControllers = [];
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

       var loadingPanel = new AsynchronousSearchLoadingPanelController();

       var repository = new Repository2("/Prompts.Service/api/prompts/child_items", loadingPanel);

       var search = new AsynchronousSearch(searchStringParser, repository);

       _.each(
           model.PromptLevelInfo.AvailableItems,
           function (availableItem) {
               var controller = new PromptItemController(availableItem, availableItemsController);
               availableItemControllers.push(controller);
           },
           this
       );

       return new AsynchronousSearchShoppingCartController(
           model,
           search,
           availableItemsController,
           selectedItemsController,
           loadingPanel);
   }
});