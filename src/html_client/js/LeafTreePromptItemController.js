var LeafTreePromptItemController = PromptItemController.extend ({
    init: function (model, availableItemsController ) {
        this.model = model;
        this.availableItemsController = availableItemsController;
        this.isSelected = false;
        this.Children = [];
    },

    createView: function () {
        var view = new LeafTreePromptItemView(this);
        this.setView(view);
        return this.view;
    }
});