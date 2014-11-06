define(['knockout'],
    function (ko) {
        var model = {
            initEntity: initEntity
        };
        return model;
        
        function initEntity(dto) {
            //            dto.rates = ko.observableArray(rates);
//            dto.rates.forEach(function(r) {
//                r.cost = ko.observable(r.cost);
//            });

            return dto;
        }
    });