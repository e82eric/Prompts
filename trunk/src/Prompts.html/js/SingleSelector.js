function SingleSelector () {
    this.Select = function (items, itemToSelect) {
        _.each(items, function (item){
            if(item == itemToSelect) {
                item.Select();
            } else{
                if(item.isSelected) {
                    item.UnSelect();
                }
            }
        },
        this);
    }
}