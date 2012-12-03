test( "It calls the collapse delegate when it is called the first time", function() {
	var numberOfNoneCalls = 0;
	var numberOfExpandCalls = 0;
	var numberOfCollapseCalls = 0;

	var folder = new FolderCatalogItem({Children: [{Name: "Child"}]});
	folder.changeToggle(
		function () {numberOfExpandCalls++;},
		function () {numberOfCollapseCalls++;});

	ok( numberOfCollapseCalls === 1 );
	ok( numberOfExpandCalls === 0 );
	ok( numberOfNoneCalls === 0 );
});

test( "It calls the expand delegate when change toggle is called", function() {  
	var numberOfExpandCalls = 0;
	var numberOfCollapseCalls = 0;

	var folder = new FolderCatalogItem({Children: [{Name: "Child"}]});
	folder.changeToggle(
		function () {},
		function () {});

	folder.changeToggle(
		function () {numberOfExpandCalls++;},
		function () {numberOfCollapseCalls++;});

	ok( numberOfCollapseCalls === 0 );
	ok( numberOfExpandCalls === 1 );
});

test( "It calls the collapse delegate when change toggle is called again", function() {  
	var numberOfExpandCalls = 0;
	var numberOfCollapseCalls = 0;

	var folder = new FolderCatalogItem({Children: [{Name: "Child"}]});
	folder.changeToggle(
		function () {},
		function () {});

	folder.changeToggle(
		function () {},
		function () {});

	folder.changeToggle(
		function () {numberOfExpandCalls++;},
		function () {numberOfCollapseCalls++;});

	ok( numberOfCollapseCalls === 1 );
	ok( numberOfExpandCalls === 0 );
});

test( "It calls the expand delegate when change toggle is called for a third time", function() {  
	var numberOfExpandCalls = 0;
	var numberOfCollapseCalls = 0;

	var folder = new FolderCatalogItem({Children: [{Name: "Child"}]});
	folder.changeToggle(
		function () {},
		function () {});

	folder.changeToggle(
		function () {},
		function () {});

	folder.changeToggle(
		function () {},
		function () {});

	folder.changeToggle(
		function () {numberOfExpandCalls++;},
		function () {numberOfCollapseCalls++;});

	ok( numberOfCollapseCalls === 0 );
	ok( numberOfExpandCalls === 1 );
});