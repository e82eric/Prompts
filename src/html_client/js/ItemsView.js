var ItemsView = Class.extend({
    init: function(controller, listClass){
        this.controller = controller;
        this.listClass = listClass;
        this.root = $("<ul></ul>");
    },

    render: function () {

		var itemsToRender = undefined;

		if (this.controller.displayItems != undefined) {
			itemsToRender = this.controller.displayItems;
		} else {
			itemsToRender = this.controller.items;
		}

		this.root.empty();
		_.each( 
			itemsToRender, 
			function (item) {
				var view = undefined;

				if (item.view != undefined) {
					view = item.view;
				} else {
					view = item.createView();
				}
				
				this.root.append(view.render());
			},
			this
		);
			
        return this.root;
    },

    renderItems: function (controllers) {
		this.itemViews = _.map(controllers, function (controller) { return controller.createView() }); 
        this.render();
    },

	addItems: function (controllers) {
		_.each(controllers, function (controller) {
			var itemView = controller.createView();
			this.root.append(itemView.render());
		},
		this);
	}
});
