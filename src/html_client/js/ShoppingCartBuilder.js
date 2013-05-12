var ShoppingCartBuilder = Class.extend({
    build: function (model, promptsController) {
        return this._build(
            model,
            promptsController,
            new PromptItemControllerBuilder(),
            function (multiSelector) { return multiSelector; },
            function (controller) { return new SearchableShoppingCartView(controller); });
    },

    _build: function (model, promptsController, availableItemBuilder, createSelectorFunc, createViewFunc, buildItemParams) {
        var singleSelector = new SingleSelector();
        var rangeSelector = new RangeSelector();
        var inverseSelector = new InverseSelector();
        var multiSelector = new MultiSelector(
            singleSelector,
            rangeSelector,
            inverseSelector
        );

        var selector = createSelectorFunc(multiSelector);

        var selectedItemBuilder = new PromptItemControllerBuilder();
        var selectedItemsBuilder = new ItemsBuilder(selectedItemBuilder);
        var selectedItemsController = new SelectedItemsController(multiSelector, selectedItemsBuilder);
        selectedItemBuilder.setAvailableItemsController(selectedItemsController);

        var availableItemsBuilder = new ItemsBuilder(availableItemBuilder);
        var availableItemsController = new AvailableItemsController(
            selector,
            new ClientSideSearch(new SearchStringParser()),
            new ItemsDisposer()
        );

        availableItemBuilder.setAvailableItemsController(availableItemsController);

        availableItemBuilder.setAvailableItemsController(availableItemsController);

        var availableItems = availableItemsBuilder.build(model.PromptLevelInfo.AvailableItems, buildItemParams);

        availableItemsController.setItems(availableItems);

        if(model.DefaultValues.length > 0) {
            var defaultAvailableItems = [];

            _.each(
                model.DefaultValues,
                function (defaultValue) {
                    var matchingModel = _.find(
                        model.PromptLevelInfo.AvailableItems,
                        function (availableItem) {
                            return availableItem.Value === defaultValue.Value;
                        },
                        this);

                    if(matchingModel != undefined) {
                        defaultAvailableItems.push(matchingModel);
                    }
                },
                this);

            selectedItemsController.setDefaults(defaultAvailableItems);
        }

        var shoppingCartController = new MultiSelectPromptController(
            model, 
            availableItemsController, 
            selectedItemsController, 
            promptsController,
            createViewFunc
        );

        return shoppingCartController;
    }
});