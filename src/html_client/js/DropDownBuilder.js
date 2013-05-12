var DropDownBuilder = Class.extend({
    init: function (promptsController) {
        this._promptsController = promptsController;
    },

    build: function (model) {
        var singleSelector = new SingleSelector();
        var selector = new DropDownSelector(singleSelector);
        var itemBuilder = new PromptItemControllerBuilder();

        return this._build(model, selector, itemBuilder, function (controller) {
            return new SearchableDropDownView(controller);
        });
    },

    _build: function (model, selector, itemBuilder, createViewFunc, buildItemParams) {
        var availableItemsController = new AvailableItemsController(
            selector,
            new ClientSideSearch(new SearchStringParser()),
            new ItemsDisposer()
        );
        
        itemBuilder.setAvailableItemsController(availableItemsController);
        var itemsBuilder = new ItemsBuilder(itemBuilder);
        var items = itemsBuilder.build(model.PromptLevelInfo.AvailableItems, buildItemParams);
        availableItemsController.setItems(items);

        var defaultController = this._getSelectedItem(model, items);

        var dropDownController = new SingleSelectPromptController(
            model, 
            availableItemsController, 
            defaultController,
            this._promptsController,
            createViewFunc
        );
        
        selector.setPromptController(dropDownController);
        return dropDownController;
    },

    _getSelectedItem: function (model, controllers) {
        var defaultController = undefined;

        if(model.DefaultValues.length > 0) {
            var defaultModel = model.DefaultValues[0];
            _.each(
                controllers,
                function (itemController){
                    if(itemController.model.Value == defaultModel.Value){
                        defaultController = itemController;
                    }
                },
                this);
        }

        return defaultController;
    }
});