function HierarchyFlattener () {
    this.flattenedItems = [];

    this.addChildren = function(item) {
        if(item.Children.length != 0){
            _.each(item.Children, function (childItem){
                this.flattenedItems.push(childItem);
                this.addChildren(childItem);
            },
            this);
        }
    };

    this.Flatten = function (collection) {
        this.flattenedItems.length = 0;

        _.each(collection, function(item){
            this.flattenedItems.push(item);
            this.addChildren(item);
        },
        this);

        return this.flattenedItems;
    }
}