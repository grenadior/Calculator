requirejs.config({
    paths: {
        'durandal': '../../scripts/durandal',
        'plugins': '../../scripts/durandal/plugins',
        'transitions': '../../scripts/durandal/transitions',
        'jquery': '../../scripts/vendor/jquery',
        'knockout': '../../scripts/vendor/knockout-3.0.0',
        'text': '../../scripts/vendor/text'
    }
});

define(['durandal/system', 'durandal/app', 'durandal/viewLocator'], function (system, app, viewLocator) {
    app.start().then(function () {
        viewLocator.useConvention();
        app.setRoot('viewmodels/entityList');
    });
})