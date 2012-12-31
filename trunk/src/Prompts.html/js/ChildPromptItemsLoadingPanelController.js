var ChildPromptItemsLoadingPanelController = LoadingPanelControllerBase.extend({
    createView: function (childAvailableItemsView) {
        this.setView(new ChildPromptItemsLoadingPanelView(this, childAvailableItemsView));
        return this.view;
    }
});