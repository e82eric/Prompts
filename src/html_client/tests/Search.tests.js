module("Search");

test("It returns all of the items that return true from the search", function (){
    var promptItem1 = {Name: "Item1"};
    var promptItem2 = {Name: "Item2"};
    var promptItem3 = {Name: "Item3"};
    var promptItem4 = {Name: "Item4"};
    var promptItem5 = {Name: "Item5"};

    var itemSearch = {execute: sinon.stub()};
    itemSearch.execute.withArgs(promptItem1).returns(false);
    itemSearch.execute.withArgs(promptItem2).returns(true);
    itemSearch.execute.withArgs(promptItem3).returns(false);
    itemSearch.execute.withArgs(promptItem4).returns(true);
    itemSearch.execute.withArgs(promptItem5).returns(true);

    var search = new Search(itemSearch);

    var result = search.execute([promptItem1, promptItem2, promptItem3, promptItem4, promptItem5]);

    ok(result.length == 3);
    ok(result[0] == promptItem2);
    ok(result[1] == promptItem4);
    ok(result[2] == promptItem5);
});