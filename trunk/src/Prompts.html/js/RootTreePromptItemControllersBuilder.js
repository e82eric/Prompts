var RootTreePromptItemControllersBuilder = Class.extend({
    init: function (promptName, selector) {
        this.promptName = promptName;
        this.selector = selector;
    },

    build: function (promptLevelInfo) {
        var result = [];

        var availableItemsController = new AvailableItemsController(
            this.selector,
            new PromptItemControllersProvider());

        _.each(
            promptLevelInfo.AvailableItems,
            function (model) {
                var childItemsBuilder = new TreePromptItemControllersBuilder(this.promptName, availableItemsController);
                var repository = new ChildPromptItemsRepository(childItemsBuilder);
                var loadingPanel = new ChildPromptItemsLoadingPanelController(repository);

                var childItemsRequest = {
                    PromptName: this.promptName,
                    ParameterName: promptLevelInfo.ParameterName,
                    ParameterValues: [{Name: promptLevelInfo.ParameterName, Value: model.Value}]
                };

                if(promptLevelInfo.HasChildLevel) {
                    controller = new TreePromptItemController(
                        model,
                        availableItemsController,
                        loadingPanel,
                        childItemsRequest,
                        this.parentPromptItem);
                } else {
                    controller = new LeafTreePromptItemController(
                        model,
                        availableItemsController,
                        loadingPanel,
                        childItemsRequest,
                        this.parentPromptItem);
                }

                childItemsBuilder.setParentPromptItem(controller);

                result.push(controller);
            },
            this
        );

        availableItemsController.setItems(result);

        return availableItemsController;
    },

    setAvailableItemsController: function (val) {
        this.availableItemsController = val;
    }
});