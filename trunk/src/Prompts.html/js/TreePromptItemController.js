var TreePromptItemController = RootTreePromptItemController.extend({
    init: function (
        model,
        availableItemsController,
        childPromptItemsLoadingPanel,
        childPromptItemsRequest,
        parentPromptItem) {

        this._super(
            model,
            availableItemsController,
            childPromptItemsLoadingPanel,
            childPromptItemsRequest
        );
        this.parentPromptItem = parentPromptItem;
    }
});