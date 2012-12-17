module( "Loading Panel Tests", {
    setup: function () {
        this.view = {};
        this.view.showLoading = sinon.spy();
        this.view.hideRetry = sinon.spy();
        this.view.hideError = sinon.spy();
        this.view.setErrorMessage = sinon.spy();
        this.view.showLoaded = sinon.spy();
        this.view.hideLoading = sinon.spy();
        this.view.showError = sinon.spy();
        this.view.showRetry = sinon.spy();
        this.view.hideLoaded = sinon.spy();

        this.catalog = {};
        this.catalog.Get = sinon.spy();

        this.controller = new LoadingPanelController(this.catalog);
        this.controller.setView(this.view);
    }
});

test( "It calls hide loading when constructed", function() {
    ok( this.view.hideLoading.calledOnce );
});

test( "It calls hide retry when constructed", function() {
    ok( this.view.hideRetry.calledOnce );
});

test( "It calls show loading on the view when load is called", function() {
    ok( !(this.view.showLoading.called) );

    this.controller.load({});

    ok ( this.view.showLoading.calledOnce );
});

test( "It calls hide loaded on the view when load is called", function() {
    ok( !(this.view.hideLoaded.called) );

    this.controller.load({});

    ok ( this.view.hideLoaded.calledOnce );
});

test( "It calls hide error on the view when load is called", function() {
    ok( !(this.view.hideError.called) );

    this.controller.load({});

    ok ( this.view.hideError.calledOnce );
});

test( "It sets the error message to blank on the view when load is called", function() {
    ok( !(this.view.setErrorMessage.called) );

    this.controller.load({});

    ok ( this.view.setErrorMessage.withArgs("").calledOnce );
});

test( "It calls hide retry on the view when load is called", function() {
    ok( this.view.hideRetry.calledOnce );

    this.controller.load({});

    ok ( this.view.hideRetry.calledTwice );
});

test( "It calls hide loading on the view when the repository callback with success", function() {
    var localRequest = { data: "data 1" };
    var localSuccessCallback = undefined;

    this.catalog.Get = function (request, successCallback, errorCallback){
        if(localRequest == request){
            localSuccessCallback = successCallback;
        }
    };

    this.controller.load(localRequest);

    ok( this.view.hideLoading.calledOnce );

    localSuccessCallback(0);

    ok ( this.view.hideLoading.calledTwice );
});

test( "It calls show loaded on the view when the repository callback with success", function() {
    var localRequest = { data: "data 1" };
    var viewToCallbackWith = {};

    var localSuccessCallback = undefined;

    this.catalog.Get = function (request, successCallback, errorCallback){
        if(request == localRequest)  {
            localSuccessCallback = successCallback;
        }
    };

    this.controller.load(localRequest);

    ok( !(this.view.showLoaded.called) );

    localSuccessCallback(viewToCallbackWith);

    ok ( this.view.showLoaded.withArgs(viewToCallbackWith).calledOnce );
});

test( "It calls hide loading on the view when the repository callback with error", function() {
    var localRequest = { data: "data 1" };
    var localErrorCallback = undefined;

    this.catalog.Get = function (request, successCallback, errorCallback){
        if(request == localRequest){
            localErrorCallback = errorCallback;
        }
    };

    this.controller.load(localRequest);

    ok( this.view.hideLoading.calledOnce );

    localErrorCallback("Error Message");

    ok ( this.view.hideLoading.calledTwice );
});

test( "It calls show error on the view when the repository callback with error", function() {
    var localRequest = { data: "data 1" };
    var localErrorCallback = undefined;

    this.catalog.Get = function (request, successCallback, errorCallback){
        if(request == localRequest){
            localErrorCallback = errorCallback;
        }
    };

    this.controller.load(localRequest);

    ok( !(this.view.showError.called) );

    localErrorCallback("error message");

    ok ( this.view.showError.calledOnce );
});

test( "It calls show retry on the view when the repository callback with error", function() {
    var localRequest = { data: "data 1" };
    var localErrorCallback = undefined;

    this.catalog.Get = function (request, successCallback, errorCallback){
        if(request == localRequest) {
            localErrorCallback = errorCallback;
        }
    };

    this.controller.load(localRequest);

    ok( !(this.view.showRetry.called) );

    localErrorCallback("error message");

    ok ( this.view.showRetry.calledOnce );
});

test( "It calls set error message on the view when the repository callback with error", function() {
    var localRequest = { data: "data 1" };
    var errorMessage = "error message 1";

    var localErrorCallback = undefined;

    this.catalog.Get = function (request, successCallback, errorCallback){
        if(request == localRequest){
            localErrorCallback = errorCallback;
        }
    };

    this.controller.load(localRequest);

    ok( this.view.setErrorMessage.calledOnce );

    localErrorCallback(errorMessage);

    ok( this.view.setErrorMessage.calledTwice );
    ok ( this.view.setErrorMessage.withArgs(errorMessage).calledOnce );
});