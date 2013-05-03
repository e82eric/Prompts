var TreeDropDownBuilder = DropDownBuilder.extend({
    build: function (model) {
        var singleSelector = new SingleSelector();
        var dropDownSelector = new TreeDropDownSelector(singleSelector, new HierarchyFlattener());

        var itemBuilder = new TreePromptItemControllerBuilder(model.Name, undefined, []);

        return this._build(
            model,
            dropDownSelector, 
            itemBuilder,
            function (controller) {
                return new DropDownView(controller, "treeDropDownTemplate");
            },
            { promptLevelInfo: model.PromptLevelInfo }
        );
    }
});