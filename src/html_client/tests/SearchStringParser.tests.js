module("Search String parser");

test("It returns a null search when the search string is a *", function () {
    var parser = new SearchStringParser();

    var result = parser.parse("*");

    ok(result instanceof NullSearch);
    ok(result.searchString == "");
});

test("It returns a null search when the search string is a blank", function () {
    var parser = new SearchStringParser();

    var result = parser.parse("");

    ok(result instanceof NullSearch);
    ok(result.searchString == "");
});

test("It returns a ends with search when the first character is *", function () {
    var parser = new SearchStringParser();

    var result = parser.parse("*SearchString1");

    ok(result instanceof EndsWithSearch);
    ok(result.searchString == "SearchString1");
});

test("It returns a contains with search when the first and last character is *", function () {
    var parser = new SearchStringParser();

    var result = parser.parse("*SearchString1*");

    ok(result instanceof ContainsSearch);
    ok(result.searchString == "SearchString1");
});

test("It returns a starts with search when the last character is *", function () {
    var parser = new SearchStringParser();

    var result = parser.parse("SearchString1*");

    ok(result instanceof StartsWithSearch);
    ok(result.searchString == "SearchString1");
});

test("It returns a equals search when the first and last characters are not *", function () {
    var parser = new SearchStringParser();

    var result = parser.parse("SearchString1");

    ok(result instanceof EqualsSearch);
    ok(result.searchString == "SearchString1");
});