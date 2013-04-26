function RecursiveTreeDropDownBuilder () {
    this.build = function (model, promptsController) {
        var singleSelector = new SingleSelector();
        var dropDownSelector = new TreeDropDownSelector(singleSelector, new HierarchyFlattener());

        var rootAvailableItemsController = new AvailableItemsController(dropDownSelector);

        var filterParameterName = model.PromptLevelInfo.ParameterName;

        var itemBuilder = new RecursiveTreePromptItemControllerBuilder(
            model.Name,
            rootAvailableItemsController,
            filterParameterName);

        var itemsBuilder = new RecursiveTreePromptItemControllersBuilder(itemBuilder);
        var items = itemsBuilder.build(model.PromptLevelInfo);
        rootAvailableItemsController.setItems(items);

        var dropDownController = new SingleSelectPromptController(
            model, 
            rootAvailableItemsController,
            promptsController,
            function () {
                return new DropDownView(dropDownController, "treeDropDownTemplate");
            }
        );
        
        dropDownSelector.setPromptController(dropDownController);
        return dropDownController;
    }
}