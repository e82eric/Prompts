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
        var selectedItemsController = new SelectedItemsController(multiSelector);
        var availableItemsController = new AvailableItemsController(
            multiSelector,
            promptItemControllersProvider);

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

        return new ShoppingCartController(availableItemsController, selectedItemsController);
    }
}