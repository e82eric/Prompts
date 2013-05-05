module("Asynchronous Search Available Items Controller", {
	setup: function () {
		this.view = { renderItems: sinon.spy() };
		this.selector = { select: sinon.stub() };
		this.controller = new AsynchronousSearchAvailableItemsController(this.selector);
		this.controller.setView(this.view);
	}
});

test("It calls delete on all of the old items when new items are set", function () {
	var oldItem1 = { deleteItem: sinon.spy() };
	var oldItem2 = { deleteItem: sinon.spy() };
	var oldItem3 = { deleteItem: sinon.spy() };

	var oldItems = [oldItem1, oldItem2, oldItem3];

	this.controller.setItems(oldItems);

	ok(oldItem1.deleteItem.callCount === 0);
	ok(oldItem2.deleteItem.callCount === 0);
	ok(oldItem3.deleteItem.callCount === 0);

	this.controller.setItems([1,2,3]);

	ok(oldItem1.deleteItem.callCount === 1);
	ok(oldItem2.deleteItem.callCount === 1);
	ok(oldItem3.deleteItem.callCount === 1);
});

test("It initalizes the selected items as an empty array", function () {
	ok(this.controller.getSelectedItems().length === 0);
});

test("It delegates selection to the selector", function () {
	var item1 = { deleteItem: sinon.spy() };
	var item2 = { deleteItem: sinon.spy() };
	var item3 = { deleteItem: sinon.spy() };

	var items = [item1, item2, item3];

	this.controller.setItems(items);

	var expected = [1, 2, 3];

	this.selector.select.withArgs(true, false, items, item2).returns(expected);

	this.controller.select(true, false, item2);

	ok(this.controller.getSelectedItems() === expected);
});

