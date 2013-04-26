function DropDownBuilder () {
    this.build = function (model, promptsController) {
        var singleSelector = new SingleSelector();
        var selector = new DropDownSelector(singleSelector );

        var availableItemsController = new AvailableItemsController(selector);
        var itemsBuilder = new PromptItemControllersProvider();
        itemsBuilder.setAvailableItemsController(availableItemsController);
        var availableItemControllers = itemsBuilder.get(model.PromptLevelInfo.AvailableItems);

        availableItemsController.setItems(availableItemControllers);

        var dropDownController = new SingleSelectPromptController(
            model, 
            availableItemsController, 
            promptsController, 
            function () {
                return new SearchableDropDownView(dropDownController);
            }
        );
        selector.setPromptController(dropDownController);

        return dropDownController;
    }
}