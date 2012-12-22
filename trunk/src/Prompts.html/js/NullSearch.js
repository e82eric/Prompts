function NullSearch (searchString) {
    this.searchString = searchString;

    this.execute = function (promptItem) {
        return true;
    }
}