function TreePromptItemControllersBuilder (promptName, rootAvailableItemsController) {
    this.promptName = promptName;
    this.rootAvailableItemsController = rootAvailableItemsController;

    this.build = function (promptLevelInfo) {
        var result = [];

        _.each(
            promptLevelInfo.AvailableItems,
            function (model) {
                var childItemsBuilder = new TreePromptItemControllersBuilder(this.promptName, this.rootAvailableItemsController);
                var repository = new ChildPromptItemsRepository(childItemsBuilder);
                var loadingPanel = new ChildPromptItemsLoadingPanelController(repository);

                var parameterValues = [];

                _.each(
                    this.parentPromptItem.childPromptItemsRequest.ParameterValues,
                    function (parameterValue) {
                        parameterValues.push(parameterValue);
                    },
                    this
                );

                var parameterValue = {Name: promptLevelInfo.ParameterName, Value: model.Value};
                parameterValues.push(parameterValue);

                var childItemsRequest = {
                    PromptName: this.promptName,
                    ParameterName: promptLevelInfo.ParameterName,
                    ParameterValues: parameterValues
                };

                var controller = undefined;

                if(promptLevelInfo.HasChildLevel) {
                    controller = new TreePromptItemController(
                        model,
                        this.rootAvailableItemsController,
                        loadingPanel,
                        childItemsRequest,
                        this.parentPromptItem);
                } else {
                    controller = new LeafTreePromptItemController(
                        model,
                        this.rootAvailableItemsController,
                        loadingPanel,
                        childItemsRequest,
                        this.parentPromptItem);
                }

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