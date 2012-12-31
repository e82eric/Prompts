var PromptControllerBuilder = Class.extend({
    build: function (model) {
        if (this.model.PromptType == "ShoppingCart") {
            var shoppingCartBuilder = new ShoppingCartBuilder();
            return shoppingCartBuilder.build(model);
        }

        if (this.model.PromptType == "SingleSelectTree") {
            var builder = new TreeDropDownBuilder();
            return builder.build(model);
        }

        if (this.model.PromptType == "RecursiveSingleSelectTree") {
            var builder = new RecursiveTreeDropDownBuilder();
            return builder.build(model);
        }

        if (this.model.PromptType == "RecursiveTree") {
            var builder = new RecursiveTreeShoppingCartBuilder();
            return builder.build(model);
        }

        if (this.model.PromptType == "Tree") {
            var builder = new TreeShoppingCartBuilder();
            return builder.build(model);
        }

        if (this.model.PromptType == "Empty") {
            return new EmptyPromptController(model);
        }

        if (this.model.PromptType == "CasscadingSearch") {
            var builder = new AsynchronousSearchShoppingCartBuilder();
            return builder.build(model);
        }

        var dropDownBuilder = new DropDownBuilder();
        return dropDownBuilder.build(model);
    }
});