var RootTreePromptItemController = Class.extend ({
    init: function (model, availableItemsController, childPromptItemsLoadingPanel, childPromptItemsRequest ) {
        this.model = model;
        this.availableItemsController = availableItemsController;
        this.isSelected = false;
        this.childPromptItemsRequest = childPromptItemsRequest;
        this.childPromptItemsLoadingPanel = childPromptItemsLoadingPanel;
        this.toggleState = 'open';

        this.setChildren([]);
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
        var view = new TreePromptItemView(this);
        this.setView(view);
        return this.view;
    },

    setView: function (val) {
        this.view = val;
        this.changeToggle();
    },

    deleteItem: function () {
        this.view.deleteItem();
    },

    expanderClicked: function () {
        this.changeToggle();

    },

    changeToggle: function () {
        if (this.toggleState === 'closed') {
            this.toggleState = 'open';
        } else if (this.toggleState === 'open') {
            this.toggleState = 'closed';
        }

        if (this.toggleState === 'closed') {
            this.view.renderCollapse();
        } else if (this.toggleState === 'open') {
            this.view.renderExpand();
            if(this.Children.length == 0) {
                this.childPromptItemsLoadingPanel.load(this.childPromptItemsRequest);
            }
        }
    },

    setChildren: function (val) {
        this.Children = val;
    }
});