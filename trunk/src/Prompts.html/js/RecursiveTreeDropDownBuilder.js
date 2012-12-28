function RecursiveTreeDropDownBuilder () {
    this.build = function (model) {
        var singleSelector = new SingleSelector();
        var dropDownSelector = new TreeDropDownSelector(singleSelector, new HierarchyFlattener());
        var itemsBuilder = new RootRecursiveTreePromptItemControllersBuilder(model.Name, dropDownSelector);
        var availableItemsController = itemsBuilder.build(model.PromptLevelInfo);
        var dropDownController = new TreeDropDownController(availableItemsController);
        dropDownSelector.setPromptController(dropDownController);
        return dropDownController;
    }
}