function StartsWithSearch (searchString) {
    this.searchString = searchString;

    this.execute = function(promptItem) {
        return promptItem.model.Label.toLowerCase().indexOf(this.searchString.toLowerCase()) == 0;
    }
}