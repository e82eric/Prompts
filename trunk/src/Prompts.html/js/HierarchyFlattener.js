function HierarchyFlattener () {
    this.flattenedItems = [];

    this.addChildren = function(item) {
        if(item.get("Children").length != 0){
            item.get("Children").each(function (childItem){
                this.flattenedItems.push(childItem);
                this.addChildren(childItem);
            },
            this);
        }
    };

    this.Flatten = function (collection) {
        this.flattenedItems.length = 0;

        collection.each(function(item){
            this.flattenedItems.push(item);
            this.addChildren(item);
        },
        this);

        return this.flattenedItems;
    }
}