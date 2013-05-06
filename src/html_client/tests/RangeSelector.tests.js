module("Range Selector Tests")

function newItem(name, isSelected) {
    var item = {Name: name, isSelected: isSelected};
    item.select = sinon.spy();
    item.unSelect = sinon.spy();

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

    ok(!(item1.select.called));
    ok(item2.select.calledOnce);
    ok(item3.select.calledOnce);
    ok(item4.select.calledOnce);
    ok(item5.select.calledOnce);
    ok(!(item6.select.called));
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

    ok(item1.unSelect.calledOnce);
    ok(!(item2.unSelect.called));
    ok(!(item3.unSelect.called));
    ok(!(item4.unSelect.called));
    ok(!(item5.unSelect.called));
    ok(item6.unSelect.calledOnce);
    ok(!(item7.unSelect.calledOnce));
    ok(!(item8.unSelect.calledOnce));
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

    ok(!(item1.select.called));
    ok(item2.select.calledOnce);
    ok(item3.select.calledOnce);
    ok(item4.select.calledOnce);
    ok(item5.select.calledOnce);
    ok(!(item6.select.called));
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

    ok(!(item1.unSelect.called));
    ok(!(item2.unSelect.called));
    ok(!(item3.unSelect.called));
    ok(!(item4.unSelect.called));
    ok(!(item5.unSelect.called));
    ok(item6.unSelect.calledOnce);
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

    ok(!(item1.select.called));
    ok(item2.select.calledOnce);
    ok(!(item3.select.called));
    ok(!(item4.select.called));
    ok(!(item5.select.called));
    ok(!(item6.select.called));
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

    ok(!(item1.unSelect.called));
    ok(!(item2.unSelect.called));
    ok(item3.unSelect.calledOnce);
    ok(!(item4.unSelect.called));
    ok(item5.unSelect.calledOnce);
    ok(item6.unSelect.calledOnce);
});