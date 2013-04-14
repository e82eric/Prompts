var ExpandableItemController = SelectableItemController.extend({
    init: function () {
        this.Children = [];
    },

    setView: function (val) {
        this.view = val;
        this.collapse();
    },

    expanderClicked: function () {
        if (this.toggleState === 'closed') {
            this.expand();
        } else if (this.toggleState === 'open') {
            this.collapse();
        }
    },

    collapse: function () {
        this.view.renderCollapse();
        this.toggleState = 'closed';
    },

    expand: function () {
        this.view.renderExpand();
        this.toggleState = 'open';
    },

    setChildren: function(val) {
        this.Children = val;
    }
});