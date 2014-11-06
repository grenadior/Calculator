
var app = angular.module('app', ['marked', 'angular-bootstrap-select', 'angular-bootstrap-select.extra']);

app.controller('SelectCtrl', ['$rootScope', SelectCtrl]);

function SelectCtrl($scope) {
    $scope.form = undefined;


    $scope.changeLanguage = function (langKey) {
       // alert('LanguageCtrl requested with langKey:' + langKey);
        //  $translate.uses(langKey);
      //  debugger;
        alert(langKey);
        //  var dd = dataService.getSessionId();
    };

}


//var app = angular.module('app', ['marked', 'angular-bootstrap-select', 'angular-bootstrap-select.extra']);

//app.controller('SelectCtrl', ['$rootScope', SelectCtrl]);

//function SelectCtrl($scope) {
//    $scope.form = undefined;

//    $scope.changeLanguage = function (langKey) {
//        alert('LanguageCtrl requested with langKey:' + langKey);
//        //  $translate.uses(langKey);
//        //  debugger;
//       //  var dd = dataService.getSessionId();
//    };
//}

angular.module('marked', []).directive('marked', function () {
  return {
    restrict: 'E',
    link: function (scope, element, attrs) {
      marked.setOptions({
        gfm: true,
        tables: true,
        breaks: false,
        pedantic: false,
        sanitize: true,
        smartLists: true,
        smartypants: false,
      });   

      marked(element.text(), function (err, content) {
        if (err) throw err;
        element.html(content);
      
      });
    }
  };
});



//angular.module('selectDemoApp', ['marked', 'angular-bootstrap-select', 'angular-bootstrap-select.extra']);
//function selectCtrl($scope) {
//    $scope.form = undefined;

//    $scope.changeLanguage = function (langKey) {
//        alert('LanguageCtrl requested with langKey:' + langKey);
//        //  $translate.uses(langKey);
//        //  debugger;
//        //  var dd = dataService.getSessionId();
//    };
//}


//angular.module('marked', []).directive('marked', function () {
//    return {
//        restrict: 'E',
//        link: function (scope, element, attrs) {
//            marked.setOptions({
//                gfm: true,
//                tables: true,
//                breaks: false,
//                pedantic: false,
//                sanitize: true,
//                smartLists: true,
//                smartypants: false,
//            });

//            marked(element.text(), function (err, content) {
//                if (err) throw err;
//                element.html(content);
//            });
//        }
//    };
//});
//angular.module('app', ['marked', 'angular-bootstrap-select', 'angular-bootstrap-select.extra']);


//(function () {
//    var app = angular.module('app', ['marked', 'angular-bootstrap-select', 'angular-bootstrap-select.extra']);
//    app.controller('selectCtrl', ['$rootScope', 'dataService', selectCtrl]);

//    function selectCtrl($scope, dataService) {
//        $scope.form = undefined;

//        $scope.changeLanguage = function (langKey) {
//            alert('LanguageCtrl requested with langKey:' + langKey);
//            //  $translate.uses(langKey);
//            //  debugger;
//            //  var dd = dataService.getSessionId();
//        };
//    }


//    angular.module('marked', []).directive('marked', function () {
//        return {
//            restrict: 'E',
//            link: function (scope, element, attrs) {
//                marked.setOptions({
//                    gfm: true,
//                    tables: true,
//                    breaks: false,
//                    pedantic: false,
//                    sanitize: true,
//                    smartLists: true,
//                    smartypants: false,
//                });

//                marked(element.text(), function (err, content) {
//                    if (err) throw err;
//                    element.html(content);
//                });
//            }
//        };
//    });

//})();