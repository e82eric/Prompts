function TreeShoppingCartBuilder () {
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

        var itemBuilder = new TreePromptItemControllerBuilder(model.Name, availableItemsController, []);
        var promptItemControllersProvider = new TreePromptItemControllersBuilder(itemBuilder);
        var items = promptItemControllersProvider.build(model.PromptLevelInfo);

        availableItemsController.setItems(items);

        var selectedItemControllersProvider = new PromptItemControllersProvider();

        var selectedItemsController = new SelectedItemsController(multiSelector, selectedItemControllersProvider);

        selectedItemControllersProvider.setAvailableItemsController(selectedItemsController);

        var shoppingCartController = new MultiSelectPromptController(model, availableItemsController, selectedItemsController, function () {
            return new ShoppingCartView(shoppingCartController, "treeShoppingCartTemplate");
        });

        return shoppingCartController;
    }
}