var DropDownView = PromptView.extend({
    init: function (controller, templateName) {
        this._super(controller, templateName)

		this.itemsView = this.controller.availableItemsController.createView(function (controller) { return new ItemsView(controller, "rootItems");});

        var template = _.template($("#smallDropDownTemplate").text());
        var templateHtml = template(this.controller);
        this.smallRoot.append($(templateHtml));
		this.smallPopup = this.smallRoot.find(".popup");
		this.smallPopup.prepend(this.itemsView.render());

        this.availableItemsUl = this.root.find(".available-items");
        this.popup = this.root.find(".popup");
        this.toggle = this.root.find(".toggle");

        this.selectedItemText = this.root.find(".selection");
        this.smallSelectedItemText = this.smallRoot.find(".selection");
    },

    render: function () {
		this.popup.prepend(this.itemsView.render());
        this.toggle.click($.proxy(this.onToggleClick, this));
        return this.root;
    },

	renderSmall: function () {
		this.smallPopup.prepend(this.itemsView.render());
        this.smallRoot.find(".toggle").click($.proxy(this.onToggleClick, this));
		return this._super();
	},

    onToggleClick: function (e) {
        this.controller.onToggleClick();
    },

	showSmall: function () {
		this.smallPopup.prepend(this.itemsView.render());
		this._super();
	},

    onDocumentClick: function (e) {
        if (this.smallRoot.has(e.target).length === 0 && this.smallPopup.has(e.target).length === 0 && this.root.has(e.target).length === 0 && this.popup.has(e.target).length === 0) {
            this.controller.onOutsideClick();
        }
    },

    open: function () {
        this.popup.show();
        this.smallPopup.show();
        $(document).click($.proxy(this.onDocumentClick, this));
    },

    close: function () {
        $(document).off('click');
        this.popup.hide();
        this.smallPopup.hide();
    },

    setSelectedItemText: function (text) {
  		this.smallSelectedItemText.text(text);
  		this.selectedItemText.text(text);
    }
});
