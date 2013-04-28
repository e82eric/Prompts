var PromptingView = Class.extend({
    init: function (controller, loadingPanelView, executeReportView) {
    	this.controller = controller;
    	this.loadingPanelView = loadingPanelView;
        this.root = $("#prompts");
        loadingPanelView.root.find("#execute-report").append(executeReportView.render());
        this.root.append(loadingPanelView.render());

    },

    onRetryClick: function () {
        this.controller.onRetryClick();
    },

    render: function () {
    	this.loadingPanelView.root.filter("#retry").click($.proxy(this.onRetryClick, this));
        return this.root;
    }
});