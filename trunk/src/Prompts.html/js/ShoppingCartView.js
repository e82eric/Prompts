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

        this.availableItems = this.root.find("#availableItems:first");

        this.availableItems.append(this.controller.availableItemsController.createView().render());

        return this.root;
    }
}