var ChildItemsRequester = Class.extend({
    init: function (repository, builder, childItemsRequest) {
        this.repository = repository;
        this.builder = builder;
        this.childItemsRequest = childItemsRequest;
    },

    execute: function (childAvailableItemsController, promptItem) {
        var self = this;
        this.repository.get(
            this.childItemsRequest,
            function(result) {
                var controllers = self.builder.build(result.AvailableItems, {promptLevelInfo: result});
                childAvailableItemsController.setItems(controllers);
                promptItem.setChildren(controllers);
            }
        )
    }
});