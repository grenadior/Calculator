(function () {
    'use strict';

    var viewModel = {
        firstName: ko.observable("Sasha"),
        lastName: ko.observable("Shukletsov"),
        friends: ko.observableArray([new friend('Steve'), new friend('Annie')]),
        addFriend: addFriend,
        deleteFriend: deleteFriend
    };

    viewModel.fullName = ko.dependentObservable(function() {
        return this.firstName() + ' ' + this.lastName();
    }, viewModel);

    ko.applyBindings(viewModel);
    
    function friend(name) {
        return {
            name: ko.observable(name)
        };
    }

    function addFriend() {
        viewModel.friends.push(new friend('another'));
    }
    function deleteFriend(f) {
        viewModel.friends.remove(f);
    }
})();