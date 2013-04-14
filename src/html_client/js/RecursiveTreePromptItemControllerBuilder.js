var RecursiveTreePromptItemControllerBuilder = Class.extend({
    init: function(promptName, rootAvailableItemsController, filterParameterName) {
        this.promptName = promptName;
        this.rootAvailableItemsController = rootAvailableItemsController;
        this.filterParameterName = filterParameterName;
    },

    build: function (model) {
        var childAvailableItemsController = new ChildAvailableItemsController();

        var loadingPanel = new LoadingPanelControllerBase(function (controller) {
            return new ChildPromptItemsLoadingPanelView(controller, childAvailableItemsController.createView());
        });
        
        var childItemsBuilder = new RecursiveTreePromptItemControllersBuilder(this);
        
        var repository = new Repository("/Prompts.Service/api/prompts/child_items/recursive", loadingPanel, "POST");

        var parameterValue = {Name: this.filterParameterName, Value: model.Value};

        var childItemsRequest = {
            PromptName: this.promptName,
            ParameterName: this.filterParameterName,
            ParameterValue: parameterValue
        };

        var childItemsRequester = new ChildItemsRequester(repository, childItemsBuilder, childItemsRequest);
        



        var controller = new TreePromptItemController(
            model,
            this.rootAvailableItemsController,
            childItemsRequester,
            childAvailableItemsController,
            loadingPanel);

        return controller;
    }
});