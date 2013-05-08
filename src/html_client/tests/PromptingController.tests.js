module("Prompting Controller", {
	setup: function () {
		this.requester = { execute: sinon.spy() };
		this.itemsController = {};
		this.loadingPanelController = {};
		this.executeCommand = { setCanExecute: sinon.spy() };
		this.controller = new PromptingController(
			this.requester,
			this.itemsController,
			this.loadingPanelController,
			this.executeCommand)
	}
});

test("It asks the requester to populate the items controller with the prompts when show is called", function () {
	var path = "path 1";
	ok(this.requester.execute.callCount === 0);
	this.controller.show(path);
	ok(this.requester.execute.callCount === 1);
	ok(this.requester.execute.args[0][0].Path === path);
	ok(this.requester.execute.args[0][1] === this.itemsController);
});

test("It enables the excute command when all prompts are ready for execution", function () {
	ok(this.executeCommand.setCanExecute.callCount === 0);

	this.itemsController.items = [
		{ name: "prompt 1", readyForExecution: true },
		{ name: "prompt 2", readyForExecution: true },
		{ name: "prompt 3", readyForExecution: true },
	];

	this.controller.evaluateReadyForExecution();

	ok(this.executeCommand.setCanExecute.callCount === 1);
	ok(this.executeCommand.setCanExecute.withArgs(true).callCount === 1);
});

test("It diables the excute command when one of the prompts is not ready for execution", function () {
	ok(this.executeCommand.setCanExecute.callCount === 0);

	this.itemsController.items = [
		{ name: "prompt 1", readyForExecution: true },
		{ name: "prompt 2", readyForExecution: false },
		{ name: "prompt 3", readyForExecution: true },
	];

	this.controller.evaluateReadyForExecution();

	ok(this.executeCommand.setCanExecute.callCount === 1);
	ok(this.executeCommand.setCanExecute.withArgs(false).callCount === 1);
});

test("It diables the excute command when fall of the prompts is not ready for execution", function () {
	ok(this.executeCommand.setCanExecute.callCount === 0);

	this.itemsController.items = [
		{ name: "prompt 1", readyForExecution: true },
		{ name: "prompt 2", readyForExecution: false },
		{ name: "prompt 3", readyForExecution: true },
	];

	this.controller.evaluateReadyForExecution();

	ok(this.executeCommand.setCanExecute.callCount === 1);
	ok(this.executeCommand.setCanExecute.withArgs(false).callCount === 1);
});

test("It uses the selections from all of prompts for the requests selections", function () {
	var selectionInfo1 = { name: "selection info 1" };
	var selectionInfo2 = { name: "selection info 2" };
	var selectionInfo3 = { name: "selection info 3" };

	this.itemsController.items = [
		{ name: "prompt 1", selectionInfo: function () { return selectionInfo1; } },
		{ name: "prompt 2", selectionInfo: function () { return selectionInfo2; } },
		{ name: "prompt 3", selectionInfo: function () { return selectionInfo3; } },
	];

	var result = this.controller.getExecuteRequest();

	ok(result.PromptSelections.length === 3);
	ok(result.PromptSelections[0] === selectionInfo1);
	ok(result.PromptSelections[1] === selectionInfo2);
	ok(result.PromptSelections[2] === selectionInfo3);
});

test("It uses the path for the Path of the request", function () {
	var path = "path 1";

	this.itemsController.items = [
		{ name: "prompt 1", selectionInfo: function () { return { name: "selection info 1" }; } },
		{ name: "prompt 2", selectionInfo: function () { return { name: "selection info 2" }; } },
		{ name: "prompt 3", selectionInfo: function () { return { name: "selection info 3" }; } },
	];

	this.controller.show(path);

	var result = this.controller.getExecuteRequest();

	ok(result.Path === path);
});