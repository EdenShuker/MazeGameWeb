var ViewModel = function() {
    var self = this; // make 'this' available to subfunctions or closures
    self.users = ko.observableArray(); // enables data binding
    var usersUri = "../api/Users";

    function getAllUsers() {
        $.getJSON(usersUri).done(function (data) {
            self.users(data);
        });
    }

    // Fetch the initial data
    getAllUsers();
};

ko.applyBindings(new ViewModel());