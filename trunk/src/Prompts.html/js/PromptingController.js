var PromptingController = Class.extend({
    init: function (requester, itemsController, loadingPanelController) {
        this.requester = requester;
        this.itemsController = itemsController;
        this.loadingPanelController = loadingPanelController;
    },

    show: function(path) {
        this.requester.execute({Path: path}, this.itemsController);
    },

    createView: function () {
        var itemsView = this.itemsController.createView();
        var loadingPanelView = this.loadingPanelController.createView(itemsView);
        this.view = new PromptingView(this, loadingPanelView);
        return this.view;
    }
});