var EmptyFolderCatalogItemController = SelectableItemController.extend({
    init: function(model){
        this.model = model;
        this.Children = [];
    },

    createView: function(){
        return new TemplateView(this, "emptyFolderItemTemplate");
    },

    select: function () {},

    unSelect: function () {}
});
