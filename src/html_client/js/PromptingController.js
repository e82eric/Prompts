var PromptingController = Class.extend({
    init: function (requester, itemsController, loadingPanelController, executeCommand) {
        this.requester = requester;
        this.itemsController = itemsController;
        this.loadingPanelController = loadingPanelController;
        this._executeCommand = executeCommand;
    },

    show: function (path) {
        this.path = path;
        this.requester.execute({Path: this.path}, this.itemsController);
    },

    evaluateReadyForExecution: function () {
        var flag = _.where(this.itemsController.items, {readyForExecution: false}).length == 0;

        if(flag) {
            this._executeCommand.setCanExecute(true);
        } else {
            this._executeCommand.setCanExecute(false);
        }
    },

    getExecuteRequest: function () {
        var selections = _.map(this.itemsController.items, function (item) { return item.selectionInfo(); });
        return { Path: this.path, PromptSelections: selections };
    },

    createView: function () {
        var itemsView = this.itemsController.createView();
        var loadingPanelView = this.loadingPanelController.createView(itemsView, "");
        var executeReportView = this._executeCommand.createView();
        this.view = new PromptingView(this, loadingPanelView, executeReportView);
        return this.view;
    }
});