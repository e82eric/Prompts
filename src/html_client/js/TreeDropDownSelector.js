var TreeDropDownSelector = TreeSingleSelector.extend({
    init: function (singleSelector, hierarchyFlattener) {
        this._super(singleSelector, hierarchyFlattener);
    },

    select: function (shiftKeyPressed, controlKeyPressed, items, item) {
        this._super(shiftKeyPressed, controlKeyPressed, items, item);
        this.promptController.onSelection(item);
    },

    setPromptController: function (val) {
        this.promptController = val;
    }
});