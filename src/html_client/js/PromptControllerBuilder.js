var PromptControllerBuilder = Class.extend({
    init: function () {
        this.shoppingCartBuilder = new ShoppingCartBuilder();
        this.recursiveTreeShoppingCartBuilder = new RecursiveTreeShoppingCartBuilder();
        this.treeShoppingCartBuilder = new TreeShoppingCartBuilder();
        this.asynchronousSearchShoppingCartBuilder = new AsynchronousSearchShoppingCartBuilder();
    },

    build: function (buildParams) {
        var model = buildParams.model;

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
    },

    setPromptingController: function(val) {
        this.promptsController = val;
        this.dropDownBuilder = new DropDownBuilder(this.promptsController);
        this.treeDropDownBuilder = new TreeDropDownBuilder(this.promptsController);
        this.recursiveTreeDropDownBuilder = new RecursiveTreeDropDownBuilder(this.promptsController);
    }
});