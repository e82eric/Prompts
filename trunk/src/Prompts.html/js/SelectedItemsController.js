function SelectedItemsController (selector) {
    this.selector = selector;
    this.items = [];

    this.addItems = function (items) {
        var itemsToAdd = [];

        _.each(
            items,
            function (item) {
                var existingItem = _.find(this.items, function (thisItem){
                    return thisItem.model == item.model;
                },
                this);
                if(existingItem == undefined) {
                    itemsToAdd.push(item);
                    this.items.push(item);
                }
            },
            this
        );

        this.view.addItems(itemsToAdd);
    };

    this.select = function (shiftKeyPressed, controlKeyPressed, item) {
        this.selector.select(shiftKeyPressed, controlKeyPressed, this.items, item);
    };

    this.createView = function () {
        var view = new SelectedItemsView(this);
        this.setView(view);
        return view;
    };

    this.setView = function (val) {
        this.view = val;
    };

    this.removeSelected = function() {
        var itemsToRemove = [];

        _.each(
            this.items,
            function(item) {
                if(item.isSelected) {
                    itemsToRemove.push(item);
                }
            },
            this);

        _.each(
            itemsToRemove,
            function(item) {
                var indexOfItem = _.indexOf(this.items, item);
                this.items.splice(indexOfItem, 1);
                item.deleteItem();
            },
            this);
    };
}