test( "It returns a report catalog item when the type is Report", function() {  
	var catalogItemProvider = new CatalogItemProvider();

	var result = catalogItemProvider.GetItem({Type: "Report"});

	ok( result instanceof  ReportCatalogItem );
});

test( "It correctly sets the model for a report", function() {  
	var catalogItemProvider = new CatalogItemProvider();

	var result = catalogItemProvider.GetItem({Name: "Item1", Type: "Report"});

	ok( result.get("Name") === "Item1" );
});

test( "It delegates to the folder catalog item provider when the type is Folder", function() {
    var model = {Type: "Folder", Children: []};
    var expected = 10;

    var folderCatalogItemProvider = {};
    folderCatalogItemProvider.GetItem = sinon.stub();
    folderCatalogItemProvider.GetItem.withArgs(model).returns(expected);

    var catalogItemProvider = new CatalogItemProvider(folderCatalogItemProvider);

    var result = catalogItemProvider.GetItem(model);

    ok( result === expected);
});

test( "It sets the report catalog of the report catalog item", function() {
    var catalog = {Name: "Report Catalog 1"};

    var catalogItemProvider = new CatalogItemProvider({}, catalog);

    var result = catalogItemProvider.GetItem({Name: "Item1", Type: "Report"});

    ok( result.reportCatalog === catalog );
});

test( "It sets the children of a report to a empty collection", function() {
    var catalog = {Name: "Report Catalog 1"};

    var catalogItemProvider = new CatalogItemProvider({}, catalog);

    var result = catalogItemProvider.GetItem({Name: "Item1", Type: "Report"});

    ok( result.get("Children").length == 0 );
});