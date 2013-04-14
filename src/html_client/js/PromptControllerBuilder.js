var PromptControllerBuilder = Class.extend({
    init: function () {
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
            return this.shoppingCartBuilder.build(model);
        }

        if (model.PromptType == "SingleSelectTree") {
            return this.treeDropDownBuilder.build(model);
        }

        if (model.PromptType == "RecursiveSingleSelectTree") {
            return this.recursiveTreeDropDownBuilder.build(model);
        }

        if (model.PromptType == "RecursiveTree") {
            return this.recursiveTreeShoppingCartBuilder.build(model);
        }

        if (model.PromptType == "Tree") {
            return this.treeShoppingCartBuilder.build(model);
        }

        if (model.PromptType == "Empty") {
            return new EmptyPromptController(model);
        }

        if (model.PromptType == "CasscadingSearch") {
            return this.asynchronousSearchShoppingCartBuilder.build(model);
        }

        return this.dropDownBuilder.build(model);
    }
});