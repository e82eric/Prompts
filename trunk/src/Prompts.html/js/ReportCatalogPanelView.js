var ReportCatalogPanelView = Backbone.View.extend({
	tagName: "div",
	model: ReportCatalogPanel,

	initialize: function () {
		this.render();
	},

	render: function () {
        var panelElement = $("#reportCatalog");

        panelElement.text("loading...");
		this.model.run(function (catalogView){
            panelElement.text("done...");
            panelElement.html(catalogView.el);
		});
	}
});

