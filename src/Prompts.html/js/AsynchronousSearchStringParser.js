var AsynchronousSearchStringParser = Class.extend({
    init: function(searchStringParser, promptName, filterParameterName) {
        this.searchStringParser = searchStringParser;
        this.promptName = promptName;
        this.filterParameterName = filterParameterName;
    },

    parse: function (searchString) {
        var search = this.searchStringParser.parse(searchString);
        search.childItemsRequest = {
            PromptName: this.promptName,
            ParameterName: this.filterParameterName,
            ParameterValues: [{Name: this.filterParameterName, Value: search.searchString}]
        };

        return search;
    }
});