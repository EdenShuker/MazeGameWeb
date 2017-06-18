var ViewModel = function () {
    var self = this;

    // set values
    self.rows = ko.observable(localStorage.getItem("rows"));
    self.cols = ko.observable(localStorage.getItem("cols"));
    self.searchAlgo = ko.observable(localStorage.getItem("searchAlgo"));

    self.saveSettings = function () {
        // update values
        localStorage.setItem("rows", self.rows());
        localStorage.setItem("cols", self.cols());
        localStorage.setItem("searchAlgo", self.searchAlgo());
        alert("Your changes has been made");
    }
}

ko.applyBindings(new ViewModel());