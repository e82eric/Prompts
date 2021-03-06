var RecursiveTreePromptItemControllerBuilder = Class.extend({
    init: function(promptName, rootAvailableItemsController, filterParameterName) {
        this.promptName = promptName;
        this.rootAvailableItemsController = rootAvailableItemsController;
        this.filterParameterName = filterParameterName;
    },

    build: function (params) {
        var model = params.model;

        var childAvailableItemsController = new AsynchronousItemsController(function () {
            return new ItemsView(childAvailableItemsController, "childItems");
        });

        var loadingPanel = new LoadingPanelController(function (controller) {
            return new ChildPromptItemsLoadingPanelView(controller, childAvailableItemsController.createView());
        });
        
        var childItemsBuilder = new ItemsBuilder(this);
        
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
    },

    setAvailableItemsController: function (val) {
        this.rootAvailableItemsController = val;
    }   
});
