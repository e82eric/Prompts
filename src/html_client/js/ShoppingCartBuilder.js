function ShoppingCartBuilder () {
    this.build = function (model) {
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
        var availableItemsController = new AvailableItemsController(multiSelector);
        promptItemControllersProvider.setAvailableItemsController(selectedItemsController);

        _.each(
            model.PromptLevelInfo.AvailableItems,
            function (availableItem) {
                var controller = new PromptItemController(availableItem, availableItemsController);
                availableItemControllers.push(controller);
            },
            this
        );

        availableItemsController.setItems(availableItemControllers);

        var shoppingCartController = new MultiSelectPromptController(model, availableItemsController, selectedItemsController, function () {
            return new SearchableShoppingCartView(shoppingCartController);
        });

        return shoppingCartController;
    }
}