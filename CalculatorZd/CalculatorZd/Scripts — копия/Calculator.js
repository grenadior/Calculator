(function () {
    'use strict';
 
    var app = angular.module('app', []);

    app.controller('binding', binding);
 
    function binding() {
      
        var vm = this;
        vm.firstName = 'Sasha';
        vm.lastName = 'Shukletsov';
        vm.fullName = fullName;
        vm.friends = [new friend('Steve'), new friend('Annie')];
        vm.addFriend = addFriend;
        vm.deleteFriend = deleteFriend;
        vm.date = date;

        function fullName() {
            return vm.firstName + ' ' + vm.lastName;
        }

        function friend(name) {
            return {
                name: name
            };
        }

        function addFriend() {
            vm.friends.push(new friend('another'));
        }

        function deleteFriend(f) {
            for (var i = 0; i != vm.friends.length; i++) {
                var current = vm.friends[i];

                if (current === f) {
                    vm.friends.splice(i, 1);
                    break;
                }
            }
        }

        function date() {
            return new Date();
        }
    }
})();