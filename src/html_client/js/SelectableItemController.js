var SelectableItemController = Class.extend({
    init: function () {
        this.isSelected = false;
    },

    select: function () {
        this.view.onSelected();
        this.isSelected = true;
    },

    unSelect: function () {
        this.view.onUnSelected();
        this.isSelected = false;
    },

    setView: function (val) {
        this.view = val;
        return this.view;
    }
});