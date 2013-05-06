module("Inverse Selector");

function newItem(name, isSelected) {
    var item = {Name: name};
    item.select = sinon.spy();
    item.unSelect = sinon.spy();
    item.isSelected = isSelected;

    return item;
}

test("It calls select on the item if it is not selected", function () {
    var item1 = newItem("Item 1", true);
    var item2 = newItem("Item 2", false);
    var item3 = newItem("Item 3", true);
    var item4 = newItem("Item 4", false);
    var item5 = newItem("Item 5", true);
    var item6 = newItem("Item 6", false);

    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new InverseSelector();

    selector.select(items, item4);

    ok(!(item1.select.called));
    ok(!(item1.unSelect.called));
    ok(!(item2.select.called));
    ok(!(item2.unSelect.called));
    ok(!(item3.select.called));
    ok(!(item3.unSelect.called));
    ok(item4.select.calledOnce);
    ok(!(item4.unSelect.called));
    ok(!(item5.select.called));
    ok(!(item5.unSelect.called));
    ok(!(item6.select.called));
    ok(!(item6.unSelect.called));
});

test("It calls un select on the item if it is selected", function () {
    var item1 = newItem("Item 1", true);
    var item2 = newItem("Item 2", false);
    var item3 = newItem("Item 3", true);
    var item4 = newItem("Item 4", true);
    var item5 = newItem("Item 5", true);
    var item6 = newItem("Item 6", false);

    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new InverseSelector();

    selector.select(items, item4);

    ok(!(item1.select.called));
    ok(!(item1.unSelect.called));
    ok(!(item2.select.called));
    ok(!(item2.unSelect.called));
    ok(!(item3.select.called));
    ok(!(item3.unSelect.called));
    ok(item4.unSelect.calledOnce);
    ok(!(item4.select.called));
    ok(!(item5.select.called));
    ok(!(item5.unSelect.called));
    ok(!(item6.select.called));
    ok(!(item6.unSelect.called));
});