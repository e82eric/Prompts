function PromptController (model) {
    this.model = model;

    this.createView = function () {
        if (this.model.PromptType == "ShoppingCart") {
            var shoppingCartBuilder = new ShoppingCartBuilder();
            var shoppingCart = shoppingCartBuilder.build(model);
            return shoppingCart.createView();
        }

        if (this.model.PromptType == "SingleSelectTree") {
            var builder = new TreeDropDownBuilder();
            var treeDropDown = builder.build(model);
            return treeDropDown.createView();
        }

        if (this.model.PromptType == "RecursiveSingleSelectTree") {
            var builder = new RecursiveTreeDropDownBuilder();
            var treeDropDown = builder.build(model);
            return treeDropDown.createView();
        }

        if (this.model.PromptType == "RecursiveTree") {
            var builder = new RecursiveTreeShoppingCartBuilder();
            var treeDropDown = builder.build(model);
            return treeDropDown.createView();
        }

        if (this.model.PromptType == "Tree") {
            var builder = new TreeShoppingCartBuilder();
            var treeShoppingCart = builder.build(model);
            return treeShoppingCart.createView();
        }

        var dropDownBuilder = new DropDownBuilder();
        var dropDown = dropDownBuilder.build(model);
        return dropDown.createView();
    }
}