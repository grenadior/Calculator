
(function () {
 
    var app =
        angular.module('app');
app.controller('paginationCtrl', ['$scope', 'dataService', paginationCtrl]);

function paginationCtrl($scope, dataService) {
        $scope.totalItems = 64;
        $scope.currentPage = 1;
        $scope.bigCurrentPage = 1;
        $scope.maxSize = 10;
        $scope.numPages = 15;
        $scope.setPage = function() {
            dataService.changeCurrentPage($scope.bigCurrentPage);
            dataService.setReferencePaginationController($scope);
        };

        $scope.setReference = setReference;

        $scope.bigTotalItems = 135;
        
        setReference($scope);

        function setReference(scope) {
            dataService.setReferencePaginationController(scope);
        }
    // $scope.currentPage = dataService.getCurrentPageNum();
};
})();