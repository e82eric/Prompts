module("Asynchronous Items Controller", {
	setup: function () {
		this.view = { renderItems: sinon.spy() };
		this.controller = new AsynchronousItemsController();
	}
});

test("It sets the items to an empty array when initalized", function () {
	ok(this.controller.items.length === 0);
});

test("It sets the items correctly when a view has not been set", function () {
	var items = [1, 2, 3];
	this.controller.setItems(items);
	ok(this.controller.items === items);
});

test("It tells the view to render the items when they are set", function () {
	var items = [1, 2, 3];
	this.controller.setView(this.view);	
	this.controller.setItems(items);
	ok(this.view.renderItems.callCount === 1);
	ok(this.view.renderItems.withArgs(items).callCount === 1);
});

test("It tell the view to render the items when it is set and the items have been set first", function () {
	var items = [1, 2, 3];
	this.controller.setItems(items);
	ok(this.view.renderItems.callCount === 0);
	this.controller.setView(this.view);
	ok(this.view.renderItems.callCount === 1);
	ok(this.view.renderItems.withArgs(items).callCount === 1);
});

test("It does not tell the view render the items when it is set and no items have been set", function () {
	ok(this.view.renderItems.callCount === 0);
	this.controller.setView(this.view);
	ok(this.view.renderItems.callCount === 0);
});
