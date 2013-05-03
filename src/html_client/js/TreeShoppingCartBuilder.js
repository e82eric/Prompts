var TreeShoppingCartBuilder = ShoppingCartBuilder.extend({
    build: function (model, promptsController) {
        var hierarhcyFlattener = new HierarchyFlattener();

        var itemBuilder = new TreePromptItemControllerBuilder(model.Name, undefined, []);

        return this._build(
            model,
            promptsController,
            itemBuilder,
            function (multiSelector) { return new TreeShoppingCartSelector(multiSelector, hierarhcyFlattener); },
            function (controller) { return new ShoppingCartView(controller, "treeShoppingCartTemplate"); },
            { promptLevelInfo: model.PromptLevelInfo });
    }
});