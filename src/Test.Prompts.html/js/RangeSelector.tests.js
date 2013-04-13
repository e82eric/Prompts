module("Range Selector Tests")

function newItem(name, isSelected) {
    var item = {Name: name, isSelected: isSelected};
    item.Select = sinon.spy();
    item.UnSelect = sinon.spy();

    return item;
}

test("It calls select on ever item between the first and the second item", function () {
    var item1 = newItem("Item 1", false);
    var item2 = newItem("Item 2", false);
    var item3 = newItem("Item 3", false);
    var item4 = newItem("Item 4", false);
    var item5 = newItem("Item 5", false);
    var item6 = newItem("Item 6", false);

    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new RangeSelector();

    selector.select(items, item2, item5);

    ok(!(item1.Select.called));
    ok(item2.Select.calledOnce);
    ok(item3.Select.calledOnce);
    ok(item4.Select.calledOnce);
    ok(item5.Select.calledOnce);
    ok(!(item6.Select.called));
});

test("It calls un select on ever item not between the first and the second item that is selected", function () {
    var item1 = newItem("Item 1", true);
    var item2 = newItem("Item 2", false);
    var item3 = newItem("Item 3", false);
    var item4 = newItem("Item 4", false);
    var item5 = newItem("Item 5", true);
    var item6 = newItem("Item 6", true);
    var item7 = newItem("Item 7", false);
    var item8 = newItem("Item 8", false);

    var items = [item1, item2, item3, item4, item5, item6, item7, item8];

    var selector = new RangeSelector();

    selector.select(items, item2, item5);

    ok(item1.UnSelect.calledOnce);
    ok(!(item2.UnSelect.called));
    ok(!(item3.UnSelect.called));
    ok(!(item4.UnSelect.called));
    ok(!(item5.UnSelect.called));
    ok(item6.UnSelect.calledOnce);
    ok(!(item7.UnSelect.calledOnce));
    ok(!(item8.UnSelect.calledOnce));
});

test("It calls select on ever item between the first and the second item when the first item is after the second item", function () {
    var item1 = newItem("Item 1", false);
    var item2 = newItem("Item 2", false);
    var item3 = newItem("Item 3", false);
    var item4 = newItem("Item 4", false);
    var item5 = newItem("Item 5", false);
    var item6 = newItem("Item 6", false);

    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new RangeSelector();

    selector.select(items, item5, item2);

    ok(!(item1.Select.called));
    ok(item2.Select.calledOnce);
    ok(item3.Select.calledOnce);
    ok(item4.Select.calledOnce);
    ok(item5.Select.calledOnce);
    ok(!(item6.Select.called));
});

test("It calls un select on ever item not between the first and the second item and is selected when the first item is after the second item", function () {
    var item1 = newItem("Item 1", false);
    var item2 = newItem("Item 2", true);
    var item3 = newItem("Item 3", true);
    var item4 = newItem("Item 4", false);
    var item5 = newItem("Item 5", true);
    var item6 = newItem("Item 6", true);

    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new RangeSelector();

    selector.select(items, item5, item2);

    ok(!(item1.UnSelect.called));
    ok(!(item2.UnSelect.called));
    ok(!(item3.UnSelect.called));
    ok(!(item4.UnSelect.called));
    ok(!(item5.UnSelect.called));
    ok(item6.UnSelect.calledOnce);
});

test("It calls select on only the item when it is the first and second item", function () {
    var item1 = newItem("Item 1", false);
    var item2 = newItem("Item 2", false);
    var item3 = newItem("Item 3", false);
    var item4 = newItem("Item 4", false);
    var item5 = newItem("Item 5", false);
    var item6 = newItem("Item 6", false);

    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new RangeSelector();

    selector.select(items, item2, item2);

    ok(!(item1.Select.called));
    ok(item2.Select.calledOnce);
    ok(!(item3.Select.called));
    ok(!(item4.Select.called));
    ok(!(item5.Select.called));
    ok(!(item6.Select.called));
});

test("It calls un select on every item that is is selected except for the item when it is both the first and second item", function () {
    var item1 = newItem("Item 1", false);
    var item2 = newItem("Item 2", true);
    var item3 = newItem("Item 3", true);
    var item4 = newItem("Item 4", false);
    var item5 = newItem("Item 5", true);
    var item6 = newItem("Item 6", true);

    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new RangeSelector();

    selector.select(items, item2, item2);

    ok(!(item1.UnSelect.called));
    ok(!(item2.UnSelect.called));
    ok(item3.UnSelect.calledOnce);
    ok(!(item4.UnSelect.called));
    ok(item5.UnSelect.calledOnce);
    ok(item6.UnSelect.calledOnce);
});