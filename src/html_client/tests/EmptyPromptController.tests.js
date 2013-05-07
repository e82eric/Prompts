module("Empty Prompt Controller", {
	setup: function () {
		this.model = { Name: "Prompt 1" };
		this.controller = new EmptyPromptController(this.model);
		this.controller.setReadyForExecution = sinon.spy();
	}
});

test("It is not ready for execution when the text is null", function () {
	this.controller.onTextChanged(undefined);
	ok(!this.controller.evaluateReadyForExecution());
});

test("It is not ready for execution when the text is a empty string", function () {
	this.controller.onTextChanged("");
	ok(!this.controller.evaluateReadyForExecution());
});

test("It call set ready for execution after the text is set", function () {
	var setReadyForExecutionCalled = false;
	var text = "value 1";

	this.controller.setReadyForExecution = function () {
		setReadyForExecutionCalled = true;
		ok(this.text === text);
	};

	ok(!setReadyForExecutionCalled);
	this.controller.onTextChanged(text);
	ok(setReadyForExecutionCalled);
});

test("It uses the models name for prompt name for the selection", function () {
	ok(this.controller.selections()[0].PromptName === this.model.Name);
});

test("It uses the text for the value of the selection", function () {
	var text = "text 1";
	this.controller.onTextChanged(text);
	ok(this.controller.selections()[0].Value === text);
});

test("It only returns one selection", function () {
	var text = "text 1";
	this.controller.onTextChanged(text);
	ok(this.controller.selections().length === 1);
});