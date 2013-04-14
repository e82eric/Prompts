var FolderCatalogItemController = ExpandableItemController.extend({
    init: function(model, childItemsController){
        this.model = model;
        this.childItemsController = childItemsController;
        this.isSelected = false;
    },

    select: function () {},

    unSelect: function () {},

    createView: function(){
        var childItemsView = this.childItemsController.createView();
        this.setView(new FolderCatalogItemView(this, childItemsView));
        return this.view;
    }
});