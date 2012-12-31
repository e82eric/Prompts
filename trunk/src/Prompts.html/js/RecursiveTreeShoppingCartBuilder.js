function RecursiveTreeShoppingCartBuilder () {
    this.build = function (model) {
        var singleSelector = new SingleSelector();
        var rangeSelector = new RangeSelector();
        var inverseSelector = new InverseSelector();
        var multiSelector = new MultiSelector(
            singleSelector,
            rangeSelector,
            inverseSelector
        );
        var hierarhcyFlattener = new HierarchyFlattener();
        var treeShoppingCartSelector = new TreeShoppingCartSelector(multiSelector, hierarhcyFlattener);

        var availableItemsController = new AvailableItemsController(treeShoppingCartSelector);

        var filterParameterName = model.PromptLevelInfo.ParameterName;

        var itemBuilder = new RecursiveTreePromptItemControllerBuilder(
            model.Name,
            rootAvailableItemsController,
            filterParameterName);

        var promptItemControllersProvider = new RecursiveTreePromptItemControllersBuilder(itemBuilder);
        var items = promptItemControllersProvider.build(model.PromptLevelInfo);

        availableItemsController.setItems(items);

        var selectedItemControllersProvider = new PromptItemControllersProvider();

        var selectedItemsController = new SelectedItemsController(multiSelector, selectedItemControllersProvider);

        selectedItemControllersProvider.setAvailableItemsController(selectedItemsController);

        return new TreeShoppingCartController(availableItemsController, selectedItemsController);
    }
}