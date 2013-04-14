function EndsWithSearch (searchString) {
    this.searchString = searchString;

    this.execute = function (promptItem) {
        return promptItem.model.Label.toLowerCase().match(this.searchString.toLowerCase() + "$") == this.searchString.toLowerCase();
    }
}