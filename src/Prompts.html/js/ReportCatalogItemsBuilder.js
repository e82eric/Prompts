var ReportCatalogItemsBuilder = Class.extend({
    init: function (itemBuilder) {
	    this.itemBuilder = itemBuilder;
    },
	build: function(models) {
		var result = [];
		
		_.each(
            models,
            function (model) {
			var controller = this.itemBuilder.build(model);
			result.push(controller);
		},
		this);

		return result;
	}
});