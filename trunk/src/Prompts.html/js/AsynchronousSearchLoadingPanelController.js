var AsynchronousSearchLoadingPanelController = LoadingPanelControllerBase.extend({
    createView: function (availableItemsView, selectedItemsView) {
        this.setView(new AsynchronousSearchLoadingPanelView(this, availableItemsView, selectedItemsView));
        return this.view;
    }
});