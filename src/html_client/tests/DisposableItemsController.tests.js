module("Disposable Items Controller", {
	setup: function () {
		this.disposer = { dispose: sinon.spy() };		
		this.controller = new DisposableItemsController(this.disposer);
		
	}
});

test("It correctly sets the items on the super class", function () {
	var numberOfSuperCalls = 0;
	var items = [1,2,3];

	AsynchronousItemsController.prototype.setItems = function (superItems) {
		numberOfSuperCalls++;
		ok(items === superItems);
	}

	this.controller.setItems(items);
	ok(numberOfSuperCalls === 1);
});

test("It diposes the old items", function () {
	AsynchronousItemsController.prototype.setItems = function (superItems) { this.items = superItems; }
	
	var previousItems = [1, 2, 3];

	this.controller.setItems(previousItems);
	this.controller.setView({ renderItems: sinon.spy() });

	ok(this.disposer.dispose.callCount === 0);

	this.controller.setItems([4, 5, 6]);

	ok(this.disposer.dispose.callCount === 1);
});
