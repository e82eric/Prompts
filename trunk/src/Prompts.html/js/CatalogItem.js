var CatalogItem = Backbone.Model.extend({
    onSelected: undefined,
    onUnSelected: undefined,
    selected: false,
    reportCatalog: undefined,

    setReportCatalog: function(val) {
        this.reportCatalog = val;
    },

    setOnSelected: function (onSelected) {
        this.onSelected = onSelected;
    },

    setOnUnSelected: function (onUnSelected) {
        this.onUnSelected = onUnSelected;
    },

    changeSelect: function() {
        this.reportCatalog.Select(this);
    },

    Select: function () {
        this.onSelected();
    },

    UnSelect: function () {
        this.setOnUnSelected();
    }
});