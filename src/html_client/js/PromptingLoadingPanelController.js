var PromptingLoadingPanelController = LoadingPanelControllerBase.extend({
    createView: function (itemsControllerView) {
        this.setView(new PromptingLoadingPanelView(this, itemsControllerView));
        return this.view;
    }
});