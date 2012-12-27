function TreeDropDownSelector (singleSelector, hierarchyFlattener) {
    this.singleSelector = singleSelector;
    this.hierarchyFlattener = hierarchyFlattener;
    this.select = function (shiftKeyPressed, controlKeyPressed, items, item) {
        var flattenedItems = this.hierarchyFlattener.Flatten(items);

        this.singleSelector.select(flattenedItems, item);
        this.promptController.onSelection(item);
    };

    this.setPromptController = function (val) {
        this.promptController = val;
    }
}