test( "It calls select on the item and unselect on the rest", function() {
    var item1Select = sinon.spy();
    var item2Select = sinon.spy();
    var item3Select = sinon.spy();

    var item1UnSelect = sinon.spy();
    var item2UnSelect = sinon.spy();
    var item3UnSelect = sinon.spy();

    var item1 = {Select: item1Select, UnSelect: item1UnSelect};
    var item2 = {Select: item2Select, UnSelect: item2UnSelect};
    var item3 = {Select: item3Select, UnSelect: item3UnSelect};
    var items = [item1, item2, item3];

    var selector = new SingleSelector();

    selector.Select(items, item2);

    ok(item1Select.callCount == 0);
    ok(item2Select.calledOnce);
    ok(item3Select.callCount == 0);

    ok(item1UnSelect.calledOnce);
    ok(item2UnSelect.callCount == 0);
    ok(item3UnSelect.calledOnce);
});