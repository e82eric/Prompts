test( "It returns a empty folder catalog item where there are no children", function() {
    var catalogItemProvider = new FolderCatalogItemProvider();

    var result = catalogItemProvider.GetItem({Children: []});

    ok( result instanceof EmptyFolderCatalogItem );
});

test( "It returns a folder catalog item where there are children", function() {
    var catalogItemsProvider = {};
    catalogItemsProvider.GetItems = sinon.stub();

    var catalogItemProvider = new FolderCatalogItemProvider();
    catalogItemProvider.setCatalogItemsProvider(catalogItemsProvider);

    var result = catalogItemProvider.GetItem({Children: [{Name: "Child 1"}, {Name: "Child 2"}]});

    ok( result instanceof FolderCatalogItem );
});

test( "It correctly sets the model for a folder", function() {
    var catalogItemProvider = new FolderCatalogItemProvider();

    var result = catalogItemProvider.GetItem({Name: "Item1", Children: []});

    ok( result.get("Name") === "Item1" );
});

test( "It set the children using the items provider", function() {
    var item = {Name: "Item1", Type: "Folder", Children: [{Name: "Child1", Type: "Report", Children: []}]};

    var catalogItemsProvider = {};
    catalogItemsProvider.GetItems = sinon.stub();
    catalogItemsProvider.GetItems.withArgs(item.Children).returns(1);

    var catalogItemProvider = new FolderCatalogItemProvider();
    catalogItemProvider.setCatalogItemsProvider(catalogItemsProvider);

    var result = catalogItemProvider.GetItem(item);

    ok( result.get("Children") === 1 );
});