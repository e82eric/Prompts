function TreeDropDownBuilder () {
    this.build = function (model) {
        var singleSelector = new SingleSelector();
        var dropDownSelector = new TreeDropDownSelector(singleSelector, new HierarchyFlattener());

        var availableItemsController = new AvailableItemsController(dropDownSelector);
        var itemBuilder = new TreePromptItemControllerBuilder(model.Name, availableItemsController, []);
        var itemsBuilder = new TreePromptItemControllersBuilder(itemBuilder);
        var items = itemsBuilder.build(model.PromptLevelInfo);
        availableItemsController.setItems(items);

        var dropDownController = new TreeDropDownController(model, availableItemsController);
        dropDownSelector.setPromptController(dropDownController);
        return dropDownController;
    }
}