define(['services/dataService', 'knockout'],
    function (dataService, ko) {
        var vm = {
            itemsPerPage: ko.observable(),
            entities: ko.observableArray(),
            loadPage: loadPage,
            activate: activate
        };
        
        return vm;

        function activate() {
            vm.itemsPerPage(100);
            loadPage(1);
        }

        function loadPage(page) {

            dataService.getAllEntities(page, vm.itemsPerPage(), vm.entities).then(function() {
                
            });
        }
    });