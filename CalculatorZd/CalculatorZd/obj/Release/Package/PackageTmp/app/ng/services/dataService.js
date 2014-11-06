(function () {
    'use strict';

    var app = angular.module('app');

    app.factory('dataService', ['$http', '$q', dataService]);

    function dataService($http, $q) {
        var getAllEntitiesUrl = '/api/FiltersSettingsApi/GetCoefficientsByType';
        var saveAllEntitiesUrl = '/FiltersSettings/save';
        var getAllFilterTypesUrl = '/api/FiltersSettingsApi/getFilterTypes';
        var getCalculatorResultUrl = '/api/entities/getCalculatorResult';
        var getCalculatorSearchResultUrl = '/api/entities/getCalculatorSearchResult';
        var getContainerVolumeUrl = '/calculator/getContainerVolume';
        var getColumnsSearchResultUrl = '/api/entities/getColumnsSearchResultUrl';
        var getCargoGroupNamesUrl = '/calculator/getCargoGroupNames';
        var checkExistsFilterItemUrl = '/api/entities/checkExistsFilterItem';
        var getWagonTypesUrl = '/calculator/getWagonTypes';
        var getTransTypesUrl = '/calculator/getTransportationType';
        var getSearchResultBySessionIdtUrl = '/api/entities/getSearchResultBySessionId';
        var getSessionIdUrl = '/api/entities/getSessionId';
        var downloadReportlUrl = '/api/entities/getDownloadReport';
        var loadFiltersSettingsUrl = '/api/entities/getCalcFilterList';
        var saveCalcFilterUrl = '/api/entities/getSaveFilter';
        var loadCalcFilterByIdUrl = '/api/entities/getCalcFilterSettings';
        var deleteFilterUrl = '/api/entities/getDeleteFilter';
        var service = {
            getAllEntities: getAllEntities,
            saveAllEntities: saveAllEntities,
            getAllFilterTypes: getAllFilterTypes,
            getCalculatorResult: getCalculatorResult,
            getCalculatorSearchResult: getCalculatorSearchResult,
            getColumnsSearchResult: getColumnsSearchResult,
            searchDataByFilter: searchDataByFilter, 
            getContainerVolumes:getContainerVolumes, 
            getCargoGroupNames: getCargoGroupNames,
            getWagonTypes: getWagonTypes,
            getTransTypes: getTransTypes,
            changeCurrentPage: changeCurrentPage,
            setReferencePaginationController: setReferencePaginationController,
            checkExistsFilterItem: checkExistsFilterItem,
            getSessionId: getSessionId,
            getSearchRequestBySessionId: getSearchRequestBySessionId,
            downloadReport: downloadReport,
            loadFiltersSettings: loadFiltersSettings,
            saveCalcFilter: saveCalcFilter,
            loadCalcFilterById: loadCalcFilterById,
            deleteFilter: deleteFilter,
        };
        return service;

        function deleteFilter(filterId) {
            var config = {
                params:
                {
                    filterId: filterId
                }
            };
            return get(deleteFilterUrl, config);
       }

        function loadCalcFilterById(filterId) {
            var config = {
                params:
                {
                    filterId: filterId
                }
            };
            return get(loadCalcFilterByIdUrl, config);
        }
        function saveCalcFilter(filterName, period, transportationType, wagonType, volumeType, cargoName, cargoGroup, companySending, companyRecipient,
                countrySending,
                countryDelivering,
                waySending,
                wayDelivering,
                stationSending,
                stationDelivering,
                subjectSending,
                subjectDelivering,
                ownerWagon,
                payerWagon,
                renter, columns, earlyTransportationCargo, vagonType) {
            var config = {
                params:
                {
                    filterName: filterName,
                    period: period,
                    transportationType: transportationType,
                    wagonType: wagonType,
                    volumeType: volumeType,
                    cargoName: cargoName,
                    cargoGroup: cargoGroup,
                    companySending: companySending,
                    companyRecipient: companyRecipient,
                    countrySending:countrySending,
                    countryDelivering:countryDelivering,
                    waySending:waySending,
                    wayDelivering:wayDelivering,
                    stationSending:stationSending,
                    stationDelivering:stationDelivering,
                    subjectSending:subjectSending,
                    subjectDelivering:subjectDelivering,
                    ownerWagon:ownerWagon,
                    payerWagon:payerWagon,
                    renter: renter,
                    columns: columns,
                    earlyTransportationCargo: earlyTransportationCargo,
                    vagonType:vagonType
                }
            };
            return get(saveCalcFilterUrl, config);
        }

        function loadFiltersSettings() {
            return get(loadFiltersSettingsUrl, null);
        }

        function downloadReport(sessionId)
        {
            var config = {
                params:
                {
                    sessionId: sessionId
                }
            };
            return get(downloadReportlUrl, config);
        }

        function getSessionId()
        {
            return get(getSessionIdUrl, null);
        }

        function getColumnsSearchResult() {
            return get(getColumnsSearchResultUrl, null);
        }

        function checkExistsFilterItem(filterTypeId, value) {
             var config = {
                params:
                {
                    filterTypeId: filterTypeId,
                    value: value
                }
            };
             return get(checkExistsFilterItemUrl, config);
        }

        function getCalculatorSearchResult(pageId, filtersSelectedParams, selectedColumns, sessionId, vagonSourceTypeParam) {
            var filtersSelectedObj = $.toJSON(filtersSelectedParams);
            var selectedColumnsObj = $.toJSON(selectedColumns);
            var config = {
                params:
                {
                    pageId: pageId,
                    filters: filtersSelectedObj,
                    selectedColumnsFilter: selectedColumnsObj,
                    sessionId: sessionId,
                    vagonSourceTypeParam: vagonSourceTypeParam
                }
            };
            return get(getCalculatorSearchResultUrl, config);
        }

        function getSearchResultBySessionId(sessionIdValue, pageId) {
            var config = {
                params:
                {
                    sessionId: sessionIdValue,
                    pageId: pageId
                }
            };
            return get(getSearchResultBySessionIdtUrl, config);
        }

        function getCalculatorResult(filtersSelectedParams) {
            var jsonObjFilter = $.toJSON(filtersSelectedParams);
            var config = {
                params:
                {
                    filters: jsonObjFilter}
            };
            return get(getCalculatorResultUrl, config);
        }

        function getAllEntities(typeId) {
            var config = {
                params:
                {
                    typeId: typeId
                }
            };
            return get(getAllEntitiesUrl, config);
        }

        function getAllFilterTypes() {
            return get(getAllFilterTypesUrl, null);
        }

        function saveAllEntities(typeId, entities) {
            var jsonObjFilter = $.toJSON(entities);
            var config = {
                params:
                {
                    typeId: typeId,
                    items: jsonObjFilter
                }
            };
            return save(saveAllEntitiesUrl, config);
        }

        function get(url, config) {

            var d = $q.defer();

            $http.get(url, config)
                .success(function(data) {
                    d.resolve(data);
                })
                .error(function() {
                    d.resolve(null);
                });

            return d.promise;
        }

        function save(url, config) {

            var d = $q.defer();

            $http.get(url, config)
                .success(function(data) {
                    d.resolve(data);
                })
                .error(function() {
                    d.resolve(null);
                });

            return d.promise;
        }

        var g_sessionId;
        var newDateBegin;
        var secEnd;
        var millEnd;
        function getSearchRequestBySessionId(sessionId, vm, onlyRowsCount, pageId) {

            getSearchResultBySessionId(sessionId, pageId).then(function (data) {
                if (data != null && data.items != null) {
                    showSearchResult(data, vm, onlyRowsCount);
                    clearInterval(searchFormVar);
                    var newDateEnd = new Date();
                    var millisDifference = newDateEnd - newDateBegin;
                    vm.TimeProgress = (millisDifference) / 1000 + ' сек.';//.replace('.',','); //sec + "," + mill;
                    vm.reportDisabled = false;
                    $.unblockUI();
                }
            });
        }
        var g_calculator_vm;
        var g_scope_Pagination;
        var searchFormVar;
        var secBegin;
        var millBegin;
        function searchDataByFilter(pageId, vm, onlyRowsCount) {
            newDateBegin = new Date();
            getCalculatorSearchResult(pageId, vm.selectedFilters, vm.selectedColumnsFilter, vm.sessionId, vm.vagonSourceTypeParam).then(function (data) {
              //  if (data != null) {
                //    g_sessionId = data.sessionId;
                    searchFormVar = setInterval(function () { getSearchRequestBySessionId(vm.sessionId, vm, onlyRowsCount, pageId); }, 2000);
               // }
            });
        }

        function showSearchResult(data, vm, onlyRowsCount) {
            if (data != null && data.items != null) {
                clearInterval(searchFormVar);
                $("#btnSearchReport").text("Сформировать отчет");
                $("#btnSearchReport").removeAttr('disabled');//.val("Обработка ...");
            } else {
                return false;
            }

            if (data.items.length == 0) {
                $("#btnSearchReport").text("Сформировать отчет");
                $("#btnSearchReport").removeAttr('disabled');//.val("Обработка ...");
                $('#divSearchResultEmpty').show();
                $('#divSearchResultGrid').hide();
            }
            else {
                $('#divSearchResultEmpty').hide();
                $('#divSearchResultGrid').show();
            }

            if (onlyRowsCount == true) {
                vm.totalRecords = data.totalRecords;
                vm.searchResult = [];
                vm.headers = [];
                vm.totalPages = [];
                vm.paging = [];

            } else {
                vm.searchResult = data.items;
                vm.headers = data.headers;
                vm.totalPages = data.totalPages;
                vm.totalRecords = data.totalRecords;
                g_calculator_vm = vm;
                vm.paging = [];
                var countLoops = vm.totalPages > 30 ? 30 : vm.totalPages;
                for (var i = 0; i < countLoops; i++) {
                    vm.paging.push(pageNumObj(i + 1));
                }
            }

            //vm.TimeProgress = data.TimeProgress;

           // $('#preloader').hide();
          //  $('#preloader3').hide();
        }

        function getWagonTypes() {
            return get(getWagonTypesUrl, null);
        }

        function getTransTypes() {
            return get(getTransTypesUrl, null);
        }

        function getContainerVolumes() {
            return get(getContainerVolumeUrl, null);
        }

        function getCargoGroupNames() {
            return get(getCargoGroupNamesUrl, null);
        }
        
        function pageNumObj(id) {

            return {
                id: id
            };
        }

        function changeCurrentPage(pageId) {
            var vm = g_calculator_vm;
            getCalculatorSearchResult(pageId, vm.selectedFilters, vm.selectedColumnsFilter).then(function (data) {
                vm.searchResult = data.items;
                vm.headers = data.headers;
                vm.totalPages = data.totalPages;
                vm.totalRecords = data.totalRecords;
                refreshPagination(pageId);
            });
        }

        function refreshPagination(pageId) {
            var scope = g_scope_Pagination;
            if (g_calculator_vm != null) {
                scope.maxSize = 15;
                scope.bigTotalItems = g_calculator_vm.totalPages;

                scope.bigCurrentPage = pageId;
            }
        }

        function setReferencePaginationController(scope) {
                g_scope_Pagination = scope;
        }
    }

})();