module("Prompts Controller", {
	setup: function () {
		this.view = { 
			displayPrompt: sinon.spy(),
			disableMovePrevious: sinon.spy(),
			enableMovePrevious: sinon.spy(),
			disableMoveNext: sinon.spy(),
			enableMoveNext: sinon.spy()
		};

		this.controller = new PromptsController();
		this.controller.setView(this.view);
	}
});

test("It asks the view to display the first item when the items are set", function () {
	var item1 = { name: "item 1" };
	var item2 = { name: "item 2" };
	var item3 = { name: "item 3" };

	ok(this.view.displayPrompt.callCount === 0);

	this.controller.setItems([item1, item2, item3]);

	ok(this.view.displayPrompt.callCount === 1);
	ok(this.view.displayPrompt.withArgs(item1).callCount === 1);
});

test("It asks the view to display the second item when move next is called", function () {
	var item1 = { name: "item 1" };
	var item2 = { name: "item 2" };
	var item3 = { name: "item 3" };

	ok(this.view.displayPrompt.callCount === 0);

	this.controller.setItems([item1, item2, item3]);

	ok(this.view.displayPrompt.callCount === 1);

	this.controller.moveNext();

	ok(this.view.displayPrompt.callCount === 2);
	ok(this.view.displayPrompt.withArgs(item2).callCount === 1);
});

test("It asks the view to display the first item when move previous is called", function () {
	var item1 = { name: "item 1" };
	var item2 = { name: "item 2" };
	var item3 = { name: "item 3" };

	ok(this.view.displayPrompt.callCount === 0);

	this.controller.setItems([item1, item2, item3]);

	ok(this.view.displayPrompt.callCount === 1);

	this.controller.moveNext();

	ok(this.view.displayPrompt.callCount === 2);

	this.controller.movePrevious();

	ok(this.view.displayPrompt.callCount === 3);
	ok(this.view.displayPrompt.withArgs(item1).callCount === 2);
});

test("It tells the view to disable move previous when the items are set", function () {
	ok(this.view.disableMovePrevious.callCount === 0);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.setItems([1,2,3]);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 0);
});

test("It tells the view to disable move next the items are set and there is only one", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([1]);

	ok(this.view.disableMoveNext.callCount === 1);
	ok(this.view.enableMoveNext.callCount === 0);
});


test("It tells the view to enable move next when items are set and there is more than one item", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([1, 2]);

	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 1);
});

test("It tells the view to enable move previous when move next is called", function () {
	ok(this.view.disableMovePrevious.callCount === 0);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.setItems([1, 2]);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.moveNext();

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 1);
});

test("It tells the view to disable move next when it is moved to the last item", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([1,2,3]);

	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 1);

	this.controller.moveNext();

	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 1);

	this.controller.moveNext();

	ok(this.view.disableMoveNext.callCount === 1);
	ok(this.view.enableMoveNext.callCount === 1);
});

test("It tells the view to disable move previous when moved to the first item", function () {
	ok(this.view.disableMovePrevious.callCount === 0);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.setItems([1,2,3]);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.moveNext();

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 1);

	this.controller.moveNext();

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 1);

	this.controller.movePrevious();

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 1);

	this.controller.movePrevious();

	ok(this.view.disableMovePrevious.callCount === 2);
	ok(this.view.enableMovePrevious.callCount === 1);
});

test("It tells the view to enable move next when it is moved back from the last item", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([1, 2, 3]);

	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 1);

	this.controller.moveNext();

	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 1);

	this.controller.moveNext();

	ok(this.view.disableMoveNext.callCount === 1);
	ok(this.view.enableMoveNext.callCount === 1);

	this.controller.movePrevious();

	ok(this.view.disableMoveNext.callCount === 1);
	ok(this.view.enableMoveNext.callCount === 2);
});
