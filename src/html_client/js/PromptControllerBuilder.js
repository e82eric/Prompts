var PromptControllerBuilder = Class.extend({
    init: function (promptsController) {
        this.promptsController = promptsController;

        this.shoppingCartBuilder = new ShoppingCartBuilder();
        this.treeDropDownBuilder = new TreeDropDownBuilder();
        this.recursiveTreeDropDownBuilder = new RecursiveTreeDropDownBuilder();
        this.recursiveTreeShoppingCartBuilder = new RecursiveTreeShoppingCartBuilder();
        this.treeShoppingCartBuilder = new TreeShoppingCartBuilder();
        this.asynchronousSearchShoppingCartBuilder = new AsynchronousSearchShoppingCartBuilder();
        this.dropDownBuilder = new DropDownBuilder();
    },

    build: function (model) {
        if (model.PromptType == "ShoppingCart") {
            return this.shoppingCartBuilder.build(model, this.promptsController);
        }

        if (model.PromptType == "SingleSelectTree") {
            return this.treeDropDownBuilder.build(model, this.promptsController);
        }

        if (model.PromptType == "RecursiveSingleSelectTree") {
            return this.recursiveTreeDropDownBuilder.build(model, this.promptsController);
        }

        if (model.PromptType == "RecursiveTree") {
            return this.recursiveTreeShoppingCartBuilder.build(model, this.promptsController);
        }

        if (model.PromptType == "Tree") {
            return this.treeShoppingCartBuilder.build(model, this.promptsController);
        }

        if (model.PromptType == "Empty") {
            var controller = new EmptyPromptController(model, this.promptsController, function () {
                return new EmptyPromptView(controller);
            });

            return controller;
        }

        if (model.PromptType == "CasscadingSearch") {
            return this.asynchronousSearchShoppingCartBuilder.build(model, this.promptsController);
        }

        return this.dropDownBuilder.build(model, this.promptsController);
    }
});