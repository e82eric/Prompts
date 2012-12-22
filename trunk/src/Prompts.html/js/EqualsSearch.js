function EqualsSearch (searchString) {
    this.searchString = searchString;
    this.execute = function(promptItem){
        return promptItem.model.Label.toLowerCase() == this.searchString.toLowerCase();
    }
}