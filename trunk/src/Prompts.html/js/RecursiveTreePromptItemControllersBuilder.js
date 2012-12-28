function RecursiveTreePromptItemControllersBuilder (promptName, rootAvailableItemsController, filterParameterName) {
    this.promptName = promptName;
    this.rootAvailableItemsController = rootAvailableItemsController;
    this.filterParameterName = filterParameterName;

    this.build = function (promptLevelInfo) {
        var result = [];

        _.each(
            promptLevelInfo.PromptLevel.AvailableItems,
            function (model) {
                var childItemsBuilder = new RecursiveTreePromptItemControllersBuilder(
                    this.promptName,
                    this.rootAvailableItemsController,
                    this.filterParameterName);
                var repository = new Repository(childItemsBuilder, "/Prompts.Service/api/prompts/child_items/recursive");
                var loadingPanel = new ChildPromptItemsLoadingPanelController(repository);

                var parameterValue = {Name: this.filterParameterName, Value: model.Value};

                var childItemsRequest = {
                    PromptName: this.promptName,
                    ParameterName: filterParameterName,
                    ParameterValue: parameterValue
                };

                var controller = controller = new TreePromptItemController(
                    model,
                    this.rootAvailableItemsController,
                    loadingPanel,
                    childItemsRequest,
                    this.parentPromptItem);

                childItemsBuilder.setParentPromptItem(controller);

                result.push(controller);
            },
            this
        );

        var availableItemsController = new ChildAvailableItemsController();

        availableItemsController.setItems(result);

        this.parentPromptItem.setChildren(result);

        return availableItemsController;
    };

    this.setAvailableItemsController = function (val) {
        this.availableItemsController = val;
    };

    this.setParentPromptItem = function (val) {
        this.parentPromptItem = val;
    };
}