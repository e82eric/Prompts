test( "It returns one item where there is only one item and no children", function() {
    var flattener = new HierarchyFlattener();

    var item = new Backbone.Model({Name: "Item 1", Children: new Backbone.Collection()});
    var collection = new Backbone.Collection([item]);

    var result = flattener.Flatten(collection);

    ok(result.length == 1);
    ok(result[0] == item);
});

test( "It returns the root items when there are no children", function() {
    var flattener = new HierarchyFlattener();

    var item1 = new Backbone.Model({Name: "Item 1", Children: new Backbone.Collection()});
    var item2 = new Backbone.Model({Name: "Item 2", Children: new Backbone.Collection()});
    var item3 = new Backbone.Model({Name: "Item 3", Children: new Backbone.Collection()});

    var collection = new Backbone.Collection([item1, item2, item3]);

    var result = flattener.Flatten(collection);

    ok(result.length == 3);
    ok(result[0] == item1);
    ok(result[1] == item2);
    ok(result[2] == item3);
});

test( "It returns the children after the parent", function() {
    var flattener = new HierarchyFlattener();

    var child1 = new Backbone.Model({Name: "Child 1", Children: new Backbone.Collection()});
    var child2 = new Backbone.Model({Name: "Child 2", Children: new Backbone.Collection()});
    var childCollection = new Backbone.Collection([child1, child2]);

    var item1 = new Backbone.Model({Name: "Item 1", Children: new Backbone.Collection()});
    var item2 = new Backbone.Model({Name: "Item 2", Children: new Backbone.Collection()});
    var item3 = new Backbone.Model({Name: "Item 3", Children: childCollection });

    var collection = new Backbone.Collection([item1, item2, item3]);

    var result = flattener.Flatten(collection);

    ok(result.length == 5);
    ok(result[0] == item1);
    ok(result[1] == item2);
    ok(result[2] == item3);
    ok(result[3] == child1);
    ok(result[4] == child2);
});

test( "It returns the grand children after the children", function() {
    var flattener = new HierarchyFlattener();

    var grandChild1 = new Backbone.Model({Name: "GrandChild 1", Children: new Backbone.Collection()});
    var grandChild2 = new Backbone.Model({Name: "GrandChild 2", Children: new Backbone.Collection()});
    var grandChild3 = new Backbone.Model({Name: "GrandChild 3", Children: new Backbone.Collection()});
    var grandChildrenCollection = new Backbone.Collection([grandChild1, grandChild2, grandChild3]);

    var child1 = new Backbone.Model({Name: "Child 1", Children: grandChildrenCollection});
    var child2 = new Backbone.Model({Name: "Child 2", Children: new Backbone.Collection()});
    var childCollection = new Backbone.Collection([child1, child2]);

    var item1 = new Backbone.Model({Name: "Item 1", Children: new Backbone.Collection()});
    var item2 = new Backbone.Model({Name: "Item 2", Children: new Backbone.Collection()});
    var item3 = new Backbone.Model({Name: "Item 3", Children: childCollection });

    var collection = new Backbone.Collection([item1, item2, item3]);

    var result = flattener.Flatten(collection);

    ok(result.length == 8);
    ok(result[0] == item1);
    ok(result[1] == item2);
    ok(result[2] == item3);
    ok(result[3] == child1);
    ok(result[4] == grandChild1);
    ok(result[5] == grandChild2);
    ok(result[6] == grandChild3);
    ok(result[7] == child2);
});