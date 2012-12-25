function SearchStringParser () {
    this.parse = function (searchString) {
        if(searchString.length == 0) {
            return new NullSearch("");
        } else if(searchString.length == 1 && searchString.substring(0, 1) == "*") {
            return new NullSearch("");
        } else if(searchString.substring(0, 1) == "*" && searchString.substring(searchString.length - 1, searchString.length) == "*") {
            var parsed = searchString.substring(1, searchString.length - 1)
            return new ContainsSearch(parsed);
        } else if(searchString.substring(0, 1) == "*") {
            var parsed = searchString.substring(1, searchString.length)
            return new EndsWithSearch(parsed);
        } else if(searchString.substring(searchString.length - 1, searchString.length) == "*") {
            var parsed = searchString.substring(0, searchString.length - 1)
            return new StartsWithSearch(parsed);
        }
        return new EqualsSearch(searchString);
    }
};