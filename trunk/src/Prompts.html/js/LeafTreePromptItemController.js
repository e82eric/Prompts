var LeafTreePromptItemController = Class.extend ({
    init: function (model, availableItemsController, childPromptItemsLoadingPanel, childPromptItemsRequest ) {
        this.model = model;
        this.availableItemsController = availableItemsController;
        this.isSelected = false;
        this.childPromptItemsRequest = childPromptItemsRequest;
        this.childPromptItemsLoadingPanel = childPromptItemsLoadingPanel;
        this.Children = [];
    },

    Select: function () {
        this.view.onSelected();
        this.isSelected = true;
    },

    UnSelect: function () {
        this.view.onUnSelected();
        this.isSelected = false;
    },

    clicked: function (shiftKeyPressed, controlKeyPressed) {
        this.availableItemsController.select(shiftKeyPressed, controlKeyPressed, this);
    },

    createView: function () {
        var view = new LeafTreePromptItemView(this);
        this.setView(view);
        return this.view;
    },

    setView: function (val) {
        this.view = val;
    },

    deleteItem: function () {
        this.view.deleteItem();
    }
});