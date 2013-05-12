module("Items Disposer");

test("It calls delete on all of the items", function () {
	var item1 = { deleteItem: sinon.spy() };
	var item2 = { deleteItem: sinon.spy() };
	var item3 = { deleteItem: sinon.spy() };

	var disposer = new ItemsDisposer();

	disposer.dispose([item1, item2, item3]);

	ok(item1.deleteItem.callCount === 1);
	ok(item2.deleteItem.callCount === 1);
	ok(item3.deleteItem.callCount === 1);
});
