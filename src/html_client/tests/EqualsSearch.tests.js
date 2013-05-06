module("Equals Search");

test("It returns true when the search string is the same as the models label", function () {
    var searchString = "Search String 1";

    var promptItem = {model: {Label: searchString}};

    var search = new EqualsSearch(searchString);

    ok( search.execute(promptItem) );
});

test("It returns true when the search string is the same as the models label except that it is upper case", function () {
    var searchString = "search string 1";

    var promptItem = {model: {Label: "SEARCH STRING 1"}};

    var search = new EqualsSearch(searchString);

    ok( search.execute(promptItem));
});

test("It returns false when the search string is the same as the start of the models label", function () {
    var searchString = "search string 1";

    var promptItem = {model: {Label: searchString + " More Label"}};

    var search = new EqualsSearch(searchString);

    ok( search.execute(promptItem) == false );
});

test("It returns false when the search string is the same as the end of the models label", function () {
    var searchString = "search string 1";

    var promptItem = {model: {Label: " More Label" + searchString }};

    var search = new EqualsSearch(searchString);

    ok(  search.execute(promptItem) == false );
});

test("It returns false when the search string is the same as the middle of the models label", function () {
    var searchString = "search string 1";

    var promptItem = {model: {Label: "Start Label" + searchString + "End Label"  }};

    var search = new EqualsSearch(searchString);

    ok( search.execute(promptItem) == false );
});

test("It returns false when the search string does not contain the models label", function () {
    var searchString = "search string 1";

    var promptItem = {model: {Label: "srearch string 1"  }};

    var search = new EqualsSearch(searchString);

    ok( search.execute(promptItem) == false );
});