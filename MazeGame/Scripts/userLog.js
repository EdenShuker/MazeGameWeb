$(function() {
    var ViewModel = function() {
        var self = this; // make 'this' available to subfunctions or closures
        var usersUri = "../api/Users";

        self.userName = ko.observable("");
        self.email = ko.observable("");
        self.password = ko.observable("");
        self.confirm_password = ko.observable("");


        self.login = function() {
            var user = { Name: self.userName(), Password: self.password() }
            $.ajax({
                url: usersUri + "/login",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(user),
                success: function() {
                    // save user
                    sessionStorage.setItem('user', user.Name);
                    window.location.href = "../index.html";
                },
                error: function() {
                    $("#username").val("");
                    $("#password").val("");
                    alert("User was not found");
                }
            });
        };


        self.register = function() {
            // check for non empty input in all
            if (self.userName() === "" ||
                self.email() === "" ||
                self.password() === "" ||
                self.confirm_password() === "") {
                alert("Fill all the fields");
            } else if (!self.email().includes("@")) {
                // check valid email
                alert("Invalid email");
            } else {
                if (self.password() === self.confirm_password()) {
                    var user = {
                        Name: self.userName(),
                        Email: self.email(),
                        Password: self.password()
                    };
                    $.post(usersUri, user).done(function(item) {
                            sessionStorage.setItem("user", self.userName());
                            window.location.href = "../index.html";
                        })
                        .fail(function(jqXHR, textStatus, err) {
                            alert("adding user failed");
                        });
                } else {
                    $("#password").val("");
                    $("#confirm_password").val("");
                    alert("Passwords don't match, please try again.");
                }
            }
        };
    };

    ko.applyBindings(new ViewModel());
});