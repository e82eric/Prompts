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

        var promptItemControllersProvider = new RootRecursiveTreePromptItemControllersBuilder(model.Name, treeShoppingCartSelector);
        var selectedItemControllersProvider = new PromptItemControllersProvider();

        var selectedItemsController = new SelectedItemsController(multiSelector, selectedItemControllersProvider);

        selectedItemControllersProvider.setAvailableItemsController(selectedItemsController);
        var availableItemsController = promptItemControllersProvider.build(model.PromptLevelInfo);

        return new TreeShoppingCartController(availableItemsController, selectedItemsController);
    }
}