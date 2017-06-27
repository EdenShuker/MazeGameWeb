$(function() {
    var ViewModel = function() {
        var self = this; // make 'this' available to subfunctions or closures
        self.users = ko.observableArray(); // enables data binding
        var usersUri = "../api/Users";

        // returns all user rankings
        function getAllRankings() {
            $.getJSON(usersUri).done(function(data) {
                self.users(data);
            }).fail(function() {
                alert("Could not start connection with the server");
            });
        }

        // Fetch the initial data
        getAllRankings();
    };

    ko.applyBindings(new ViewModel());
});