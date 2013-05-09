module("Selectable Item Controller", {
	setup: function () {
		this.view = { onSelected: sinon.spy(), onUnSelected: sinon.spy() };
		this.controller = new SelectableItemController();
		this.controller.setView(this.view);
	}
});

test("It initalizes is selected as false", function () {
	ok(!this.controller.isSelected);
});

test("It calls on selected on the view when select is called", function () {
	ok(this.view.onSelected.callCount === 0);
	ok(this.view.onUnSelected.callCount === 0);

	this.controller.select();

	ok(this.view.onSelected.callCount === 1);
	ok(this.view.onUnSelected.callCount === 0);
});

test("It is selected when select is called", function () {
	ok(!this.controller.isSelected);

	this.controller.select();

	ok(this.controller.isSelected);
});

test("It calls on un selected on the view when un select is called", function () {
	ok(this.view.onSelected.callCount === 0);
	ok(this.view.onUnSelected.callCount === 0);

	this.controller.select();

	ok(this.view.onSelected.callCount === 1);
	ok(this.view.onUnSelected.callCount === 0);

	this.controller.unSelect();

	ok(this.view.onSelected.callCount === 1);
	ok(this.view.onUnSelected.callCount === 1);
});

test("It is selected when select is called", function () {
	ok(!this.controller.isSelected);

	this.controller.select();

	ok(this.controller.isSelected);

	this.controller.unSelect();

	ok(!this.controller.isSelected);
});