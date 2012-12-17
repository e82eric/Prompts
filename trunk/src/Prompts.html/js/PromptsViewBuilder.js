function PromptsViewBuilder () {
    this.build = function (models) {
        var promptsProvider = new PromptsProvider();
        var controllers = promptsProvider.get(models);
        return new PromptsView(controllers);
    }
}