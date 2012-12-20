module( "Multi Selector Tests", {
    setup: function () {
        this.singleSelector = {};
        this.singleSelector.Select = sinon.spy();

        this.rangeSelector = {};
        this.rangeSelector.select = sinon.spy();

        this.inverseSelector = {};
        this.inverseSelector.select = sinon.spy();

        this.selector = new MultiSelector(this.singleSelector, this.rangeSelector, this.inverseSelector);
    }
});

test("It calls select on the single selector when neither the shift or control key was pressed", function (){
    var item1 = {};
    var item2 = {};
    var item3 = {};

    var items = [item1, item2, item3];

    this.selector.select(false, false, items, item1);

    ok( this.singleSelector.Select.calledOnce );
    ok( this.singleSelector.Select.withArgs(items, item1).calledOnce );
    ok( !(this.rangeSelector.select.called) );
    ok( !(this.inverseSelector.select.called) );
});

test("It calls select on the single selector when the shift is pressed but is the first selection", function (){
    var item1 = {};
    var item2 = {};
    var item3 = {};

    var items = [item1, item2, item3];

    this.selector.select(true, false, items, item1);

    ok( this.singleSelector.Select.calledOnce );
    ok( this.singleSelector.Select.withArgs(items, item1).calledOnce );
    ok( !(this.rangeSelector.select.called) );
    ok( !(this.inverseSelector.select.called) );
});

test("It calls select on the range selector when the shift is pressed and it is the second selection", function (){
    var item1 = {};
    var item2 = {};
    var item3 = {};

    var items = [item1, item2, item3];

    this.selector.select(false, false, items, item1);

    this.selector.select(true, false, items, item3);

    ok( this.singleSelector.Select.calledOnce );
    ok( this.rangeSelector.select.withArgs(items, item1, item3).calledOnce );
    ok( !(this.inverseSelector.select.called) );
});

test("It calls select on the range selector when the shift is pressed and it is the third selection", function (){
    var item1 = {Name: "Item 1"};
    var item2 = {Name: "Item 2"};
    var item3 = {Name: "Item 3"};
    var item4 = {Name: "Item 4"};
    var item5 = {Name: "Item 5"};

    var items = [item1, item2, item3];

    this.selector.select(false, false, items, item1);

    this.selector.select(true, false, items, item3);

    this.selector.select(true, false, items, item5);

    ok( this.singleSelector.Select.calledOnce );
    ok( this.rangeSelector.select.withArgs(items, item1, item5).calledOnce );
    ok( !(this.inverseSelector.select.called) );
});

test("It calls select on the inverse selector when the control key is pressed", function (){
    var item1 = {Name: "Item 1"};
    var item2 = {Name: "Item 2"};
    var item3 = {Name: "Item 3"};
    var item4 = {Name: "Item 4"};
    var item5 = {Name: "Item 5"};

    var items = [item1, item2, item3];

    this.selector.select(false, true, items, item1);

    ok( !(this.singleSelector.Select.called) );
    ok( !(this.rangeSelector.select.called) );
    ok( this.inverseSelector.select.calledOnce );
    ok( this.inverseSelector.select.withArgs(items, item1).calledOnce );
});