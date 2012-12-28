function TreeShoppingCartSelector (multiSelector, hierarchyFlattener) {
    this.multiSelector = multiSelector;
    this.hierarchyFlattener = hierarchyFlattener;
    this.select = function (shiftKeyPressed, controlKeyPressed, items, item) {
        var flattenedItems = this.hierarchyFlattener.Flatten(items);
        return this.multiSelector.select(shiftKeyPressed, controlKeyPressed, flattenedItems, item);
    };

    this.setPromptController = function (val) {
        this.promptController = val;
    }
}