function SelectedItemsController (selector, promptItemsControllersProvider) {
    this.selector = selector;
    this.promptItemControllersProvider = promptItemsControllersProvider;
    this.items = [];

    this.addItems = function (models) {
        var itemsToAdd = [];

        _.each(
            models,
            function (model) {
                var existingItem = _.find(this.items, function (thisItem){
                    return thisItem.model == model;
                },
                this);
                if(existingItem == undefined) {
                    itemsToAdd.push(model);
                }
            },
            this
        );

        var controllers = this.promptItemControllersProvider.get(itemsToAdd);

        _.each(
            controllers,
            function(controller) {
                this.items.push(controller);
            },
            this
        );

        this.view.addItems(controllers);
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