function PromptController (model) {
    this.model = model;

    this.createView = function () {
        if (this.model.PromptType == "ShoppingCart") {
            var shoppingCartBuilder = new ShoppingCartBuilder();
            var shoppingCart = shoppingCartBuilder.build(model);
            return shoppingCart.createView();
        }

        var dropDownBuilder = new DropDownBuilder();
        var dropDown = dropDownBuilder.build(model);
        return dropDown.createView();
    }
}