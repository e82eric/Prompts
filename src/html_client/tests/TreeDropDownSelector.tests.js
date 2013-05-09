module("Tree Drop Down Selector", {
	setup: function () {
		this.singleSelctor = { select: sinon.spy() };
		this.hiearchyFlattner = { Flatten: sinon.stub() };
		this.promptController = { onSelection: sinon.spy() };
		this.selector = new TreeDropDownSelector(this.singleSelctor, this.hiearchyFlattner);
		this.selector.setPromptController(this.promptController);
	}
});

test("It flattens the tree items with the hiearchy flattner and selects the item with the single selector", function () {
	var itemToSelect = 5;
	var treeItems = [1,2,3];
	var flatItems = [1,4,2,itemToSelect,3,6];

	this.hiearchyFlattner.Flatten.withArgs(treeItems).returns(flatItems);

	ok(this.singleSelctor.select.callCount === 0);

	this.selector.select(false, false, treeItems, itemToSelect);

	ok(this.singleSelctor.select.callCount === 1);
	ok(this.singleSelctor.select.withArgs(flatItems, itemToSelect).callCount === 1);
});

test("It reports the selection on the prompt controller after it uses the single selector to select", function () {
	var self = this;
	var numberOfOnSelectionCalls = 0;

	var itemToSelect = 5;
	var treeItems = [1,2,3];
	var flatItems = [1,4,2,itemToSelect,3,6];

	this.hiearchyFlattner.Flatten.withArgs(treeItems).returns(flatItems);

	this.promptController.onSelection = function (item) {
		ok(self.singleSelctor.select.callCount === 1);
		ok(self.singleSelctor.select.withArgs(flatItems, itemToSelect).callCount === 1);
		ok(item === itemToSelect);
		numberOfOnSelectionCalls++;
	};

	ok(numberOfOnSelectionCalls === 0);

	this.selector.select(false, false, treeItems, itemToSelect);

	ok(numberOfOnSelectionCalls === 1);
});