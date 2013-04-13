function Search (search) {
    this.search = search;

    this.execute = function (promptItems) {
        var result = [];

        _.each(
            promptItems,
            function(item) {
                if(this.search.execute(item)){
                    result.push(item);
                }
            },
            this
        );

        return result;
    }
}