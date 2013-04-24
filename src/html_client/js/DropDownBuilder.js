function DropDownBuilder () {
    this.build = function (model) {
        var singleSelector = new SingleSelector();
        var selector = new DropDownSelector(singleSelector );

        var availableItemsController = new AvailableItemsController(selector);
        var itemsBuilder = new PromptItemControllersProvider();
        itemsBuilder.setAvailableItemsController(availableItemsController);
        var availableItemControllers = itemsBuilder.get(model.PromptLevelInfo.AvailableItems);

        availableItemsController.setItems(availableItemControllers);

        var dropDownController = new DropDownController(model, availableItemsController);
        selector.setPromptController(dropDownController);

        return dropDownController;
    }
}