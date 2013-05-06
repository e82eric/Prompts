module("Multi Select Prompt Controller", {
	setup: function () {
		var model = {};
		this.availableItemsController = { getSelectedItems: sinon.stub() };
		this.selectedItemsController = { addItems: sinon.spy(), removeSelected: sinon.spy(), items: [] };
		this.controller = new MultiSelectPromptController(
			this.model, 
			this.availableItemsController, 
			this.selectedItemsController);

		this.controller.setReadyForExecution = sinon.spy();
	}
});

test("It reports ready for execution when the selected items controller has one item selected", function () {
	this.selectedItemsController.items = [1];
	ok(this.controller.evaluateReadyForExecution());
});

test("It reports ready for execution when the selected items controller has more than one item selected", function () {
	this.selectedItemsController.items = [1, 2, 3];
	ok(this.controller.evaluateReadyForExecution());
});

test("It reports not ready for execution when the selected items controller has no items selected", function () {
	this.selectedItemsController.items = [];
	ok(!this.controller.evaluateReadyForExecution());
});

test("It adds the selected items from the available items controller to the selected items controller when select is called", function () {
	var expected = [1,2,3];
	this.availableItemsController.getSelectedItems.returns(expected);

	ok(this.selectedItemsController.addItems.callCount === 0);

	this.controller.onSelect();

	ok(this.selectedItemsController.addItems.callCount === 1);
	ok(this.selectedItemsController.addItems.withArgs(expected).callCount === 1);
});

test("It calls set ready for execution after it adds the items to the selected items controller when select is called", function () {
	var items = [1,2,3];
	var setReadyForExecutionCalled = false;
	this.availableItemsController.getSelectedItems.returns(items);

	this.controller.setReadyForExecution = function () {
		setReadyForExecutionCalled = true;
		ok(this.selectedItemsController.addItems.withArgs(items).callCount === 1);
	};

	this.controller.onSelect();

	ok(setReadyForExecutionCalled);
});

test("It calls remove selected on the selected items controller when un select is called", function () {
	ok(this.selectedItemsController.removeSelected.callCount === 0);

	this.controller.onUnSelect();

	ok(this.selectedItemsController.removeSelected.callCount === 1);
});

test("It calls set ready for execution after it removes the items from the selected items controller when un select is called", function () {
	var setReadyForExecutionCalled = false;

	this.controller.setReadyForExecution = function () {
		setReadyForExecutionCalled = true;
		ok(this.selectedItemsController.removeSelected.callCount === 1);
	};

	this.controller.onUnSelect();

	ok(setReadyForExecutionCalled);
});

test("It returns all of the models from the selected items controller for selections", function (){
	var model1 = {name: "model 1"};
	var model2 = {name: "model 2"};
	var model3 = {name: "model 3"};

	this.selectedItemsController.items = [{model: model1}, {model:model2},{model:model3}];

	var result = this.controller.selections();

	ok(result.length === 3);
	ok(result[0] === model1);
	ok(result[1] === model2);
	ok(result[2] === model3);
});