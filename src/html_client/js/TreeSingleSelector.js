var TreeSingleSelector = Class.extend({
    init: function (singleSelector, hierarchyFlatner) {
        this.singleSelector = singleSelector;
        this.hierarchyFlatner = hierarchyFlatner;
    },

    select: function (items, itemToSelect) {
        var flattenedItems = this.hierarchyFlatner.Flatten(items);
        this.singleSelector.select(flattenedItems, itemToSelect);
    }
});