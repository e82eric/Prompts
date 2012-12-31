var TreePromptItemController = ExpandableItemController.extend({
    init: function (model, availableItemsController, childItemsRequester, childAvailableItemsController, childItemsLoadingPanelController) {
        this.model = model;
        this.availableItemsController = availableItemsController;
        this.childItemsRequester = childItemsRequester;
        this.childAvailableItemsController = childAvailableItemsController;
        this.childItemsLoadingPanelController = childItemsLoadingPanelController;
        this._super();
    },

    clicked: function (shiftKeyPressed, controlKeyPressed) {
        this.availableItemsController.select(shiftKeyPressed, controlKeyPressed, this);
    },

    createView: function () {
        var childAvailableItemsView = this.childAvailableItemsController.createView();
        var childItemsLoadingPanelController = this.childItemsLoadingPanelController.createView(childAvailableItemsView);

        var view = new TreePromptItemView(this, childItemsLoadingPanelController);
        this.setView(view);
        return this.view;
    },

    deleteItem: function () {
        this.view.deleteItem();
    },

    expand: function() {
        this._super();
        if(this.childAvailableItemsController.items == undefined) {
            this.childItemsRequester.execute(this.childAvailableItemsController, this);
        }
    },

    setChildren: function(val) {
        if(val.length == 0) {
            this.view.hideExpander();
        }
        this._super(val);
    }
});