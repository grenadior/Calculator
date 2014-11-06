define(['durandal/plugins/http', 'viewModels/model'],
    function(http, model) {
        'use strict';

        var getAllEntitiesUrl = '/api/entities';

        var service = {
            getAllEntities: getAllEntities
        };
        return service;

        function getAllEntities(typeId, entitiesObservable) {
            var config = {
                typeId: typeId
            };
            
            var start = new Date();
            
            return http.get(getAllEntitiesUrl, config).then(buildEntityViewModel);

            function buildEntityViewModel(data) {
               // logTime('loaded ' + itemPerPage + ' items', start);
                start = new Date();
                var entities = [];
                for (var i = 0; i != data.length; i++) {
                    var m = model.initEntity(data[i]);
                    entities.push(m);
                }

                entitiesObservable(entities);
                logTime('data binding finished', start);
            }
        }

        function logTime(m, s) {
            if (!s) {
                return;
            }

            var end = new Date();
            console.log(m, (end - s) + ' ms');
        }
    });