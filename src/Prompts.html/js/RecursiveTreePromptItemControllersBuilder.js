var RecursiveTreePromptItemControllersBuilder = Class.extend({
    init: function(itemBuilder) {
        this.itemBuilder = itemBuilder;
    },

    build: function (promptLevel) {
        var result = [];
        _.each(
            promptLevel.AvailableItems,
            function (model) {
                var controller = this.itemBuilder.build(model);
                result.push(controller);
            },
            this
        );

        return result;
    }
});