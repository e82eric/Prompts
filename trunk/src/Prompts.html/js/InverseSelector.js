function InverseSelector () {
    this.select = function(items, item) {
        if(item.isSelected) {
            item.unSelect();
        } else {
            item.select();
        }
    }
}