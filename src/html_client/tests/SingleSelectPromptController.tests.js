module("Single Select Prompt Controller", {
	setup: function () {
		var model = {};
		this.availableItemsController = {};
		this.controller = new SingleSelectPromptController(model, this.availableItemsController);
		this.controller.setReadyForExecution = sinon.spy();
		this.view = {
			close: sinon.spy(),
			open: sinon.spy(),
			setSelectedItemText: sinon.spy()
		}
	}
});

test("It calls close on the view when it is set", function () {
	var view = { close: sinon.spy() };

	this.controller.setView(view);
	ok(view.close.callCount === 1);
});

test("It sets the views selected item text to the models label when a selection is set", function () {
	var item = { model: { Label: "selection 1" } };
	this.controller.setView(this.view);

	ok(this.view.setSelectedItemText.callCount === 0);

	this.controller.onSelection(item);

	ok(this.view.setSelectedItemText.callCount === 1);
	ok(this.view.setSelectedItemText.withArgs(item.model.Label).callCount === 1);
});

test("It calls close on the view when a selection is set", function () {
	var item = {model:{Label:"selection 1"}};
	this.controller.setView(this.view);

	ok(this.view.close.callCount === 1);

	this.controller.onSelection(item);

	ok(this.view.close.callCount === 2);
});

test("It calls set ready for execution after the selection is set", function () {
	var item = {model:{Label:"selection 1"}};

	this.controller.setView(this.view);

	var setReadyForExecutionCalled = false;

	this.controller.setReadyForExecution = function () {
		ok(this.selections()[0] === item.model);
		setReadyForExecutionCalled = true;
	};

	ok(!setReadyForExecutionCalled);

	this.controller.onSelection(item);

	ok(setReadyForExecutionCalled);
});

test("It calls select on the default item after it sets the view when there is default", function () {
	var clickedCalled = false;

	var model = {};
	var availableItemsController = {};
	var defaultItem = {};
	var controller = new SingleSelectPromptController(model, availableItemsController, defaultItem);
	
	var view = {
		close: sinon.spy(),
		setSelectedItemText: sinon.spy()
	}

	defaultItem.clicked = function () {
		ok(controller.view === view);
		clickedCalled = true;
	}

	controller.setReadyForExecution = sinon.spy();

	ok(!clickedCalled);

	controller.setView(view);

	ok(clickedCalled);
});

test("It returns false for ready for execution when a selection has not been made", function () {
	this.controller.setView(this.view);
	ok(!this.controller.evaluateReadyForExecution());
});

test("It returns true for ready for execution when a selection has been made", function () {
	this.controller.setView(this.view);
	this.controller.onSelection({model:{Label: "selection 1"}});
	ok(this.controller.evaluateReadyForExecution());
});

test("It calls open on the view the first time the toggle is clicked", function () {
	this.controller.setView(this.view);

	ok(this.view.close.callCount === 1);
	ok(this.view.open.callCount === 0);

	this.controller.onToggleClick();

	ok(this.view.close.callCount === 1);
	ok(this.view.open.callCount === 1);
});

test("It calls close on the view the second time the toggle is clicked", function () {
	this.controller.setView(this.view);

	this.controller.onToggleClick();

	ok(this.view.close.callCount === 1);
	ok(this.view.open.callCount === 1);

	this.controller.onToggleClick();

	ok(this.view.close.callCount === 2);
	ok(this.view.open.callCount === 1);
});

test("It calls open on the view the third time the toggle is clicked", function () {
	this.controller.setView(this.view);

	this.controller.onToggleClick();

	this.controller.onToggleClick();

	ok(this.view.close.callCount === 2);
	ok(this.view.open.callCount === 1);

	this.controller.onToggleClick();

	ok(this.view.close.callCount === 2);
	ok(this.view.open.callCount === 2);
});

test("It calls close on the view when an outside click is called ", function () {
	this.controller.setView(this.view);

	ok(this.view.close.callCount === 1);
	ok(this.view.open.callCount === 0);
	this.controller.onOutsideClick();

	ok(this.view.close.callCount === 2);
	ok(this.view.open.callCount === 0);
});