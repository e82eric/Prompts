var DropDownView = PromptView.extend({

    init: function (controller, templateName) {
        this._super(controller, templateName)

        this.availableItemsUl = this.root.find(".available-items");
        this.popup = this.root.find(".popup");
        this.toggle = this.root.find(".toggle");
        this.selectedItemText = this.root.find(".selection");
        this.popup.prepend(this.controller.availableItemsController.createView(
            function (controller) { return new ItemsView(controller, "rootItems"); } ).render());
    },

    render: function (){
        this.toggle.click($.proxy(this.onToggleClick, this));
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
    }
});