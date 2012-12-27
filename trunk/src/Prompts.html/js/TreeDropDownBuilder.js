function TreeDropDownBuilder () {
    this.build = function (model) {
        var singleSelector = new SingleSelector();
        var dropDownSelector = new TreeDropDownSelector(singleSelector, new HierarchyFlattener());
        var itemsBuilder = new RootTreePromptItemControllersBuilder(model.Name, dropDownSelector);
        var availableItemsController = itemsBuilder.build(model.PromptLevelInfo);
        var dropDownController = new TreeDropDownController(availableItemsController);
        dropDownSelector.setPromptController(dropDownController);
        return dropDownController;
    }
}