function SingleSelector () {
    this.select = function (items, itemToSelect) {
        _.each(items, function (item){
            if(item == itemToSelect) {
                item.select();
            } else{
                if(item.isSelected) {
                    item.unSelect();
                }
            }
        },
        this);
    };
}