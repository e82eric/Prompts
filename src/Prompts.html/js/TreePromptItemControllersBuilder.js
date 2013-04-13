var TreePromptItemControllersBuilder = Class.extend({
    init: function (itemBuilder) {
        this.itemBuilder = itemBuilder;
    },

    build: function (promptLevelInfo) {
        var result = [];

        _.each(
            promptLevelInfo.AvailableItems,
            function (model) {
                var controller = this.itemBuilder.build(model, promptLevelInfo)
                result.push(controller);
            },
            this
        );

        return result;
    }
});