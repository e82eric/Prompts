var ExecuteReportController = LoadingPanelController.extend({
    init: function (reportRenderer) {
        this._reportRenderer = reportRenderer;
    },

    setPromptingController: function (val) {
        this._promptingController = val;
    },

    setRepository: function (val) {
        this._excutionIdRepository = val;
    },

    execute: function () {
        var self = this;

        this._excutionIdRepository.get(
            this._promptingController.getExecuteRequest(),
            function(result) {
                self._reportRenderer.execute(result);
            }
        );
    },

    setCanExecute: function (val){
        this.canExecute = val;
        this._evaluateEnable();
    },

    _setIsLoading: function (val) {
        this.isLoading = val;
        this._evaluateEnable();
    },

    _evaluateEnable: function () {
        if(this.canExecute == false || this.isLoading == true) {
            this.view.disable();
            return;
        }

        this.view.enable();
    },

    showLoading: function() {
        this._super();
        this._setIsLoading(true);
    },

    showLoaded: function () {
        this._super();
        this._setIsLoading(false);
    },

    showError: function(errorMessage) {
        this._super();
        this._setIsLoading(false);
    },

    createView: function () {
        this.setView(new ExecuteReportView(this));
        return this.view;
    }
});
