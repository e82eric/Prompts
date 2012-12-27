function PromptItemControllersProvider () {
    this.get = function (models) {
        var result = [];

        _.each(
            models,
            function (model) {
                var controller = new PromptItemController(model, this.availableItemsController);
                result.push(controller);
            },
            this
        );

        return result;
    };

    this.setAvailableItemsController = function (val) {
        this.availableItemsController = val;
    };
}