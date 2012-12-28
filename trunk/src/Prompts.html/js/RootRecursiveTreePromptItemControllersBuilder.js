var RootRecursiveTreePromptItemControllersBuilder = Class.extend({
    init: function (promptName, selector) {
        this.promptName = promptName;
        this.selector = selector;
    },

    build: function (promptLevelInfo) {
        var result = [];

        var filterParameterName = promptLevelInfo.ParameterName;

        var availableItemsController = new AvailableItemsController(
            this.selector,
            new PromptItemControllersProvider());

        _.each(
            promptLevelInfo.AvailableItems,
            function (model) {
                var childItemsBuilder = new RecursiveTreePromptItemControllersBuilder(
                    this.promptName,
                    availableItemsController,
                    filterParameterName);
                var repository = new Repository(childItemsBuilder, "/Prompts.Service/api/prompts/child_items/recursive");
                var loadingPanel = new ChildPromptItemsLoadingPanelController(repository);

                var childItemsRequest = {
                    PromptName: this.promptName,
                    ParameterName: filterParameterName,
                    ParameterValue: {Name: filterParameterName, Value: model.Value}
                };

                var controller = new TreePromptItemController(
                    model,
                    availableItemsController,
                    loadingPanel,
                    childItemsRequest,
                    this.parentPromptItem);


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