module("Asynchronous Items Controller", {
	setup: function () {
		this.view = { renderItems: sinon.spy() };
		this.controller = new AsynchronousItemsController();
		this.controller.setView(this.view);
	}
});

test("It sets the items to an empty array when initalized", function () {
	ok(this.controller.items.length === 0);
});

test("It sets the items correctly", function () {
	var items = [1, 2, 3];
	this.controller.setItems(items);
	ok(this.controller.items === items);
});

test("It tells the view to render the items when they are set", function () {
	var items = [1, 2, 3];
	this.controller.setItems(items);
	ok(this.view.renderItems.callCount === 1);
	ok(this.view.renderItems.withArgs(items).callCount === 1);
});