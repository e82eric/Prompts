module("Available Items Controller", {
	setup: function () {
		this.view = { renderItems: sinon.spy() };
		this.search = { execute: sinon.spy() };
		this.selector = { select: sinon.stub() };
		this.itemsDisposer = { dispose: sinon.spy() };
		this.controller = new AvailableItemsController(this.selector, this.search, this.itemsDisposer);
	}
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
	this.controller.setView(this.view);

	var expected = [1, 2, 3];

	this.selector.select.withArgs(true, false, items, item2).returns(expected);

	this.controller.select(true, false, item2);

	ok(this.controller.getSelectedItems() === expected);
});

test("It selects with the display items", function () {
	var item1 = { deleteItem: sinon.spy() };
	var item2 = { deleteItem: sinon.spy() };
	var item3 = { deleteItem: sinon.spy() };

	var items = [item1, item2, item3];

	var displayItems = [4, 5, 6];

	this.controller.setItems(items);
	this.controller.setView(this.view);

	this.controller.setDisplayItems(displayItems);

	this.controller.select(true, false, item2);

	ok(this.selector.select.callCount === 1);
	ok(this.selector.select.withArgs(true, false, displayItems, item2).callCount === 1);
});

test("It executes the search with the search string", function () {
	var searchString = "search string 1";

	this.controller.setSearchString(searchString);

	ok(this.search.execute.callCount === 0);

	this.controller.search();

	ok(this.search.execute.callCount === 1);
	ok(this.search.execute.withArgs(searchString, this.controller).callCount === 1);
});

test("It sets the display items when the items are set", function () {
	var items = [1, 2, 3];
	var displayItems = [4, 5, 6];

	this.controller.setItems(items);
	this.controller.setView(this.view);

	ok(this.view.renderItems.callCount === 1);
	
	this.controller.setDisplayItems(displayItems);

	ok(this.view.renderItems.callCount ===2);
	ok(this.view.renderItems.withArgs(displayItems).callCount === 1);
});

test("It asks the view to render the items when the display items are set", function () {
	var items = [1, 2, 3];
	var displayItems = [4, 5, 6];

	ok(this.controller.displayItems === undefined);
	
	this.controller.setItems(items);
	this.controller.setView(this.view);

	ok(this.view.renderItems.callCount === 1);
	ok(this.view.renderItems.withArgs(displayItems).callCount === 0);

	this.controller.setDisplayItems(displayItems);

	ok(this.view.renderItems.callCount === 2);
	ok(this.view.renderItems.withArgs(displayItems).callCount === 1);
});

test("It disposes the old display items", function () {
	var firstItems = [1, 2, 3];
	
	this.controller.setItems(firstItems);
	this.controller.setView(this.view);

	ok(this.itemsDisposer.dispose.callCount === 0);

	this.controller.setDisplayItems([4, 5, 6]);

	ok(this.itemsDisposer.dispose.callCount === 1);
	ok(this.itemsDisposer.dispose.withArgs(firstItems).callCount === 1);
});

test("It calls render items on the view when the view is set", function () {
	var view = { renderItems: sinon.spy() };
	var search = { execute: sinon.spy() };
	var selector = { select: sinon.stub() };
	var itemsDisposer = { dispose: sinon.spy() };
	var controller = new AvailableItemsController(this.selector, this.search, this.itemsDisposer);

	var items = [1, 2,3];

	this.controller.setItems(items);

	ok(view.renderItems.callCount === 0);

	this.controller.setView(view);

	ok(view.renderItems.callCount === 1);
	ok(view.renderItems.withArgs(items).callCount === 1);
});

