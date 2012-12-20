function RangeSelector () {
    this.select = function (items, firstItem, secondItem) {
        var inRange = false;

        var startItem = undefined;
        var endItem = undefined;

        if(_.indexOf(items, firstItem) < _.indexOf(items, secondItem)) {
            startItem = firstItem;
            endItem = secondItem;
        } else {
            startItem = secondItem;
            endItem = firstItem;
        }

        _.each(
            items,
            function (item) {
                if(item == startItem) {
                    inRange = true;
                }

                if(inRange) {
                    item.Select();
                } else {
                    if(item.isSelected){
                        item.UnSelect();
                    }
                }

                if(item == endItem) {
                    inRange = false;
                }
            },
            this
        )
    }
}