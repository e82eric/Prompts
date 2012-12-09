var EmptyFolderCatalogItemController = function(model){
    this.model = model;
    this.Select = function () {
    };

    this.UnSelect = function () {
    };

    this.CreateView = function(){
            return new EmptyFolderCatalogItemView(this);
    };
};
