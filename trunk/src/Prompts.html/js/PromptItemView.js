function PromptItemView (controller) {
    this.controller = controller;

    this.render = function () {
        this.root = $("<li class='itemTest' unselectable='on'><div unselectable='on' id='selectWrap'>" + this.controller.model.Label + "</div></li>");
        this.root.click($.proxy(this.onClick,this));

        this.selectWrap = this.root.find("#selectWrap");
        this.selectWrap.addClass('itemTestNotSelected');

        return this.root;
    }


    this.onClick = function (e) {
        this.controller.clicked(e.shiftKey, e.ctrlKey);
    };

    this.onSelected = function () {
        this.selectWrap.addClass('itemTestSelect');
        this.selectWrap.removeClass('itemTestNotSelected');
    };

    this.onUnSelected = function () {
        this.selectWrap.removeClass('itemTestSelect');
        this.selectWrap.addClass('itemTestNotSelected');
    };

    this.deleteItem = function () {
        this.root.remove();
    }
}