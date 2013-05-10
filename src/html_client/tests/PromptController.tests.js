module("Prompt Controller", {
	setup: function () {
		var model = {name: "model"};
		this.promptsController = {evaluateReadyForExecution: sinon.spy()};
		this.view = { 
			setExecutionIndicatorReady: sinon.spy(), 
			setExecutionIndicatorNotReady: sinon.spy(),
			deleteItem: sinon.spy() 
		};
		this.controller = new PromptController(model, this.promptsController);
		this.controller.evaluateReadyForExecution = sinon.stub();
		this.controller.setView(this.view);
	}
});

test("It calls delete on the view when deleted", function () {
	ok(this.view.deleteItem.callCount === 0);
	this.controller.deleteItem();
	ok(this.view.deleteItem.callCount === 1);
});

test("It sets ready for execution to true when sub class evaluate method is true", function () {
	this.controller.evaluateReadyForExecution.returns(true);

	ok(!this.controller.readyForExecution);

	this.controller.setReadyForExecution();

	ok(this.controller.readyForExecution);
});

test("It calls set execution indicator ready on the view when sub class evaluate method is true", function () {
	var controller = new PromptController({}, this.promptsController);
	controller.evaluateReadyForExecution = sinon.stub();
	var view = { setExecutionIndicatorReady: sinon.spy(), setExecutionIndicatorNotReady: sinon.spy() };
	controller.view = view;

	controller.evaluateReadyForExecution.returns(true);

	ok(view.setExecutionIndicatorNotReady.callCount === 0);
	ok(view.setExecutionIndicatorReady.callCount === 0);

	controller.setReadyForExecution();

	ok(view.setExecutionIndicatorNotReady.callCount === 0);
	ok(view.setExecutionIndicatorReady.callCount === 1);
});

test("It calls evaluate ready for execution on the prompts controller when sub class evaluate method is true", function () {
	this.controller.evaluateReadyForExecution.returns(true);

	ok(this.promptsController.evaluateReadyForExecution.callCount === 1);

	this.controller.setReadyForExecution();

	ok(this.promptsController.evaluateReadyForExecution.callCount === 2);
});

test("It sets ready for execution to false when sub class evaluate method is false", function () {
	this.controller.evaluateReadyForExecution.returns(false);

	ok(!this.controller.readyForExecution);

	this.controller.setReadyForExecution();

	ok(!this.controller.readyForExecution);
});

test("It calls set execution indicator not ready on the view when sub class evaluate method is false", function () {
	this.controller.evaluateReadyForExecution.returns(false);

	ok(this.view.setExecutionIndicatorReady.callCount === 0);
	ok(this.view.setExecutionIndicatorNotReady.callCount === 1);

	this.controller.setReadyForExecution();

	ok(this.view.setExecutionIndicatorReady.callCount === 0);
	ok(this.view.setExecutionIndicatorNotReady.callCount === 2);
});

test("It calls evaluate ready for execution on the prompts controller when sub class evaluate method is false", function () {
	this.controller.evaluateReadyForExecution.returns(false);

	ok(this.promptsController.evaluateReadyForExecution.callCount === 1);

	this.controller.setReadyForExecution();

	ok(this.promptsController.evaluateReadyForExecution.callCount === 2);
});

test("It calls set ready for execution when the view is set", function () {
	var controller = new PromptController();
	controller.setReadyForExecution = sinon.spy();

	ok(controller.setReadyForExecution.callCount === 0);

	controller.setView({});

	ok(controller.setReadyForExecution.callCount === 1);
});

test("It sets the view before it calls set ready for execution", function () {
	var view = {name: "view1"};
	var isViewSet = false;
	var controller = new PromptController();
	
	controller.setReadyForExecution = function () {
		if(controller.view === view) {
			isViewSet = true;
		}
	};

	controller.setView(view)

	ok(isViewSet);
});

test("It uses the model's name for the selection info's PromptName", function () {
	var model = { Name: "Model 1" };
	var controller = new PromptController(model);
	controller.selections = sinon.stub();
	ok(controller.selectionInfo().PromptName === model.Name);
});

test("It uses the sub classes selections for the selection info's Selections", function () {
	var expected = [1, 2, 3];
	var model = { Name: "Model 1" };
	var controller = new PromptController(model);
	controller.selections = sinon.stub().returns(expected);
	ok(controller.selectionInfo().Selections === expected);
});

