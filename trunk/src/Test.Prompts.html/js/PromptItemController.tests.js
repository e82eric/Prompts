test("It is constructed with is selected set to false", function () {
    var controller = new PromptItemController({},{});
    ok(controller.isSelected == false);
});

test("It sets is selected to true when select is called", function () {
    var promptItemView = {};
    promptItemView.onSelected = sinon.spy();

    var controller = new PromptItemController({},{});
    controller.setView(promptItemView);
    controller.Select();
    ok(controller.isSelected);
});

test("It sets is selected to true when select is called again", function () {
    var promptItemView = {};
    promptItemView.onSelected = sinon.spy();

    var controller = new PromptItemController({},{});
    controller.setView(promptItemView);
    controller.Select();
    controller.Select();
    ok(controller.isSelected);
});

test("It sets is selected to false when un select is called", function () {
    var promptItemView = {};
    promptItemView.onUnSelected = sinon.spy();

    var controller = new PromptItemController({},{});
    controller.setView(promptItemView);
    controller.UnSelect();
    ok(controller.isSelected == false);
});

test("It sets is selected to false when un select is called again", function () {
    var promptItemView = {};
    promptItemView.onUnSelected = sinon.spy();

    var controller = new PromptItemController({},{});
    controller.setView(promptItemView);
    controller.UnSelect();
    controller.UnSelect();
    ok(controller.isSelected == false);
});