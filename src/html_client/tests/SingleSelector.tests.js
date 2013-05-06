module( "Single Selector Tests", {
    setup: function () {
        this.newItem = function (name, isSelected) {
            var item = {Name: name, isSelected: isSelected};
            item.select = sinon.spy();
            item.unSelect = sinon.spy();
            return item;
        }
    }
});

test( "It calls select on the item and un select on the rest", function() {
    var item1 = this.newItem("item 1", false);
    var item2 = this.newItem("item 2", false);
    var item3 = this.newItem("item 3", false);
    var items = [item1, item2, item3];

    var selector = new SingleSelector();

    selector.select(items, item2);

    ok(item1.select.callCount == 0);
    ok(item2.select.calledOnce);
    ok(item3.select.callCount == 0);
});

test( "It calls un select on the other items where is selected is true", function() {
    var item1 = this.newItem("item 1", true);
    var item2 = this.newItem("item 2", false);
    var item3 = this.newItem("item 3", true);
    var item4 = this.newItem("item 4", false);
    var item5 = this.newItem("item 5", true);
    var item6 = this.newItem("item 6", false);
    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new SingleSelector();

    selector.select(items, item2);

    ok(item1.unSelect.callCount == 1);
    ok(item2.unSelect.callCount == 0);
    ok(item3.unSelect.callCount == 1);
    ok(item4.unSelect.callCount == 0);
    ok(item5.unSelect.callCount == 1);
    ok(item6.unSelect.callCount == 0);
});

test( "It does not call un select on the item to select if it is already selected", function() {
    var item1 = this.newItem("item 1", true);
    var item2 = this.newItem("item 2", true);
    var item3 = this.newItem("item 3", true);
    var item4 = this.newItem("item 4", false);
    var item5 = this.newItem("item 5", true);
    var item6 = this.newItem("item 6", false);
    var items = [item1, item2, item3, item4, item5, item6];

    var selector = new SingleSelector();

    selector.select(items, item2);


    ok(item2.unSelect.callCount == 0);
});