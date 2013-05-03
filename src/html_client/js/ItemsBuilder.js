var ItemsBuilder = Class.extend({
	init: function (itemBuilder) {
		this._itemBuilder = itemBuilder;
	},

	build: function (models, params) {
		if(params == undefined) {
			params = {};
		}

		var result = [];
		_.each(
			models,
			function (model) {
				params.model = model;
				var item = this._itemBuilder.build(params);
				result.push(item);
			},
			this);
		return result;
	}
});