function ShoppingCartView (controller){
    this.controller = controller;

    this.render = function (){
        this.root = $("<li></li>");

        var template = $("#shoppingCartTemplate").html();

        var templateFunction  = _.template(template);
        var templateHtml = templateFunction(this.controller.model);
        this.root.html(templateHtml);

        this.selectElement = this.root.find("#selectButton:first");
        this.selectElement.click($.proxy(this.onSelect,this));

        this.availableItems = this.root.find("#availableItems");
        this.selectedItems = this.root.find("#selectedItems:first");
        this.selectButton = this.root.find("#selectButton:first");
        this.selectButton.click($.proxy(this.onSelectButtonClick, this));
        this.unSelectButton = this.root.find("#unSelectButton:first");
        this.unSelectButton.click($.proxy(this.onUnSelectButtonClick, this));
        this.searchStringInput = this.root.find("#searchString:first");
        this.searchButton = this.root.find("#searchButton:first");
        this.searchButton.click($.proxy(this.onSearch, this));


        this.availableItems.append(this.controller.availableItemsController.createView().render());
        this.selectedItems.append(this.controller.selectedItemsController.createView().render());

        return this.root;
    };

    this.onSelectButtonClick = function () {
        this.controller.onSelect();
    };

    this.onUnSelectButtonClick = function () {
        this.controller.onUnSelect();
    };

    this.onSearch = function () {
        this.controller.availableItemsController.search(this.searchStringInput.val());
    }
}