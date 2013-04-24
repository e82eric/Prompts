var DropDownView = PromptView.extend({

    init: function (controller) {
        this._super(controller, "dropDownTemplate");
        this.popup = this.root.find(".popup");
        this.toggle = this.root.find(".toggle");
        this.selectedItemText = this.root.find(".selection");
        this.searchInput = this.root.find(".search-input");
        this.searchButton = this.root.find(".search-button");
        this.popup.prepend(this.controller.availableItemsController.createView().render());
    },

    render: function (){
        this.toggle.click($.proxy(this.onToggleClick, this));
        this.searchButton.click($.proxy(this.onSearch, this));
        return this.root;
    },

    onToggleClick: function (e) {
        this.controller.onToggleClick();
    },

    onDocumentClick: function (e) {
        if (this.root.has(e.target).length === 0 && this.popup.has(e.target).length === 0) {
            this.controller.onOutsideClick();
        }
    },

    open: function () {
        this.popup.show();
        $(document).click($.proxy(this.onDocumentClick, this));
    },

    close: function () {
        $(document).off('click');
        this.popup.hide();
    },

    setSelectedItemText: function (text) {
        this.selectedItemText.text(text);
    },

    onSearch: function () {
        this.controller.availableItemsController.search(this.searchInput.val());
    }
});