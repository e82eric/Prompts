test( "It delegates Item creation to the item provider", function() {  
	var item1 = {Name: "Item1"};
	var item2 = {Name: "Item2"};

	var catalogItemProvider = {};
	catalogItemProvider.GetItem = sinon.stub();
	catalogItemProvider.GetItem.withArgs(item1).returns(new ReportCatalogItemController({"Name": "CItem1"}));
	catalogItemProvider.GetItem.withArgs(item2).returns(new ReportCatalogItemController({"Name": "CItem2"}));

	var itemsProvider = new CatalogItemsProvider(catalogItemProvider);
	var result = itemsProvider.GetItems([item1, item2]);

	ok( result.length === 2 );
	ok( result[0].model.Name === "CItem1" );
	ok( result[1].model.Name === "CItem2" );
});

test( "It returns an empty array when there are no items", function() {  

	var catalogItemProvider = {};
	catalogItemProvider.GetItem = sinon.stub();

	var itemsProvider = new CatalogItemsProvider(catalogItemProvider);
	var result = itemsProvider.GetItems([]);

	ok( result.length === 0 );
});

