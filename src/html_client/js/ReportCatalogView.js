var ReportCatalogView = Class.extend({
	init: function(controller, loadingPanelView) {
		this.controller = controller;
		this.loadingPanelView = loadingPanelView;
		this.root = $("#report-catalog");
		this.root.append(loadingPanelView.render());
	},

	render: function () {
		this.loadingPanelView.root.filter("#retry").click($.proxy(this.onRetry, this));
		return this.root;
	},

	onRetry: function () {
		this.controller.onRetryClick();
	}
});