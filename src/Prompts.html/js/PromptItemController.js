var PromptItemController = SelectableItemController.extend({
    init: function (model, availableItemsController) {
        this.model = model;
        this.availableItemsController = availableItemsController;
    },

    clicked: function (shiftKeyPressed, controlKeyPressed) {
        this.availableItemsController.select(shiftKeyPressed, controlKeyPressed, this);
    },

    createView: function () {
        var view = new PromptItemView(this);
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