module("Execute Report Controller", {
	setup: function () {
		this.reportRenderer = { execute: sinon.spy() };
		this.promptingController = { getExecuteRequest: sinon.stub() };
		this.repository = {};
		this.view = {
			disable: sinon.spy(), 
			enable: sinon.spy(),
			showRetry: sinon.spy(),
			hideRetry: sinon.spy(),
			showLoaded: sinon.spy(),
			hideLoaded: sinon.spy(),
			showLoading: sinon.spy(),
			hideLoading: sinon.spy(),
			hideError: sinon.spy(),
			showError: sinon.spy(),
			setErrorMessage: sinon.spy()
		};
		this.controller = new ExecuteReportController(this.reportRenderer);
		this.controller.setPromptingController(this.promptingController);
		this.controller.setRepository(this.repository);
		this.controller.setView(this.view);
	}
});

test("It calls disable on the view when can execute is set to false", function () {
	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 0);

	this.controller.setCanExecute(false);

	ok(this.view.disable.callCount === 1);
	ok(this.view.enable.callCount === 0);
});

test("It calls enable on the view when can execute is set to true", function () {
	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 0);

	this.controller.setCanExecute(true);

	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 1);
});

test("It does not calls disable on the view when it is set to loading", function () {
	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 0);

	this.controller.setCanExecute(true);

	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 1);

	this.controller.showLoading();

	ok(this.view.disable.callCount === 1);
	ok(this.view.enable.callCount === 1);
});

test("It does not call enable on the view when can execute is set to true while it is loading", function () {
	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 0);

	this.controller.showLoading();

	ok(this.view.disable.callCount === 1);
	ok(this.view.enable.callCount === 0);

	this.controller.setCanExecute(true);

	ok(this.view.disable.callCount === 2);
	ok(this.view.enable.callCount === 0);
});

test("It calls enable on the view when loaded is called when can execute is set to true while loading", function () {
	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 0);

	this.controller.showLoading();

	ok(this.view.disable.callCount === 1);
	ok(this.view.enable.callCount === 0);

	this.controller.setCanExecute(true);

	ok(this.view.disable.callCount === 2);
	ok(this.view.enable.callCount === 0);

	this.controller.showLoaded();

	ok(this.view.disable.callCount === 2);
	ok(this.view.enable.callCount === 1);
});

test("It calls enable on the view when show error is called when can execute is set to true while loading", function () {
	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 0);

	this.controller.showLoading();

	ok(this.view.disable.callCount === 1);
	ok(this.view.enable.callCount === 0);

	this.controller.setCanExecute(true);

	ok(this.view.disable.callCount === 2);
	ok(this.view.enable.callCount === 0);

	this.controller.showError("some error");

	ok(this.view.disable.callCount === 2);
	ok(this.view.enable.callCount === 1);
});

test("It does not call enable on the view when loaded is called when can execute is set to false", function () {
	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 0);

	this.controller.showLoading();

	ok(this.view.disable.callCount === 1);
	ok(this.view.enable.callCount === 0);

	this.controller.setCanExecute(false);

	ok(this.view.disable.callCount === 2);
	ok(this.view.enable.callCount === 0);

	this.controller.showLoaded();

	ok(this.view.disable.callCount === 3);
	ok(this.view.enable.callCount === 0);
});

test("It does not call enable on the view when show error is called when can execute is set to false", function () {
	ok(this.view.disable.callCount === 0);
	ok(this.view.enable.callCount === 0);

	this.controller.showLoading();

	ok(this.view.disable.callCount === 1);
	ok(this.view.enable.callCount === 0);

	this.controller.setCanExecute(false);

	ok(this.view.disable.callCount === 2);
	ok(this.view.enable.callCount === 0);

	this.controller.showError("some error");

	ok(this.view.disable.callCount === 3);
	ok(this.view.enable.callCount === 0);
});

test("It calls execute on the report renderer with the result from the execution id service", function () {
	var numberOfGetCalls = 0;

	var request = {name: "request 1"};
	var executionId = "id 1";
	var capturedCallback = undefined;
	this.promptingController.getExecuteRequest.returns(request);

	this.repository.get = function (capturedRequest, callback) {
		numberOfGetCalls++;
		capturedCallback = callback;
		ok(capturedRequest == request);
	};

	ok(this.reportRenderer.execute.callCount === 0);
	ok(numberOfGetCalls === 0);

	this.controller.execute();

	ok(this.reportRenderer.execute.callCount === 0);
	ok(numberOfGetCalls === 1);

	capturedCallback(executionId);

	ok(this.reportRenderer.execute.callCount === 1);
	ok(this.reportRenderer.execute.withArgs(executionId).callCount === 1);
	ok(numberOfGetCalls === 1);
});