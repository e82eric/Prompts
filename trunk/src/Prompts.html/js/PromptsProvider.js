function PromptsProvider () {
    this.get = function (models) {
        var result = [];

        _.each(
            models,
            function (model) {
                var controller = new PromptController(model);
                result.push(controller)
            },
            this);

        return result;
    }
}