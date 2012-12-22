module("Available Items Controller");

test("It returns all of the items where is selected is true", function () {
    var model1 = {Name: "Model 1"};
    var model2 = {Name: "Model 2"};
    var model3 = {Name: "Model 3"};
    var model4 = {Name: "Model 4"};
    var model5 = {Name: "Model 5"};
    var model6 = {Name: "Model 6"};

    var item1 = {isSelected: true, model: model1};
    var item2 = {isSelected: false, model: model2 };
    var item3 = {isSelected: false, model: model3};
    var item4 = {isSelected: true, model: model4};
    var item5 = {isSelected: false, model: model5};
    var item6 = {isSelected: true, model: model6};

    var newItem1 = {Name: "New Item 1"};
    var newItem4 = {Name: "New Item 4"};
    var newItem6 = {Name: "New Item 6"};

    var items = [item1,  item2,  item3, item4, item5, item6];

    var promptItemControllersProvider = {};

    promptItemControllersProvider.get = sinon.stub()
        .withArgs([model1, model4, model6])
        .returns([newItem1, newItem4, newItem6]);

    var controller = new AvailableItemsController({}, promptItemControllersProvider);
    controller.setItems(items);

    var result = controller.getSelectedItems();

    ok( result.length == 3 );
    ok( result[0] == newItem1 );
    ok( result[1] == newItem4 );
    ok( result[2] == newItem6 );
});

test("It returns an empty array when there are no selected items", function () {
    var item1 = {isSelected: false};
    var item2 = {isSelected: false};
    var item3 = {isSelected: false};
    var item4 = {isSelected: false};
    var item5 = {isSelected: false};
    var item6 = {isSelected: false};

    var items = [item1,  item2,  item3, item4, item5, item6];

    var promptItemControllersProvider =  {};
    promptItemControllersProvider.get = sinon.stub().withArgs([]).returns([]);

    var controller = new AvailableItemsController({}, promptItemControllersProvider);
    controller.setItems(items);

    var result = controller.getSelectedItems();

    ok(result.length == 0);
});