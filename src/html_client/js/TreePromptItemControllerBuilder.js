var TreePromptItemControllerBuilder = Class.extend({
    init: function(promptName, rootAvailableItemsController, parentParameterValues) {
        this.promptName = promptName;
        this.rootAvailableItemsController = rootAvailableItemsController;
        this.parentParameterValues = parentParameterValues;
    },

    build: function (model, promptLevel){
        var parameterValues = [];

        _.each(
           this.parentParameterValues,
           function (parameterValue) {
               parameterValues.push(parameterValue);
           },
           this
        );

        var parameterValue = {Name: promptLevel.ParameterName, Value: model.Value};
        parameterValues.push(parameterValue);

        var childItemBuilder = new TreePromptItemControllerBuilder(
            this.promptName,
            this.rootAvailableItemsController,
            parameterValues);

        var childItemsBuilder = new TreePromptItemControllersBuilder(childItemBuilder);

        var loadingPanel = new LoadingPanelControllerBase(function (controller) {
          return new ChildPromptItemsLoadingPanelView(controller, childAvailableItemsController.createView());
        });

        var repository = new Repository("/Prompts.Service/api/prompts/child_items", loadingPanel, "POST");

        var childItemsRequest = {
           PromptName: this.promptName,
           ParameterName: promptLevel.ParameterName,
           ParameterValues: parameterValues
        };

        var childItemsRequester = new ChildItemsRequester(repository, childItemsBuilder, childItemsRequest);
        var childAvailableItemsController = new AsynchronousItemsController(function () {
          return new ItemsView(childAvailableItemsController, "childItems")
        });
        var controller = undefined;

        if(promptLevel.HasChildLevel) {
           controller = new TreePromptItemController(
               model,
               this.rootAvailableItemsController,
               childItemsRequester,
               childAvailableItemsController,
               loadingPanel);
        } else {
           controller = new LeafTreePromptItemController(
               model,
               this.rootAvailableItemsController);
        }

        return controller;
   }
});