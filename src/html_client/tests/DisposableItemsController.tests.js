module("Disposable Items Controller", {
	setup: function () {		
		this.controller = new DisposableItemsController();
		
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

test("It calls delete on all of the previous items", function (){
	AsynchronousItemsController.prototype.setItems = function (superItems) { this.items = superItems; }
	
	var previousItem1 = { deleteItem: sinon.spy() };
	var previousItem2 = { deleteItem: sinon.spy() };
	var previousItem3 = { deleteItem: sinon.spy() };

	this.controller.setItems([previousItem1, previousItem2, previousItem3]);

	ok(previousItem1.deleteItem.callCount === 0);
	ok(previousItem2.deleteItem.callCount === 0);
	ok(previousItem3.deleteItem.callCount === 0);

	this.controller.setItems([1,2,3]);

	ok(previousItem1.deleteItem.callCount === 1);
	ok(previousItem2.deleteItem.callCount === 1);
	ok(previousItem3.deleteItem.callCount === 1);
});