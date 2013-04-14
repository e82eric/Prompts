var EmptyFolderCatalogItemController = SelectableItemController.extend({
    init: function(model){
        this.model = model;
        this.Children = [];
    },

    createView: function(){
        return new EmptyFolderCatalogItemView(this);
    },

    select: function () {},

    unSelect: function () {}
});
