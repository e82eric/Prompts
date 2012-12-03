var FolderCatalogItem = Backbone.Model.extend({
	toggleState: 'open',

	initialize: function () {
		if (this.get("Children").length === 0) {
			this.toggleState = 'none';
		}
	},

	changeToggle: function (openDelegate, closeDelegate) {
		if (this.toggleState === 'closed') {
			this.toggleState = 'open';
		} else if (this.toggleState === 'open') {
			this.toggleState = 'closed';
		}

        if (this.toggleState === 'closed') {
            closeDelegate();
        } else if (this.toggleState === 'open') {
            openDelegate();
        }
	},

    Select: function () {
    },

    UnSelect: function () {
    }
});