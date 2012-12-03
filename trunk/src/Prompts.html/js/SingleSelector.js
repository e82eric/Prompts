function SingleSelector () {
    this.Select = function (items, itemToSelect) {
        _.each(items, function (item){
            if(item == itemToSelect) {
                item.Select();
            } else{
                item.UnSelect();
            }
        },
        this);
    }
}