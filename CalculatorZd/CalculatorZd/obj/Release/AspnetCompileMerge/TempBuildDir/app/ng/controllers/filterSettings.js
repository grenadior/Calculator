
(function () {
    
    var app = angular.module('app');

    app.controller('filterSettings', ['$rootScope', 'dataService', filterSettings]);

    function filterSettings($rootScope, dataService) {
        var vm = this;
        vm.filterTypesChanged = filterTypesChanged;
        vm.filterTypes = [];
        vm.filterType = [];
        vm.id = null;
        vm.coefficients = [new coefficient(1, 0)];
        vm.addFilterCoeffs = addFilterCoeffs;
        vm.deleteFilterCoeffs = deleteFilterCoeffs;
        vm.saveCoefficientsByType = saveCoefficientsByType;
       
        function addFilterCoeffs() {

            var id = vm.coefficients.length + 1;
            if (vm.coefficients == null) {
                vm.coefficients = [new coefficient(id, 0)];
            } else {
                vm.coefficients.push(new coefficient(id, 0));
            } //else {
            //    return true;
            //}
            return true;
        }
    
        function coefficient(id, value) {
            return {
                id: id,
                value: value
            };
        }

        function deleteFilterCoeffs(f) {

            for (var i = 0; i != vm.coefficients.length; i++) {
                var current = vm.coefficients[i];

                if (current === f) {
                    vm.coefficients.splice(i, 1);
                    break;
                }
            }

            for (var j = 0; j != vm.coefficients.length; j++) {
                current = vm.coefficients[j];

                current.id = j + 1;
            }
        }
        
        getAllFilterTypes();
        
        function getAllFilterTypes() {
           
            dataService.getAllFilterTypes().then(function (data) {
                vm.filterTypes = data;
                vm.filterType = data[0];
                filterTypesChanged();
            });
        }

        function filterTypesChanged() {
            dataService.getAllEntities(vm.filterType.id).then(function (data) {
                vm.coefficients = data;
            });
        }
        
        function saveCoefficientsByType() {
            
            dataService.saveAllEntities(parseInt(vm.filterType.id), vm.coefficients).then(function (data) {
                vm.coefficients = data;
            });
        }
    }
})();