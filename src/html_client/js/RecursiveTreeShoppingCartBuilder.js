var RecursiveTreeShoppingCartBuilder = ShoppingCartBuilder.extend({
    build: function (model, promptsController) {
        var hierarhcyFlattener = new HierarchyFlattener();

        var filterParameterName = model.PromptLevelInfo.ParameterName;

        var itemBuilder = new RecursiveTreePromptItemControllerBuilder(
            model.Name,
            undefined,
            filterParameterName);

        return this._build(
            model,
            promptsController,
            itemBuilder,
            function (multiSelector) { return new TreeShoppingCartSelector(multiSelector, hierarhcyFlattener); },
            function (controller) { return new ShoppingCartView(controller, "treeShoppingCartTemplate"); });
    }
});