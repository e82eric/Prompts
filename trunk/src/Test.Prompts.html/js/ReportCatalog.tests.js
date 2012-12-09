module( "Report Catalog Tests");

test( "It flattens the items and then uses the selector to select the item", function() {
    var item1 = {Name: "Item 1"};
    var item2 = {Name: "Item 2"};
    var item3 = {Name: "Item 3"};
    var item4 = {Name: "Item 4"};

    var flattener = {};
    flattener.Flatten = sinon.stub();

    var selector = {};
    selector.Select = sinon.spy();

    var items = [item1, item2];
    var flattenedItems = [item1, item2, item3, item4];

    flattener.Flatten.withArgs(items).returns(flattenedItems);

    var reportCatalog = new ReportCatalog(flattener, selector);
    reportCatalog.setItems(items);

    reportCatalog.Select(item2);

    ok(selector.Select.calledWith(flattenedItems, item2));
    ok(selector.Select.calledOnce);
});