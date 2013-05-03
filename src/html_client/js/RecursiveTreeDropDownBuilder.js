var RecursiveTreeDropDownBuilder = DropDownBuilder.extend({
    build: function (model, promptsController) {
        var singleSelector = new SingleSelector();
        var dropDownSelector = new TreeDropDownSelector(singleSelector, new HierarchyFlattener());

        var filterParameterName = model.PromptLevelInfo.ParameterName;

        var itemBuilder = new RecursiveTreePromptItemControllerBuilder(
            model.Name,
            undefined,
            filterParameterName);

        return this._build(
            model,
            dropDownSelector, 
            itemBuilder,
            function (controller) {
                return new DropDownView(controller, "treeDropDownTemplate");
            }
        );
    }
});