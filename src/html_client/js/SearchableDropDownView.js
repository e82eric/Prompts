var SearchableDropDownView = DropDownView.extend({
    init: function (controller) {
        this._super(controller, "dropDownTemplate");
        this.searchInput = this.root.find(".search-input");
        this.searchButton = this.root.find(".search-button");
    },

    render: function (){
        this.searchButton.click($.proxy(this.onSearch, this));
        return this._super();
    },

    onSearch: function () {
        this.controller.availableItemsController.search(this.searchInput.val());
    }
});