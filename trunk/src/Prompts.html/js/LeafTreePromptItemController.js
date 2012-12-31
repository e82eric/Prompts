var LeafTreePromptItemController = SelectableItemController.extend ({
    init: function (model, availableItemsController ) {
        this.model = model;
        this.availableItemsController = availableItemsController;
        this.isSelected = false;
        this.Children = [];
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
    }
});