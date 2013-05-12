var ItemsDisposer = Class.extend({
	dispose: function (items) {
		 _.each(
		    items,
		    function (item) {
			item.deleteItem();
		    },
		    this
		);
	}
});
