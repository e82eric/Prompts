function newItem(name, isSelected) {
    var item = {Name: name};
    item.Select = sinon.spy();
    item.UnSelect = sinon.spy();
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

    ok(!(item1.Select.called));
    ok(!(item1.UnSelect.called));
    ok(!(item2.Select.called));
    ok(!(item2.UnSelect.called));
    ok(!(item3.Select.called));
    ok(!(item3.UnSelect.called));
    ok(item4.Select.calledOnce);
    ok(!(item4.UnSelect.called));
    ok(!(item5.Select.called));
    ok(!(item5.UnSelect.called));
    ok(!(item6.Select.called));
    ok(!(item6.UnSelect.called));
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

    ok(!(item1.Select.called));
    ok(!(item1.UnSelect.called));
    ok(!(item2.Select.called));
    ok(!(item2.UnSelect.called));
    ok(!(item3.Select.called));
    ok(!(item3.UnSelect.called));
    ok(item4.UnSelect.calledOnce);
    ok(!(item4.Select.called));
    ok(!(item5.Select.called));
    ok(!(item5.UnSelect.called));
    ok(!(item6.Select.called));
    ok(!(item6.UnSelect.called));
});