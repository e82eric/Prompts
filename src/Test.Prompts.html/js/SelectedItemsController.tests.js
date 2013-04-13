module("Selected Items Controller");

test("It calls add items on the view", function (){
    var item1 = {model: {Name: "Item 1"}};
    var item2 = {model: {Name: "Item 2"}};
    var item3 = {model: {Name: "Item 3"}};
    var item4 = {model: {Name: "Item 4"}};

    var view = {};
    view.addItems = sinon.spy();

    var controller = new SelectedItemsController({});
    controller.setView(view);

    ok(!(view.addItems.called));

    controller.addItems([item1, item2, item3, item4]);

    ok(view.addItems.calledOnce);
    ok(view.addItems.withArgs([item1, item2, item3, item4]).calledOnce);
});

test("It does not call add items on the view for models that have already been added", function (){
    var model1 = {model: {Name: "Item 1"}};
    var model2 = {model: {Name: "Item 2"}};
    var model3 = {model: {Name: "Item 3"}};
    var model4 = {model: {Name: "Item 4"}};
    var model5 = {model: {Name: "Item 5"}};

    var item1 = {model: model1};
    var item2 = {model: model2};
    var item3 = {model: model3};
    var item4 = {model: model4};

    var item5 = {model: model5};
    var item6 = {model: model1};
    var item7 = {model: model2};


    var view = {};
    view.addItems = sinon.spy();

    var controller = new SelectedItemsController({});
    controller.setView(view);

    ok(!(view.addItems.called));

    controller.addItems([item1, item2]);

    ok(view.addItems.calledOnce);
    ok(view.addItems.withArgs([item1, item2]).calledOnce);

    controller.addItems([item5,item6,item7]);

    ok(view.addItems.calledTwice);
    ok(view.addItems.withArgs([item5]).calledOnce);
});

test("It calls delete on all of the items where is selected is true", function (){
    var item1 = {isSelected: true, model: {Name: "Item 1"}, deleteItem: sinon.spy()};
    var item2 = {isSelected: true, model: {Name: "Item 2"}, deleteItem: sinon.spy()};
    var item3 = {isSelected: false, model: {Name: "Item 3"}, deleteItem: sinon.spy()};
    var item4 = {isSelected: true, model: {Name: "Item 4"}, deleteItem: sinon.spy()};

    var view = {};
    view.addItems = sinon.spy();

    var controller = new SelectedItemsController({});
    controller.setView(view);
    controller.addItems([item1, item2, item3, item4]);
    controller.removeSelected();

    ok(item1.deleteItem.calledOnce);
    ok(item2.deleteItem.calledOnce);
    ok(!(item3.deleteItem.called));
    ok(item4.deleteItem.calledOnce);
});

test("It will re add the item if it has been removed", function (){
    var model1 = {model: {Name: "Item 1"}};
    var model2 = {model: {Name: "Item 2"}};
    var model3 = {model: {Name: "Item 3"}};
    var model4 = {model: {Name: "Item 4"}};
    var model5 = {model: {Name: "Item 5"}};

    var item1 = {isSelected: true, model: model1, deleteItem: sinon.spy()};
    var item2 = {isSelected: false,model: model2, deleteItem: sinon.spy()};
    var item3 = {isSelected: false,model: model3, deleteItem: sinon.spy()};
    var item4 = {isSelected: true,model: model4, deleteItem: sinon.spy()};

    var item5 = {model: model5, deleteItem: sinon.spy()};
    var item6 = {model: model1, deleteItem: sinon.spy()};
    var item7 = {model: model2, deleteItem: sinon.spy()};

    var view = {};
    view.addItems = sinon.spy();

    var controller = new SelectedItemsController({});
    controller.setView(view);

    controller.addItems([item1, item2, item3, item4]);

    controller.removeSelected();

    controller.addItems([item5, item6, item7]);

    ok(view.addItems.withArgs([item5, item6]).calledOnce);
});