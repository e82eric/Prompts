function DropDownView (controller){
    this.controller = controller;
    this.root = $("<li></li>");
    var template = $("#dropDownTemplate").html();
    var templateFunction  = _.template(template);
    var templateHtml = templateFunction(this.controller.model);
    this.root.html(templateHtml);
    this.availableItemsUl = this.root.find("#availableItems");
    this.popup = this.root.find("#popup");
    this.toggle = this.root.find("#toggle");
    this.selectedItemText = this.root.find("#selectedItemText");
    this.searchInput = this.root.find("#searchString");
    this.searchButton = this.root.find("#searchButton");
    this.popup.prepend(this.controller.availableItemsController.createView().render());

    this.render = function (){
        this.toggle.click($.proxy(this.onToggleClick, this));
        this.searchButton.click($.proxy(this.onSearch, this));
        return this.root;
    };

    this.onToggleClick = function (e) {
        this.controller.onToggleClick();
    };

    this.onDocumentClick = function (e) {
        if (this.root.has(e.target).length === 0 && this.popup.has(e.target).length === 0) {
            this.controller.onOutsideClick();
        }
    };

    this.open = function () {
        this.popup.show();
        $(document).click($.proxy(this.onDocumentClick, this));
    };

    this.close = function () {
        $(document).off('click');
        this.popup.hide();
    };

    this.setSelectedItemText = function (text) {
        this.selectedItemText.text(text);
    }

    this.onSearch = function () {
        this.controller.availableItemsController.search(this.searchInput.val());
    }
}