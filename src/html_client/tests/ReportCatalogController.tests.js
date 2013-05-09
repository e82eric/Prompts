module("Report Catalog Controller", {
	setup: function () {
		this.requester = { };
		this.itemsController = { name: "items controller 1" };
		this.controller = new ReportCatalogController(this.requester, this.itemsController);
	}
});

test("It executes the report catalog requester after the view is set", function () {
	var self = this;
	var view = { name: "view 1" };
	var numberOfExecuteCalls = 0;

	this.requester.execute = function (itemsController) {
		numberOfExecuteCalls++;
		ok(self.controller.view === view);
		ok(self.itemsController === itemsController);
	};

	ok(numberOfExecuteCalls === 0);

	this.controller.setView(view);

	ok(numberOfExecuteCalls === 1);
});