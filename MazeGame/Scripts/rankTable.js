var ViewModel = function () {
    var self = this; // make 'this' available to subfunctions or closures
    self.userRankings = ko.observableArray(); // enables data binding
    var userRankingsUri = "../api/UserRankings";

    function getAllRankings() {
        $.getJSON(userRankingsUri).done(function (data) {
            self.userRankings(data);
        });
    }
    // Fetch the initial data
    getAllRankings();
};

ko.applyBindings(new ViewModel());