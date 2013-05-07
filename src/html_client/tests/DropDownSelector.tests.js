module("Drop Down Selector", {
	setup: function () {
		this.singleSelector = { select: sinon.spy() };
		this.promptController = { onSelection: sinon.spy() };

		this.selector = new DropDownSelector(this.singleSelector);
		this.selector.setPromptController(this.promptController);
	}
});

test("It calls select on the single selector with the items and the item", function () {
	var items = [1, 2, 3];
	var item = { name: "item1" };

	ok(this.singleSelector.select.callCount === 0);

	this.selector.select(false, false, items, item);

	ok(this.singleSelector.select.callCount === 1);
	ok(this.singleSelector.select.withArgs(items, item).callCount === 1);
});

test("It reports the selection to the prompt controller", function () {
	var items = [1, 2, 3];
	var item = { name: "item1" };

	ok(this.promptController.onSelection.callCount === 0);

	this.selector.select(false, false, items, item);

	ok(this.promptController.onSelection.callCount === 1);
	ok(this.promptController.onSelection.withArgs(item).callCount === 1);
});