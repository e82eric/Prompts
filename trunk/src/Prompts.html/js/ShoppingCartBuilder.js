function ShoppingCartBuilder () {
    this.build = function (model) {
        var availableItemControllers = [];
        var singleSelector = new SingleSelector();
        var availableItemsController = new AvailableItemsController(singleSelector);

        _.each(
            model.PromptLevelInfo.AvailableItems,
            function (availableItem) {
                var controller = new PromptItemController(availableItem, availableItemsController);
                availableItemControllers.push(controller);
            },
            this
        );

        availableItemsController.setItems(availableItemControllers);

        return new ShoppingCartController(availableItemsController);
    }
}