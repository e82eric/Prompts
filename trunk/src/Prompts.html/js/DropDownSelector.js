function DropDownSelector (singleSelector) {
    this.singleSelector = singleSelector;
    this.select = function (shiftKeyPressed, controlKeyPressed, items, item) {
        this.singleSelector.select(items, item);
        this.promptController.onSelection(item);
    };

    this.setPromptController = function (val) {
        this.promptController = val;
    }
}