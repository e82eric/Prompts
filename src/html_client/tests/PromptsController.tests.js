module("Prompts Controller", {
	setup: function () {
		this.view = { 
			displayPrompt: sinon.spy(),
			disableMovePrevious: sinon.spy(),
			enableMovePrevious: sinon.spy(),
			disableMoveNext: sinon.spy(),
			enableMoveNext: sinon.spy(),
			setSmallItems: sinon.spy()
		};


		this.item1 = { name: "item 1", showSmall: sinon.spy(), hideSmall: sinon.spy() };
		this.item2 = { name: "item 2", showSmall: sinon.spy(), hideSmall: sinon.spy() };
		this.item3 = { name: "item 3", showSmall: sinon.spy(), hideSmall: sinon.spy() };
		this.item4 = { name: "item 4", showSmall: sinon.spy(), hideSmall: sinon.spy() };

		this.controller = new PromptsController();
		this.controller.setView(this.view);
	}
});

test("It asks the view to display the first item when the items are set", function () {
	ok(this.view.displayPrompt.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.view.displayPrompt.callCount === 1);
	ok(this.view.displayPrompt.withArgs(this.item1).callCount === 1);
});

test("It asks the view to display the second item when move next is called", function () {
	ok(this.view.displayPrompt.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.view.displayPrompt.callCount === 1);

	this.controller.moveNext();

	ok(this.view.displayPrompt.callCount === 2);
	ok(this.view.displayPrompt.withArgs(this.item2).callCount === 1);
});

test("It asks the view to display the first item when move previous is called", function () {
	ok(this.view.displayPrompt.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.view.displayPrompt.callCount === 1);

	this.controller.moveNext();

	ok(this.view.displayPrompt.callCount === 2);

	this.controller.movePrevious();

	ok(this.view.displayPrompt.callCount === 3);
	ok(this.view.displayPrompt.withArgs(this.item1).callCount === 2);
});

test("It tells the view to disable move previous when the items are set", function () {
	ok(this.view.disableMovePrevious.callCount === 0);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 0);
});

test("It tells the view to disable move next the items are set and there is only one", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([this.item1]);

	ok(this.view.disableMoveNext.callCount === 1);
	ok(this.view.enableMoveNext.callCount === 0);
});


test("It tells the view to enable move next when items are set and there is more than one item", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([this.item1, this.item2]);

	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 1);
});

test("It tells the view to enable move previous when move next is called", function () {
	ok(this.view.disableMovePrevious.callCount === 0);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.setItems([this.item1, this.item2]);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.moveNext();

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 1);
});

test("It tells the view to disable move next when it is moved to the last item", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

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

	this.controller.setItems([this.item1, this.item2, this.item3]);

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

	this.controller.setItems([this.item1, this.item2, this.item3]);

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

test("It tells the view to display the small items when the items are set", function () {
	var items = [this.item1, this.item2, this.item3];

	ok(this.view.setSmallItems.callCount === 0);

	this.controller.setItems(items);

	ok(this.view.setSmallItems.callCount === 1);
	ok(this.view.setSmallItems.withArgs(items).callCount === 1);
});

test("It tells the view to display the prompt when move to prompt is called", function () {
	ok(this.view.displayPrompt.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3, this.item4]);

	ok(this.view.displayPrompt.callCount === 1);

	this.controller.moveToPrompt(this.item3);
	
	ok(this.view.displayPrompt.callCount === 2);
	ok(this.view.displayPrompt.withArgs(this.item3).callCount === 1);
});

test("It tells the view to disable move next when it is told to move to the last prompt", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 1);

	this.controller.moveToPrompt(this.item3);

	ok(this.view.disableMoveNext.callCount === 1);
	ok(this.view.enableMoveNext.callCount === 1);
});


test("It tells the view to enable move next when told to move to the second to last prompt after being told to too move to the last prompt", function () {
	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.view.disableMoveNext.callCount === 0);
	ok(this.view.enableMoveNext.callCount === 1);

	this.controller.moveToPrompt(this.item3);

	ok(this.view.disableMoveNext.callCount === 1);
	ok(this.view.enableMoveNext.callCount === 1);

	this.controller.moveToPrompt(this.item2);

	ok(this.view.disableMoveNext.callCount === 1);
	ok(this.view.enableMoveNext.callCount === 2);
});

test("It tells the view to enable move previous when it is told to move to a prompt that is not the first", function () {
	ok(this.view.disableMovePrevious.callCount === 0);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.moveToPrompt(this.item2);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 1);
});

test("It tells the view to disable move previous when it is told to move to the first prompt after being told to move to a prompt that is not the first", function () {
	ok(this.view.disableMovePrevious.callCount === 0);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 0);

	this.controller.moveToPrompt(this.item2);

	ok(this.view.disableMovePrevious.callCount === 1);
	ok(this.view.enableMovePrevious.callCount === 1);

	this.controller.moveToPrompt(this.item1);

	ok(this.view.disableMovePrevious.callCount === 2);
	ok(this.view.enableMovePrevious.callCount === 1);
});

test("It tells the first prompt hide its small view when the items are set", function () {
	ok(this.item1.showSmall.callCount === 0);
	ok(this.item1.hideSmall.callCount === 0);
	ok(this.item2.showSmall.callCount === 0);
	ok(this.item2.hideSmall.callCount === 0);
	ok(this.item3.showSmall.callCount === 0);
	ok(this.item3.hideSmall.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.item1.showSmall.callCount === 0);
	ok(this.item1.hideSmall.callCount === 1);
	ok(this.item2.showSmall.callCount === 0);
	ok(this.item2.hideSmall.callCount === 0);
	ok(this.item3.showSmall.callCount === 0);
	ok(this.item3.hideSmall.callCount === 0);

});

test("It tells the first prompt to show its small and second prompt to hide its small when move next is called", function () {
	ok(this.item1.showSmall.callCount === 0);
	ok(this.item1.hideSmall.callCount === 0);
	ok(this.item2.showSmall.callCount === 0);
	ok(this.item2.hideSmall.callCount === 0);
	ok(this.item3.showSmall.callCount === 0);
	ok(this.item3.hideSmall.callCount === 0);

	this.controller.setItems([this.item1, this.item2, this.item3]);

	ok(this.item1.showSmall.callCount === 0);
	ok(this.item1.hideSmall.callCount === 1);
	ok(this.item2.showSmall.callCount === 0);
	ok(this.item2.hideSmall.callCount === 0);
	ok(this.item3.showSmall.callCount === 0);
	ok(this.item3.hideSmall.callCount === 0);

	this.controller.moveNext();

	ok(this.item1.showSmall.callCount === 1);
	ok(this.item1.hideSmall.callCount === 1);
	ok(this.item2.showSmall.callCount === 0);
	ok(this.item2.hideSmall.callCount === 1);
	ok(this.item3.showSmall.callCount === 0);
	ok(this.item3.hideSmall.callCount === 0);
});
