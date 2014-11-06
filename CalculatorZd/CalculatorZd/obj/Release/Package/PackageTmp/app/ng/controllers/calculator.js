g_countrySending = null;
var g_countryDelivering = null;
var g_SELECT_VALUES = 'Выберите значение';
var g_ALL = 'ВСЕ';


(function () {
   
    var app = angular.module('app');
    app.controller('binding', ['$rootScope', 'dataService', binding]);

    
    function blockPageUI(message) {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#сссссс',
                '-webkit-border-radius': '5px',
                '-moz-border-radius': '5px',
               
                color: '#fff'
            }, message: message
        });
    }

    function binding($scope, dataService) {
        
        var vm = this;
       
        vm.rbListCargoSourceType =
     [
         "груженый рейс",
         "порожний рейс"
     ];

        vm.rbListStationDelivering =
       [
           "выключено", "поиск по 2-м цифрам",
           "поиск по 3-м цифрам"
       ];

        vm.vagonSourceTypeParam = ["груженый рейс"];
        vm.vagonSourceTypeParamArr = null;
        vm.rbVagonSourceTypeClick = rbVagonSourceTypeClick;
        vm.isGruzhTypeSelected = isGruzhTypeSelected;
        vm.stationDeliveringSearchParam = ["выключено"];
        vm.stationDeliveringSearchParamArr = null;
        vm.rbListStationDeliveringClick = rbListStationDeliveringClick;
        vm.reportDisabled = true;
        vm.TimeProgress = null;
        vm.clientGeneratedTimeBegin = null;
        vm.clientGeneratedTimeEnd = null;
        vm.addColumns = addColumns;
        vm.deleteColumns = deleteColumns;
        vm.columns = [];
        vm.column = [];
        vm.selectedColumns = [];
        vm.selectedColumn = [];
        vm.selectedColumnsFilter = [];
        vm.selectedDopInfo = [];
    
        vm.dateBegin = null;
        vm.dateEnd = null;
        vm.selectedFilters = [];
        vm.calculatorResult = 0;

        vm.transportationPeriod = [];
        vm.transportationTypes = [];
        vm.addTransportationType = addTransportationType;
        vm.deleteItem = deleteItem;

        vm.loadPage = loadPage;

        vm.wagonTypes = [];
        vm.wagonType = [];
        vm.selectedWagonTypes = [];
        vm.addWagonType = addWagonType;
      
      //  vm.containerVolumes = [];
       // vm.selectedContainerVolumes = [];
       // vm.containerVolume = [];
      //  vm.addContainerVolume = addContainerVolume;

        vm.cargoNames = [];
        vm.addCargoName = addCargoName;
       
        vm.cargoGroupNames = [];
        vm.selectedCargoGroupNames = [];
        vm.addCargoGroupName = addCargoGroupName;
        vm.cargoGroupName = [];

        vm.okpos = [];
        vm.addOkpo = addOkpo;

        vm.countriesDelivering = [];
        vm.countryDelivering = [];
        vm.addCountryDelivering = addCountryDelivering;
        vm.selectedCountryDelivering = [];

        vm.countriesSending = [];
        vm.addCountrySending = addCountrySending;
        vm.countrySending = [];
        vm.selectedCountrySending = [];

        vm.waysSending = [];
        vm.addWaySending = addWaySending;
        vm.waySending = [];
        vm.selectedWaysSending = [];

        vm.waysDelivering = [];
        vm.wayDelivering = [];
        vm.addWayDelivering = addWayDelivering;
        vm.selectedWaysDelivering = [];

        vm.subjectsSending = [];
        vm.subjectSending = [];
        vm.addSubjectSending = addSubjectSending;
        vm.selectedSubjectsSending = [];

        vm.subjectsDelivering = [];
        vm.addSubjectDelivering = addSubjectDelivering;
        vm.selectedSubjectsDelivering = [];
        vm.subjectDelivering = [];

        vm.stationsSending = [];
        vm.addStationSending = addStationSending;
       
        vm.stationsDelivering = [];
        vm.addStationDelivering = addStationDelivering;

        vm.ownersVagon = [];
        vm.addOwnerVagon = addOwnerVagon;

        vm.rentersVagon = [];
        vm.addRenterVagon = addRenterVagon;

        vm.payers = [];
        vm.addPayer = addPayer;

        vm.ownersVagon = [];
        vm.addOwnerVagon = addOwnerVagon;

        vm.companiesSending = [];
        vm.addCompanySending = addCompanySending;
        vm.companySending = null;

        vm.companiesRecipient = [];
        vm.addCompanyRecipient = addCompanyRecipient;

        vm.calculateForm = calculateForm;
        vm.searchForm = searchForm;

        vm.downloadReport = downloadReport;

        vm.searchResult = [];
        vm.headers = [];
        vm.totalPages = 0;
        vm.paging = [];
        vm.selectedValues = [];
  
        vm.showRowsCount = showRowsCount;
        vm.calcCoeffDetailReport = null;
        var maxLengthFilters = 10;

        vm.allcolumns = [];

        vm.allFiltersSettingsList = [];
        vm.firmFilterName = null;
        vm.createFilter = createFilter;
        vm.confirmFilter = confirmFilter;
        vm.deleteFilter = deleteFilter;
        vm.addVagonNumber = addVagonNumber;
        vm.vagonNumber = null;
        vm.vagonNumbers = [];
        
        function isGruzhTypeSelected() {
            return (vm.vagonSourceTypeParam[0] == ["груженый рейс"] || vm.vagonSourceTypeParam == ["груженый рейс"]);
        }

        function deleteFilter(filterId) {
            if (confirm('Удалить фильтр?')) {
                dataService.deleteFilter(filterId).then(function (data) {
                    if(data!=null)
                       loadFilterSettings();
                });
            }
        }

        function confirmFilter(filterId) {
          
            dataService.loadCalcFilterById(filterId).then(function (data) {
                if (data != null) {
                    vm.selectedTransportationTypes = [];
                    var transpTypeArr = data.items.transportationType.split(':');
                    var i;
                    if (transpTypeArr.length > 0) {
                      
                        for (i = 0; i <= transpTypeArr.length - 1; i++) {
                            if (transpTypeArr[i].length > 0) {
                                addTransportationValueType(transpTypeArr[i]);
                            }
                        }
                    }
                    $('#dateBegin').val('');
                    $('#dateEnd').val('');
                    var period = data.items.periodTransportation.split(':');
                    if (period.length > 0) {
                        if (period[0] != null && period[0]!="") {
                            $('#dateBegin').val(period[0]);
                        }
                        if (period[1] != null && period[1] != "") {
                            $('#dateEnd').val(period[1]);
                        }
                    }

                    //vm.selectedContainerVolumes = [];
                    //var volumeTypeArr = data.items.volumeType.split(':');
                    //if (volumeTypeArr.length > 0) {
                    
                    //    for (i = 0; i <= volumeTypeArr.length - 1; i++) {
                    //        if (volumeTypeArr[i].length > 0) {
                    //            addContainerVolumeType(volumeTypeArr[i]);
                    //        }
                    //    }
                    //}
                    vm.selectedWagonTypes = [];
                    var wagonTypeArr = data.items.wagonType.split(';');
                    if (wagonTypeArr.length > 0) {
                        for (i = 0; i <= wagonTypeArr.length - 1; i++) {
                            if (wagonTypeArr[i].length > 0) {
                                addWagonValueType(wagonTypeArr[i],true);
                            }
                        }
                    }

                    vm.cargoNames = [];
                   var cargoArr = data.items.cargoName.split(':');
                   if (cargoArr.length > 0) {
                        for (i = 0; i <= cargoArr.length - 1; i++) {
                            if (cargoArr[i].length > 0) {
                                addCargoValueName(cargoArr[i]);
                            }
                        }
                   }

                   vm.selectedCargoGroupNames = [];
                   var cargoGroupArr = data.items.cargoGroup.split(':');
                   if (cargoGroupArr.length > 0) {
                    
                       for (i = 0; i <= cargoGroupArr.length - 1; i++) {
                           if (cargoGroupArr[i].length > 0) {
                               addCargoGroupValueName(cargoGroupArr[i]);
                           }
                       }
                   }

                   vm.companiesSending = [];
                   var companySendingArr = data.items.companySending.split(':');
                   if (cargoGroupArr.length > 0) {

                       for (i = 0; i <= companySendingArr.length - 1; i++) {
                           if (companySendingArr[i].length > 0) {
                               addCompanySendingValue(companySendingArr[i]);
                           }
                       }
                   }

                   vm.companiesRecipient = [];
                   var companyRecipientArr = data.items.companyRecipient.split(':');
                   if (companyRecipientArr.length > 0) {
                       for (i = 0; i <= companyRecipientArr.length - 1; i++) {
                           if (companyRecipientArr[i].length > 0) {
                               addCompanyRecipientValue(companyRecipientArr[i]);
                           }
                       }
                   }
                  
                   vm.selectedCountrySending = [];
                   var countriesSendingArr = data.items.countrySending.split(':');
                   if (countriesSendingArr.length > 0) {
                       for (i = 0; i <= countriesSendingArr.length - 1; i++) {
                           if (countriesSendingArr[i].length > 0) {
                               addCountrySendingValue(countriesSendingArr[i]);
                           }
                       }
                   }

                   vm.selectedCountryDelivering = [];
                   var countriesDeliveringArr = data.items.countryDelivering.split(':');
                   if (countriesDeliveringArr.length > 0) {
                       for (i = 0; i <= countriesDeliveringArr.length - 1; i++) {
                           if (countriesDeliveringArr[i].length > 0) {
                               addCountryDeliveringValue(countriesDeliveringArr[i]);
                           }
                       }
                   }

                   vm.selectedWaysSending = [];
                   var waysSendingArr = data.items.waySending.split(':');
                   if (waysSendingArr.length > 0) {
                       for (i = 0; i <= waysSendingArr.length - 1; i++) {
                           if (waysSendingArr[i].length > 0) {
                               addWaySendingValue(waysSendingArr[i]);
                           }
                       }
                   }

                   vm.selectedWaysDelivering = [];
                   var waysDeliveringArr = data.items.wayDelivering.split(':');
                   if (waysDeliveringArr.length > 0) {
                       for (i = 0; i <= waysDeliveringArr.length - 1; i++) {
                           if (waysDeliveringArr[i].length > 0) {
                               addWayDeliveringValue(waysDeliveringArr[i]);
                           }
                       }
                   }

                   vm.selectedSubjectsSending = [];
                   var subjectsSendingArr = data.items.subjectSending.split(':');
                   if (waysDeliveringArr.length > 0) {
                       for (i = 0; i <= subjectsSendingArr.length - 1; i++) {
                           if (subjectsSendingArr[i].length > 0) {
                               addSubjectSendingValue(subjectsSendingArr[i]);
                           }
                       }
                   }

                   vm.selectedSubjectsDelivering = [];
                   var subjectsDeliveringArr = data.items.subjectDelivering.split(':');
                   if (subjectsDeliveringArr.length > 0) {
                       for (i = 0; i <= subjectsDeliveringArr.length - 1; i++) {
                           if (subjectsDeliveringArr[i].length > 0) {
                               addSubjectDeliveringValue(subjectsDeliveringArr[i]);
                           }
                       }
                   }

                   vm.stationsSending = [];
                   var stationSendingArr = data.items.stationSending.split(':');
                   if (stationSendingArr.length > 0) {
                       for (i = 0; i <= stationSendingArr.length - 1; i++) {
                           if (stationSendingArr[i].length > 0) {
                               addStationSendingValue(stationSendingArr[i]);
                           }
                       }
                   }

                   vm.stationsDelivering = [];
                   var stationsDeliveringArr = data.items.stationDelivering.split(':');
                   if (stationsDeliveringArr.length > 0) {
                       for (i = 0; i <= stationsDeliveringArr.length - 1; i++) {
                           if (stationsDeliveringArr[i].length > 0) {
                               addStationDeliveringValue(stationsDeliveringArr[i]);
                           }
                       }
                   }

                   vm.ownersVagon = [];
                   var ownerVagonArr = data.items.ownerWagon.split(':');
                   if (ownerVagonArr.length > 0) {
                       for (i = 0; i <= ownerVagonArr.length - 1; i++) {
                           if (ownerVagonArr[i].length > 0) {
                               addOwnerVagonValue(ownerVagonArr[i]);
                           }
                       }
                   }

                   vm.payers = [];
                   var payerWagonArr = data.items.payerWagon.split(':');
                   if (payerWagonArr.length > 0) {
                       for (i = 0; i <= payerWagonArr.length - 1; i++) {
                           if (payerWagonArr[i].length > 0) {
                               addPayerValue(payerWagonArr[i]);
                           }
                       }
                   }

                   vm.rentersVagon = [];
                   var rentersVagonArr = data.items.renterWagon.split(':');
                   if (rentersVagonArr.length > 0) {
                       for (i = 0; i <= rentersVagonArr.length - 1; i++) {
                           if (rentersVagonArr[i].length > 0) {
                               addRenterVagonValue(rentersVagonArr[i]);
                           }
                       }
                   }

                   vm.cargoNamesEarlyTransportation = [];
                   var earlyTransportationCargoArr = data.items.earlyTransportationCargo.split(':');
                   if (earlyTransportationCargoArr.length > 0) {
                       for (i = 0; i <= earlyTransportationCargoArr.length - 1; i++) {
                           if (earlyTransportationCargoArr[i].length > 0) {
                               addCargoNameEarlyTransportationValueName(earlyTransportationCargoArr[i]);
                           }
                       }
                   }
                   angular.copy(vm.allcolumns, vm.columns);
                   vm.vagonSourceTypeParam = data.items.vagonType == "0" ? ["груженый рейс"] : ["порожний рейс"];
                   if (data.items.vagonType == "0") {
                       deleteColumnByCaption('Ранее перевозимый груз');
                   }
                   if (data.items.vagonType == "1") {
                       $('#trCargoNameEarlyTransportation').show();
                   } else {
                       $('#trCargoNameEarlyTransportation').hide();
                   }
                 
                   vm.selectedColumns = [];
                   var columnsArr = data.items.columns.split(':');
                   if (columnsArr.length > 0) {
                   
                       for (i = 0; i <= columnsArr.length - 1; i++) {
                           var columnArr = columnsArr[i].split(',');
                           var column = {
                               id: columnArr[0],
                               name: columnArr[1]
                           };
                           if (columnsArr[i].length > 0) {
                               vm.selectedColumns.push(column);
                               for (var j = 0; j <= vm.columns.length - 1; j++) {
                                   var current = column;
                                   var f = vm.columns[j];
                                   if (current.id == f.id) {
                                       vm.columns.splice(j, 1);
                                       break;
                                   }
                               }
                           }
                       }
                   }
                }
            });
           
        }

        function addTransportationValueType(val) {
            if (!validateDdlFilters(vm.selectedTransportationTypes, val))
                return false;

            var name = getObj(vm.selectedTransportationTypes, val);

            if (vm.selectedTransportationTypes == null) {
                vm.selectedTransportationTypes = [name];
            } else if (!isExists(val, vm.selectedTransportationTypes)) {
                vm.selectedTransportationTypes.push(name);
            } else {
                return false;
            }
            vm.transportationType = vm.transportationTypes[0];
            return true;
        }

        function createFilter() {
            var period = $('#dateBegin').val() + ':' + $('#dateEnd').val();
            var transportationType = "";
            var i;
            for (i = 0; i <= vm.selectedTransportationTypes.length - 1; i++)
                transportationType += ":" + vm.selectedTransportationTypes[i].name;

            var wagonType = "";
            for (i = 0; i <= vm.selectedWagonTypes.length - 1; i++)
                wagonType += ";" + vm.selectedWagonTypes[i].name;

            var volumeType = "";
        //    for (i = 0; i <= vm.selectedContainerVolumes.length - 1; i++)
         //       volumeType += ":" + vm.selectedContainerVolumes[i].name;

            var cargoName = "";
            for (i = 0; i <= vm.cargoNames.length - 1; i++)
                cargoName += ":" + vm.cargoNames[i].name;

            var cargoGroup = "";
            for (i = 0; i <= vm.selectedCargoGroupNames.length - 1; i++)
                cargoGroup += ":" + vm.selectedCargoGroupNames[i].name;

            var companySending = "";
            for (i = 0; i <= vm.companiesSending.length - 1; i++)
                companySending += ":" + vm.companiesSending[i].name;

            var companyRecipient = "";
            for (i = 0; i <= vm.companiesRecipient.length - 1; i++)
                companyRecipient += ":" + vm.companiesRecipient[i].name;

            var countrySending = "";
            for (i = 0; i <= vm.selectedCountrySending.length - 1; i++)
                countrySending += ":" + vm.selectedCountrySending[i].name;

            var countryDelivering = "";
            for (i = 0; i <= vm.selectedCountryDelivering.length - 1; i++)
                countryDelivering += ":" + vm.selectedCountryDelivering[i].name;

            var waySending = "";
            for (i = 0; i <= vm.selectedWaysSending.length - 1; i++)
                waySending += ":" + vm.selectedWaysSending[i].name;

            var wayDelivering = "";
            for (i = 0; i <= vm.selectedWaysDelivering.length - 1; i++)
                wayDelivering += ":" + vm.selectedWaysDelivering[i].name;

            var stationSending = "";
            for (i = 0; i <= vm.stationsSending.length - 1; i++)
                stationSending += ":" + vm.stationsSending[i].name;
            
            var stationDelivering = "";
            for (i = 0; i <= vm.stationsDelivering.length - 1; i++)
                stationDelivering += ":" + vm.stationsDelivering[i].name;
           
            var subjectSending = "";
            for (i = 0; i <= vm.selectedSubjectsSending.length - 1; i++)
                subjectSending += ":" + vm.selectedSubjectsSending[i].name;

            var subjectDelivering = "";
            for (i = 0; i <= vm.selectedSubjectsDelivering.length - 1; i++)
                subjectDelivering += ":" + vm.selectedSubjectsDelivering[i].name;

            var ownerWagon = "";
            for (i = 0; i <= vm.ownersVagon.length - 1; i++)
                ownerWagon += ":" + vm.ownersVagon[i].name;

            var payerWagon = "";
            for (i = 0; i <= vm.payers.length - 1; i++)
                payerWagon += ":" + vm.payers[i].name;

            var renter = "";
            for (i = 0; i <= vm.rentersVagon.length - 1; i++)
                renter += ":" + vm.rentersVagon[i].name;

            var earlyTransportationCargo = "";
            for (i = 0; i <= vm.cargoNamesEarlyTransportation.length - 1; i++)
                earlyTransportationCargo += ":" + vm.cargoNamesEarlyTransportation[i].name;
            
            var columns = "";
            for (i = 0; i <= vm.selectedColumns.length - 1; i++)
                columns += ":" + vm.selectedColumns[i].id + ',' + vm.selectedColumns[i].name;

            var vagonType = isGruzhTypeSelected() ? 0 : 1;

            dataService.saveCalcFilter(vm.firmFilterName, period, transportationType, wagonType, volumeType, cargoName, cargoGroup, companySending, companyRecipient,
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
                renter,
                columns, earlyTransportationCargo, vagonType).then(function (data) {
                    if (data != null) {
                        vm.firmFilterName = null;
                        loadFilterSettings();
                    
                        alert('Фильтр сохранен');
                }
            });
        }

        function rbListStationDeliveringClick() {
            vm.stationDeliveringSearchParamArr = [];
            var val = vm.stationDeliveringSearchParam; 
            addValueIfExists(vm.stationDeliveringSearchParamArr, 45, val, false);
            m.stationDeliveringSearchParam = null;
            return true;
        }
        $('#trCargoNameEarlyTransportation').hide();
        function rbVagonSourceTypeClick() {
            getColumnsFromDb();

            if (!isGruzhTypeSelected()) {
                $('#trCargoNameEarlyTransportation').show();
            } else {
                $('#trCargoNameEarlyTransportation').hide();
                $('#txtCargoName').val('');
            }
        }

        function excludeColumnsForPoroznieType() {
            deleteColumnByCaption('Тонно-киллометры');
            deleteColumnByCaption('Объем перевозки тонн');
            deleteColumnByCaption('Расстояние перевозки');
            deleteColumnByCaption('Тарифный класс груза');
            deleteColumnByCaption('Количество контейнеров');
        }

        function addColumns() {
            for (var i = 0; i <= vm.column.length - 1; i++)
                vm.selectedColumns.push(vm.column[i]);

            for (i = 0; i <= vm.column.length - 1; i++)
                deleteItem(vm.column[i], vm.columns);

            vm.column = null;
            vm.selectedColumns[0].isCurrent = true;
        }

        vm.addColumnsAll = addColumnsAll;
        vm.deleteColumnsAll = deleteColumnsAll;
        vm.moveUpColumns = moveUpColumns;
        vm.moveDownColumns = moveDownColumns;
        function addColumnsAll() {
            if (vm.selectedColumns == null)
                angular.copy(vm.columns, vm.selectedColumns);
            else {
                for (var i = 0; i <= vm.columns.length - 1; i++)
                    vm.selectedColumns.push(vm.columns[i]);
                vm.columns = [];
            }
        }

        function deleteColumnsAll() {
            vm.columns = [];
            angular.copy(vm.allcolumns, vm.columns);
            vm.selectedColumns = [];
        }

       
        function moveUpColumns() {
           
            var objSelected = [];
            var objReplaced = [];
            if (vm.selectedColumn.length == 1) {
                angular.copy(vm.selectedColumn, objSelected);
                for (var i = 0; i <= vm.selectedColumns.length - 1; i++) {
                    if(i == 0 && vm.selectedColumns[i].name == objSelected[0].name)
                    break;

                    if (vm.selectedColumns[i].name == objSelected[0].name) {
                        angular.copy(vm.selectedColumns[i - 1], objReplaced);
                        vm.selectedColumns[i - 1].name = objSelected[0].name;
                        vm.selectedColumns[i - 1].id = objSelected[0].id;
                        vm.selectedColumns[i].name = objReplaced.name;
                        vm.selectedColumns[i].id = objReplaced.id;
                        //$('#selectedColumnsTxt option:selected').each(function () {
                        //    this.selected = false;
                        //});
                      //  vm.selectedOptionColumn = objSelected[0];
                        g_index = i - 1;
                       // vm.selectedColumn = { selected: objSelected[0].name };
                        vm.selectedColumn = objSelected;
                     //   $("#selectedColumnsTxt [value='" + index + "']").attr("selected", "selected"); return true;
                        break;
                    }
                }
            }
        }
     
        function deleteColumnByCaption(caption) {
          
            for (var i = 0; i <= vm.columns.length - 1; i++)
                if (caption == vm.columns[i].name)
                    vm.columns.splice(i, 1); 

            for (i = 0; i <= vm.selectedColumns.length - 1; i++)
                if (caption == vm.selectedColumns[i].name)
                    vm.selectedColumns.splice(i, 1);
        }

        vm.selectedOptionColumn = null;
        var g_index;
        function moveDownColumns() {
            var objSelected = [];
            var objReplaced = [];
            if (vm.selectedColumn.length == 1) {
                angular.copy(vm.selectedColumn, objSelected);
                for (var i = 0; i <= vm.selectedColumns.length - 1; i++) {

                    if (i == vm.selectedColumns.length - 1 && vm.selectedColumns[i].name == objSelected[0].name)
                        break;
                    if (vm.selectedColumns[i].name == objSelected[0].name) {
                        angular.copy(vm.selectedColumns[i + 1], objReplaced);
                        vm.selectedColumns[i + 1].name = objSelected[0].name;
                        vm.selectedColumns[i + 1].id = objSelected[0].id;
                        vm.selectedColumns[i].name = objReplaced.name;
                        vm.selectedColumns[i].id = objReplaced.id;
                       // g_selectedColName = objSelected[0].id;
                        vm.selectedOptionColumn = objSelected;
                        //$('#selectedColumnsTxt option:selected').each(function () {
                        //    this.selected = false;
                        //});
                        g_index = i + 1;
                        vm.selectedColumn = objSelected;
                      //  $("#selectedColumnsTxt [value='" + index + "']").attr("selected", "selected");
                      //  return true;
                        break;
                        // vm.selectedColumns
                    }
                }
            }
        }
        

        function deleteColumns() {
            if (vm.selectedColumn == null)
                return;

            for (var i = 0; i <= vm.selectedColumn.length - 1; i++)
                for (var j = 0; j <= vm.selectedColumns.length - 1; j++) {

                    if (vm.selectedColumns[j].id == vm.selectedColumn[i].id) {
                        vm.selectedColumns.splice(j, 1);
                        break;
                    }
                }

            angular.copy(vm.allcolumns, vm.columns);
            for ( i = 0; i <= vm.selectedColumns.length - 1; i++)
                for ( j = 0; j <= vm.columns.length - 1; j++) {

                    if (vm.columns[j].id == vm.selectedColumns[i].id) {
                        vm.columns.splice(j, 1);
                        break;
                    }
                }
            
            vm.selectedValuesRightTemp = null;
            if (!isGruzhTypeSelected()) {;
                excludeColumnsForPoroznieType();
            }
        }
        
        getColumnsFromDb();
        function getColumnsFromDb() {
            dataService.getColumnsSearchResult().then(function (data) {
                vm.columns = data;
                angular.copy(data, vm.allcolumns);  
             
                if (!isGruzhTypeSelected()) {
                    excludeColumnsForPoroznieType();
                } else {
                    deleteColumnByCaption('Ранее перевозимый груз');
                }
            });
        }
       
        function Name(name, id) {

            return {
                name: name,
                id: id
            };
        }

        function getId(length, name) {
            var id;
            if (name == g_ALL) {
                id = -1;//настройка поиска по умолчанию
            } else {
                id = length + 1;
            }
            return id;
        }
        
        var filterIdsNumberVagon = 1;
        var filterIdsTransportationTypes = 4;
        var filterIdsOwnerVagonTypes = 42;
        var filterIdsCargoName = 10;
        var filterIdsCargoNameEarlyTransportation = 53;
        var filterIdsCargoGroupName = 48;
        var filterIdsCompanySending = 43;
        var filterIdsCompanyRecipient = 44;
        var filterIdsCountrySending = 11;
        var filterIdsCountryDelivering = 20;
        var filterIdsWaysSending = 12;
        var filterIdsWaysDelivering = 21;
        var filterIdsSubjectsSending = 13;
        var filterIdsSubjectsDelivering = 22;

        var filterIdsStationsSending = 14;
        var filterIdsStationsDelivering = 23;

        var filterIdsOwnerVagon = 29;
        var filterIdsPayer = 31;

        var filterIdsRenter = 30;

        
        function addVagonNumber() {
            if (vm.vagonNumber.length >= maxLengthFilters)
                return false;

            addVagonNumberValue(vm.vagonNumber);
            return true;
        }

        function addVagonNumberValue(val) {
            addValueIfExists(vm.vagonNumbers, filterIdsNumberVagon, val, false);
            vm.vagonNumber = null;
            return true;
        }

        vm.ownerVagon = null;
        function addOwnerVagon() {
            if (vm.ownersVagon.length >= maxLengthFilters)
                return false;

            addOwnerVagonValue(vm.ownerVagon);
            return true;
        }

        function addOwnerVagonValue(val) {
            addValueIfExists(vm.ownersVagon, filterIdsOwnerVagon, val, false);
            vm.ownerVagon = null;
            return true;
        }

        vm.renterVagon = null;
        function addRenterVagon() {
            if (vm.ownersVagon.length >= maxLengthFilters)
                return true;

             addRenterVagonValue(vm.renterVagon); 
            return true;
        }

        function addRenterVagonValue(val) {
            addValueIfExists(vm.rentersVagon, filterIdsRenter, val, false);
            vm.renterVagon = null;
            return true;
        }

        vm.payer = null;
        function addPayer() {
            if (vm.payers.length >= maxLengthFilters)
                return true;

            addPayerValue(vm.payer); 
            return true;
        }

        function addPayerValue(val) {
            addValueIfExists(vm.payers, filterIdsPayer, val, false);
            vm.payer = null;
            return true;
        }

        vm.stationSending = null;
        function addStationSending() {
            if (vm.stationsSending.length >= maxLengthFilters)
                return false;

            addStationSendingValue(vm.stationSending);
            return true;
        }

        function addStationSendingValue(val) {
            addValueIfExists(vm.stationsSending, filterIdsStationsSending, val, true);
            vm.stationSending = null;
            return true;
        }

        vm.stationDelivering = null;
        function addStationDelivering() {
            if (vm.stationsDelivering.length >= maxLengthFilters)
                return true;

            addStationDeliveringValue(vm.stationDelivering); 
           
            return true;
        }

        function addStationDeliveringValue(val) {
            addValueIfExists(vm.stationsDelivering, filterIdsStationsDelivering, val, true);
            vm.stationDelivering = null;
            return true;
        }

        vm.wayDelivering = null;
        function addWayDelivering() {
            if (vm.selectedWaysDelivering.length >= maxLengthFilters)
                return true;

            addWayDeliveringValue(vm.wayDelivering);
            return true;
        }

        function addWayDeliveringValue(val) {
            addValueIfExists(vm.selectedWaysDelivering, filterIdsWaysDelivering, val, false);
            vm.wayDelivering = null;
            return true;
        }

        vm.subjectSending=null;
        function addSubjectSending() {
            if (vm.selectedSubjectsSending.length >= maxLengthFilters)
                return true;

            addSubjectSendingValue(vm.subjectSending);
            return true;
        }

        function addSubjectSendingValue(val) {
            addValueIfExists(vm.selectedSubjectsSending, filterIdsSubjectsSending, val, false);
            vm.subjectSending = null;
        }

        vm.subjectDelivering = null;
        function addSubjectDelivering() {
            if (vm.selectedSubjectsDelivering.length >= maxLengthFilters)
                return true;

            addSubjectDeliveringValue(vm.subjectDelivering);
            return true;
        }

        function addSubjectDeliveringValue(val) {
            addValueIfExists(vm.selectedSubjectsDelivering, filterIdsSubjectsDelivering, val, false);
            vm.subjectDelivering = null;
            return true;
        }

        vm.waySending = null;
        function addWaySending() {
            if (vm.selectedWaysSending.length >= maxLengthFilters)
                return true;

            addWaySendingValue(vm.waySending);
            return true;
        }

        function addWaySendingValue(val) {
            addValueIfExists(vm.selectedWaysSending, filterIdsWaysSending, val, false);
            vm.waySending = null;
            return true;
        }

        function getObj(obj, val) {
            var length = obj == null ? 0 : obj.length;
            var id = getId(length, val);
            var name = new Name(val, id);
            return name;
        }

        function addOkpo() {
            var val = $('#txtOKPO').val();
            var name = getObj(vm.okpos, val);

            if (vm.okpos == null) {
                vm.okpos = [name];
            } else if (!isExists(val, vm.okpos)) {
                vm.okpos.push(name);
            } else {
                return false;
            }
            return true;
        }

        function addCargoGroupName() {
            if (!validateDdlFilters(vm.selectedCargoGroupNames, vm.cargoGroupName.name))
                return false;

            addCargoGroupValueName(vm.cargoGroupName.name);
            vm.cargoGroupName = vm.cargoGroupNames[0];
            return true;
        }

        function addCargoGroupValueName(val) {
         
            var name = getObj(vm.selectedCargoGroupNames, val);

            if (vm.selectedCargoGroupNames == null) {
                vm.selectedCargoGroupNames = [name];
            } else if (!isExists(val, vm.selectedCargoGroupNames)) {
                vm.selectedCargoGroupNames.push(name);
            } else {
                return false;
            }
            return false;
        }

        vm.cargoName = null;   
        function addCargoName() {
            if (vm.cargoNames.length >= maxLengthFilters)
                return false;

            addCargoValueName(vm.cargoName);
            return false;
        }

        vm.cargoNamesEarlyTransportation = [];
        vm.cargoNameEarlyTransportation =null;
        vm.addCargoNameEarlyTransportation = addCargoNameEarlyTransportation;

        function addCargoNameEarlyTransportation() {
            if (vm.cargoNamesEarlyTransportation.length >= maxLengthFilters)
                return false;

            addCargoNameEarlyTransportationValueName(vm.cargoNameEarlyTransportation);
            return false;
        }
        
        function addCargoNameEarlyTransportationValueName(val) {
            addValueIfExists(vm.cargoNamesEarlyTransportation, filterIdsCargoNameEarlyTransportation, val, false);
            vm.cargoNameEarlyTransportation = null;
        }
       

        function addCargoValueName(val)
        {
            addValueIfExists(vm.cargoNames, filterIdsCargoName, val, true);
            vm.cargoName = null;
        }

        function validateDdlFilters(arr, name) {
            if (name == null || name == '')
                return false;

            if (arr.length >= maxLengthFilters)
                return false;

            if (name == g_SELECT_VALUES)
                return false;

            return true;
        }

        function addWagonType() {
          
            if (!validateDdlFilters(vm.selectedWagonTypes, vm.wagonType.WagonName))
                return false;

            var val = vm.wagonType.WagonName;
            addWagonValueType(val);
            return true;
        }

        function addWagonValueType(val, isLoadFilter) {
            isLoadFilter = isLoadFilter == undefined ? false : true;
            var nameObj = getObj(vm.selectedWagonTypes, val);

            if (!isLoadFilter) {
                     if (nameObj.name.indexOf('---') == 0) {
                nameObj.name = nameObj.name.substr(3, nameObj.name.length - 1);
                nameObj.name = 'подгруппа: ' + nameObj.name;
                }
                else
                    nameObj.name = 'группа: ' + nameObj.name;
            } 

            if (vm.selectedWagonTypes == null) {
                vm.selectedWagonTypes = [nameObj];
            } else if (!isExists(val, vm.selectedWagonTypes)) {
                vm.selectedWagonTypes.push(nameObj);
            } else {
                return false;
            }
            vm.wagonType = vm.wagonTypes[0];
            return true;
        }


        //function addContainerVolume() {
        //    if (!validateDdlFilters(vm.selectedContainerVolumes, vm.containerVolume.name))
        //        return false;
        //    addContainerVolumeType(vm.containerVolume.name);
        //    return true;
        //}

        //function addContainerVolumeType(val) {
        //    var name = getObj(vm.selectedContainerVolumes, val);

        //    if (vm.selectedContainerVolumes == null) {
        //        vm.selectedContainerVolumes = [name];
        //    } else if (!isExists(val, vm.selectedContainerVolumes)) {
        //        vm.selectedContainerVolumes.push(name);
        //    } else {
        //        return false;
        //    }
        //    vm.containerVolume = vm.containerVolumes[0];
        //    return true;
        //}

        vm.countrySending = null;
        function addCountrySending() {
            if (vm.selectedCountrySending.length >= maxLengthFilters)
                return true;

            addCountrySendingValue(vm.countrySending);
          
            return true;
        }

        function addCountrySendingValue(val) {
            
            addValueIfExists(vm.selectedCountrySending, filterIdsCountrySending, val, false);
            vm.countrySending = null;
            return true;
        }

        vm.countryDelivering = null;
        function addCountryDelivering() {
            if (vm.selectedCountryDelivering.length >= maxLengthFilters)
                return true;

            addCountryDeliveringValue(vm.countryDelivering);

            return true;
        }

        function addCountryDeliveringValue(val) {
           
            addValueIfExists(vm.selectedCountryDelivering, filterIdsCountryDelivering, val, false);
            vm.countryDelivering = null;
        }

        function addCompanySending() {
            if (vm.companiesSending.length >= maxLengthFilters)
                return true;

            addCompanySendingValue(vm.companySending);
            return true;
        }

        function addCompanySendingValue(val) {
            addValueIfExists(vm.companiesSending, filterIdsCompanySending, val, true);
            vm.companySending = null;
        }


        vm.companyRecipient = null;
        function addCompanyRecipient() {
            if (vm.companiesRecipient.length >= maxLengthFilters)
                return true;

            addCompanyRecipientValue(vm.companyRecipient);
            return true;
        }

        function addCompanyRecipientValue(val) {
            addValueIfExists(vm.companiesRecipient, filterIdsCompanyRecipient, val, true);
            vm.companyRecipient = null;
        }


        vm.transportationType = null;
        vm.selectedTransportationTypes = [];
        function addTransportationType() {            
            addTransportationValueType(vm.transportationType.name);
            return true;
        }
        
        function isExists(name, arr) {
            for (var i = 0; i != arr.length; i++) {
                var current = arr[i];

                if (current.name === name) {
                    return true;
                }
            }
            return false;
        }
        
        function deleteItem(f, arr) {
            for (var i = 0; i != arr.length; i++) {
                var current = arr[i];

                if (current === f) {
                    arr.splice(i, 1);
                    break;
                }
            }
        }

        var notSelectedFieldInCalcWarning = 'Не выбраны значения в калькуляторе!';
        function calculateForm() {
           
             getSelectedParams();

            if (vm.selectedFilters == 0) {
                alert(notSelectedFieldInCalcWarning);
                return;
            }

            dataService.getCalculatorResult(vm.selectedFilters).then(function (data) {
                vm.calculatorResult = data.result;
                vm.calcCoeffDetailReport = data.calcCoeffDetailReport;
                
            });
        }
        
        function searchForm(onlyRowsCount) {
            
            if (vm.selectedColumns.length == 0) {
                alert('Не выбраны поля для отображения!');
                return;
            }

            getSelectedParams();

            if (vm.selectedFilters == null || vm.selectedFilters.length == 0) {
                alert(notSelectedFieldInCalcWarning);
                vm.searchResult = [];
                $('#divSearchResultGrid').hide();
                $('#divSearchResultEmpty').show();
                return;
            }

            $('#divTotalRecords').show();

            $("#btnSearchReport").attr('disabled', 'disabled');//.val("Обработка ...");
            blockPageUI('<img src="/img/ajax-loader.gif"  " alt=""/>  ');
            dataService.searchDataByFilter(1, vm, onlyRowsCount);
        }

        function showRowsCount() {
            searchForm(true);
        }
      
        function getSelectedParams() {
           
            vm.selectedFilters = [];

            var obj = getSelectedFilterParams(filterIdsNumberVagon, vm.vagonNumbers);
            if (obj != null)
                vm.selectedFilters.push(obj);

             obj = getSelectedFilterParams(filterIdsTransportationTypes, vm.selectedTransportationTypes);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(filterIdsOwnerVagonTypes, vm.selectedWagonTypes);
            if (obj != null)
                vm.selectedFilters.push(obj);

          //  obj = getSelectedFilterParams(8, vm.selectedContainerVolumes);
          //  if (obj != null)
            //    vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(filterIdsCargoName, vm.cargoNames);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(filterIdsCargoGroupName, vm.selectedCargoGroupNames);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(11, vm.selectedCountrySending);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(12, vm.selectedWaysSending);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(13, vm.selectedSubjectsSending);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(14, vm.stationsSending);
            if (obj != null)
                vm.selectedFilters.push(obj);//Код

            obj = getSelectedFilterParams(20, vm.selectedCountryDelivering);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(21, vm.selectedWaysDelivering);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(22, vm.selectedSubjectsDelivering);
            if (obj != null)
                vm.selectedFilters.push(obj);
            
            obj = getSelectedFilterParams(23, vm.stationsDelivering);
            if (obj != null)
                vm.selectedFilters.push(obj);//Код

            obj = getSelectedFilterParams(29, vm.ownersVagon);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(30, vm.rentersVagon);
            if (obj != null)
                vm.selectedFilters.push(obj);

            if (!isGruzhTypeSelected()) {
                obj = getSelectedFilterParams(53, vm.cargoNamesEarlyTransportation);
                if (obj != null)
                    vm.selectedFilters.push(obj);
            }

            obj = getSelectedFilterParams(31, vm.payers);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(43, vm.companiesSending);
            if (obj != null)
                vm.selectedFilters.push(obj);

            obj = getSelectedFilterParams(44, vm.companiesRecipient);
            if (obj != null)
                vm.selectedFilters.push(obj);

            if (vm.stationsDelivering.length > 0) {
                obj = getSelectedFilterParams(45, vm.stationDeliveringSearchParamArr);
                if (obj != null)
                    vm.selectedFilters.push(obj);
            }
    
            getTransportationPeriod();

            obj = getSelectedFilterParams(3, vm.transportationPeriod);
            if (obj != null)
                vm.selectedFilters.push(obj);

            vm.selectedColumnsFilter = [];
            vm.selectedDopInfo = [];
            angular.copy(vm.selectedColumns, vm.selectedColumnsFilter);
          
        }
        
        function getTransportationPeriod()
        {
            if ($('#dateBegin').val()!='' || $('#dateEnd').val()!='') {
                var dateBegin = getObj(vm.dateBegin, $('#dateBegin').val());
                var dateEnd = getObj(vm.dateEnd, $('#dateEnd').val());
                vm.transportationPeriod = [dateBegin];
                vm.transportationPeriod.push(dateEnd);
            }
        }

        function getSelectedFilterParams(filterId, filterArr) {

            var selectedCount = filterArr == null ?  0 : filterArr.length;//-1

            var obj;
            if (selectedCount <= 0) { //TODO
                obj = null;//selectedObject(filterId, 0, null);//настройка поиска по умолчанию -1 - все
            }
            else {
                var count;
                if (selectedCount == 1)
                    count = filterArr[0].name == g_ALL ? -1 : 1;//-1
                else {
                    count = selectedCount;
                }
                obj = selectedCount == 1 ? selectedObject(filterId, count, filterArr) : selectedObject(filterId, selectedCount, filterArr);
            }

            return obj;
        }

        function loadFilterSettings() {
            dataService.loadFiltersSettings().then(function (data) {
                vm.allFiltersSettingsList = data.items;
                initData();
            });
        }

        function selectedObject(filterId, selectedValue, values) {
            return {
                filterId: filterId,
                cv: selectedValue,
                sv: values
            };
        }
        
        function loadPage(pageId) {
            blockPageUI('<img src="/img/ajax-loader.gif"   alt=""/>');
            dataService.searchDataByFilter(pageId, vm, false);
        }

     
        function initData() {
            vm.stationDeliveringSearchParamArr = [];
             addValueIfExists(vm.stationDeliveringSearchParamArr, 45, vm.stationDeliveringSearchParam[0], false);
             loadDictionaties();
         
        }
        
        function loadDictionaties() {
            loadCargoGroupNames();
            loadWagonTypes();
            loadTransTypes();
        }

      

    
        function loadCargoGroupNames() {
            dataService.getCargoGroupNames().then(function (data) {
                vm.cargoGroupNames = data;
                vm.cargoGroupName = data[0];
            });
        }
        
       
        function loadWagonTypes() {
            dataService.getWagonTypes().then(function (data) {
                vm.wagonTypes = data;
                vm.wagonType = data[0];
            });
        }
  
       function loadTransTypes() {
           dataService.getTransTypes().then(function (data) {
               vm.transportationTypes = data;
               vm.transportationType = data[0];
               
          //     $.unblockUI();
            });
       }

       function downloadReport() {
           dataService.downloadReport(vm.sessionId).then(function (data) {
               if (data != null) {
               }
           });
           alert('Отчет отправлен на формирование, через несколько минут его можно будет скачать в кабинете.');
       }

       
        function addValueIfExists(arr, filterName, nameValue, checkSlash) {
            if (!validateAddFilterValue(arr,nameValue, checkSlash))
                return false;

            addToArray(arr, nameValue);
            return true;
        }

        function validateAddFilterValue(arr, nameValue, checkSlash) {
            if (nameValue == null || nameValue.trim() == '')
                return false;

            if (checkSlash && nameValue.indexOf('|') <= 0) {
                alert('Неверный формат ввода данных');
                return false;
            }

            if (isExists(nameValue, arr)) {
                return false;
            }
            return true;
        }
        
        function addToArray(arr, nameValue) {
            var name = getObj(vm.companiesRecipient, nameValue);

            if (arr == null) {
                arr = [name];
            } else {
                arr.push(name);
            }
        }

        vm.sessionId = null;
        getSessionId();
        function getSessionId() {
            dataService.getSessionId().then(function (data) {
                vm.sessionId = data.sessionId;
            });
        }


        loadFilterSettings();
    }
})();