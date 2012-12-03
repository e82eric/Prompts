var CatalogItemView = Backbone.View.extend({
	tagName: "li",
	template: $("#itemTemplate").html(),

	events:{
        "mouseover #hoverWrap": "onMouseOver",
        "mouseleave #hoverWrap": "onMouseLeave",
        "click #hoverWrap": "onClick"
    },

	initialize: function () {
		this.render();
        if (this.model instanceof ReportCatalogItem){
            this.model.setOnSelected(this.onSelected.context(this));
            this.model.setOnUnSelected(this.onUnSelected.context(this));
        }
	},

	render: function () {
		var templ, childCatalogItems;
		templ = _.template(this.template);
		this.$el.html(templ({catalogItem: this.model.toJSON()}));
		childCatalogItems = this.model.get("Children");
		this.$el.attr("class", "ReportView");

        if (!(childCatalogItems instanceof Array)) {
            var childCatalogView = new CatalogView({ collection:childCatalogItems, css:"childItems" });
            this.$el.append(childCatalogView.el);
        }
	},

	onMouseOver: function(){
		this.$el.find("#hoverWrap:first").attr('class', 'hoverGlow');
	},

	onMouseLeave: function(){
		this.$el.find("#hoverWrap:first").attr('class', 'catalogItem');
	},

	onClick: function(){
		this.model.changeSelect(this);
	},

    onSelected: function(){
        this.$el.find("#selectWrap:first").attr('class', 'selectedGlow');
    },

    onUnSelected: function(){
        this.$el.find("#selectWrap:first").attr('class', 'catalogItem');
    }
});