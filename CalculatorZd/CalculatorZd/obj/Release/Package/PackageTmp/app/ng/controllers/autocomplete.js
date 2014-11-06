angular.module('app');

function TypeaheadCtrl($scope, $http) {
   
 // $scope.asyncSelected = [g_SELECT_VALUES];
  $scope.cargoNamesUrl = '/Calculator/GetCargoNames';
  $scope.cargoGroupNamesUrl = '/Calculator/GetCargoNames';
  $scope.getCompanySendingUrl = '/Calculator/GetCompanySending';
  $scope.getCompanyRecipientUrl = '/Calculator/getCompanyRecipient';
  $scope.getCountrySendingUrl = '/Calculator/getCountriesSending';
  $scope.getWaySendingUrl = '/Calculator/getWaysSending';
  $scope.getSubjectSendingUrl = '/Calculator/getSubjectsSending';
  $scope.getStationSendingUrl = '/Calculator/getStationsSending';
  $scope.getCountryDeliveringUrl = '/Calculator/getCountriesDelivering';
  $scope.getWayDeliveringUrl = '/Calculator/getWaysDelivering';
  $scope.getSubjectDeliveringUrl = '/Calculator/getSubjectsDelivering';
  $scope.getStationDeliveringUrl = '/Calculator/getStationsDelivering';
  $scope.getOwnerVagonUrl = '/Calculator/getOwnerVagon';
  $scope.getRenterVagonUrl = '/Calculator/getRenterVagon';
  $scope.getPayerUrl = '/Calculator/getPayer';
  $scope.getContainerVolumeUrl = '/Calculator/getContainerVolume';
  $scope.getVagonTypeUrl = '/Calculator/getVagonType';
  $scope.getCargoNameEarlyTransportationUrl = '/Calculator/getCargoNameEarlyTransportation';
  //  var addresses = [];
   // addresses.push(g_SELECT_VALUES);

    function get(val, url) {
        return $http.post(url, { term: val }).then(function (res) {
                var addresses = [];
                angular.forEach(res.data, function(value) {
                    addresses.push(value);
                });
                return addresses;
            }
        );
    };

    function getFiltered(val, url, filter) {
        return $http.post(url, { term: val, filter: filter }).then(function (res) {
           var addresses = [];
            angular.forEach(res.data, function (value) {
                addresses.push(value);
            });
            return addresses;
        }
        );
    };

    $scope.getCompanySending = function(val) {
        return get(val, $scope.getCompanySendingUrl);
    };

    $scope.getCargoNameEarlyTransportation = function (val) {
        return get(val, $scope.getCargoNameEarlyTransportationUrl);
    };

    $scope.getCompanyRecipient = function (val) {
        return get(val, $scope.getCompanyRecipientUrl);
    };

    $scope.getCargoName = function (val) {
        
      return  get(val, $scope.cargoNamesUrl);
    };
    
    $scope.getStationsSending = function (val) {
        return getFiltered(val, $scope.getStationSendingUrl, g_countrySending);
    };
    
    $scope.getStationsDelivering = function (val) {
        return getFiltered(val, $scope.getStationDeliveringUrl, g_countryDelivering);
    };

    $scope.getOwnerVagon = function (val) {
        return get(val, $scope.getOwnerVagonUrl);
    };

    $scope.getRenterVagon = function (val) {
        return get(val, $scope.getRenterVagonUrl);
    };
    $scope.getPayer = function (val) {
        return get(val, $scope.getPayerUrl);
    };

    $scope.getCountriesSending = function (val) {
        return get(val, $scope.getCountrySendingUrl);
    };
    
    $scope.getSubjectsSending = function (val) {
        return get(val, $scope.getSubjectSendingUrl);
    };
    
    $scope.getCountriesDelivering = function (val) {
        return get(val, $scope.getCountryDeliveringUrl);
    };
    $scope.getWaysSending = function (val) {
        return get(val, $scope.getWaySendingUrl);
    };

    $scope.getWaysDelivering = function (val) {
        return get(val, $scope.getWayDeliveringUrl);
    };

    $scope.getSubjectsDelivering = function (val) {
        return get(val, $scope.getSubjectDeliveringUrl);
    };
  };