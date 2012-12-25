function DropDownBuilder () {
    this.build = function (model) {
        var availableItemControllers = [];
        var singleSelector = new SingleSelector();
        var selector = new DropDownSelector(singleSelector );

        var availableItemsController = new AvailableItemsController(
            selector,
            undefined);

        _.each(
            model.PromptLevelInfo.AvailableItems,
            function (availableItem) {
                var controller = new PromptItemController(availableItem, availableItemsController);
                availableItemControllers.push(controller);
            },
            this
        );

        availableItemsController.setItems(availableItemControllers);

        var dropDownController = new DropDownController(availableItemsController);
        selector.setPromptController(dropDownController);

        return dropDownController;
    }
}