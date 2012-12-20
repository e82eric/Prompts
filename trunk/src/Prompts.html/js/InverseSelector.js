function InverseSelector () {
    this.select = function(items, item) {
        if(item.isSelected) {
            item.UnSelect();
        } else {
            item.Select();
        }
    }
}