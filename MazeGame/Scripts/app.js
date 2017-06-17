var ViewModel = function () {
    var self = this; // make 'this' available to subfunctions or closures
    var usersUri = "../api/Users";

    self.userName = ko.observable("");
    self.email = ko.observable("");
    self.password = ko.observable("");
    self.confirm_password = ko.observable("");


    self.login = function () {
        $.getJSON(usersUri + "/" + self.userName()).done(function (data) {
            this.name1 = self.userName();
            this.password1 = self.password();
            alert(this.password1 + " " + data.Password);
            if (this.password1 === data.Password) {
                sessionStorage.setItem('user', this.name1);
                alert("passwords match");
                window.location.href = "../index.html";
            } else {
                alert("passwords doesn't match");
            }
        })
            .fail(function (jqXHR, textStatus, err) {
                alert("user doesn't exist");
            });
    };


    self.register = function () {
        if (self.userName() === "" || self.email() === "" || self.password() === "" || self.confirm_password() === "") {
            alert("Fill all the fields");
        } else {
            if (self.email().includes("@") && self.password() === self.confirm_password()) {
                var user = {
                    Name: self.userName(),
                    Email: self.email(),
                    Password: self.password()
                };
                $.post(usersUri, user).done(function (item) {
                    alert(item.Name + " " + item.Email);
                })
                    .fail(function (jqXHR, textStatus, err) {
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