module("Shopping Cart Controller");

test("It calls add items on the selected items controller with the selected items of the available items controller when select is called", function () {
    var selectedAvailableItems = [{Name: "Item 1"}, {Name: "Item 2"}, {Name: "Item 3"}];

    var availableItemsController = {};
    availableItemsController.getSelectedItems = sinon.stub().returns(selectedAvailableItems);
    var selectedItemsController = {};
    selectedItemsController.addItems = sinon.spy();

    var shoppingCartController = new ShoppingCartController(availableItemsController, selectedItemsController);

    ok( !(selectedItemsController.addItems.called) );

    shoppingCartController.onSelect();

    ok( selectedItemsController.addItems.calledOnce );
    ok( selectedItemsController.addItems.withArgs(selectedAvailableItems).calledOnce);
});