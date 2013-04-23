function PromptItemView (controller) {
    this.controller = controller;

    this.render = function () {
        this.root = $("<li class='promptItem'><div class='item selectable' onselectstart='return false;'>" + this.controller.model.Label + "</div></li>");
        this.root.click($.proxy(this.onClick,this));

        this.selectElement = this.root.find(".selectable");

        return this.root;
    }

    this.onClick = function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
    };

    this.onSelected = function () {
        this.selectElement.attr('class', 'item selectable-selected');
    };

    this.onUnSelected = function () {
        this.selectElement.attr('class', 'item selectable');
    };

    this.deleteItem = function () {
        this.root.remove();
    }
}