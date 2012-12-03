Function.prototype.context = function (context) {
    var action = this;
    return function () {
        action.apply(context, arguments);
    };
};

var FolderCatalogItemView = CatalogItemView.extend({
	template: $("#folderItemTemplate").html(),
    expandImage: undefined,

	events: {
		'click div:first':"handleClick"
    },

	render: function () {
		CatalogItemView.prototype.render.apply(this, []);
        this.expandImage = this.$el.find("#ExpandImage:first img");
		this.model.changeToggle(this.renderExpand.context(this), this.renderCollapse.context(this));
    },

	renderExpand: function () {
        this.expandImage.attr("src", "../images/tree_expand.png");
		$(this.$el.children()[1]).show();
	},

	renderCollapse: function () {
        this.expandImage.attr("src", "../images/tree_collapsed.png");
		$(this.$el.children()[1]).hide();
	},

	handleClick: function (e) {
		this.model.changeToggle(this.renderExpand.context(this), this.renderCollapse.context(this));
		e.stopPropagation();
	}
});