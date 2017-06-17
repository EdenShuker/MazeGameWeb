var navbar =
    " <nav id='navbar' class='navbar navbar-inverse'>\
        <div class='container-fluid'>\
        <div class='navbar-header'><a class='navbar-brand' href='../index.html'>Maze</a></div>\
        <div id='mazeNavbar' class='navbar-collapse collapse'>\
            <ul class='nav navbar-nav'>\
                <li><a href='../Views/SinglePlayerPage.html'>Single Game</a></li>\
                <li><a href='../Views/MultiPlayerPage.html'>Multiplayer Game</a></li>\
                <li><a href='#'>Settings</a></li>\
                <li><a href='../Views/UserRankings.html'>User Rankings</a></li>\
            </ul>\
            <ul class='nav navbar-nav navbar-right'>\
                <li><a id='regLbl' href='../Views/Register.html'>Register</a></li>\
                <li><a id='logLbl' href='../Views/Login.html'>Login</a></li>\
            </ul>\
        </div>\
        <!--/.nav-collapse -->\
        </div>\
        \
    <!--/.container-fluid -->\
    </nav>";

var login = ">Login";
var logout = ">Logout";


$(function () {
    if (sessionStorage.user === undefined) {
        // change text to login
        navbar = navbar.replace(logout, login);
        $(".navigationbar").html(navbar);
    } else {
        // change text to logout
        navbar = navbar.replace(login, logout);
        $(".navigationbar").html(navbar);
        // say hello to user
        //$("#regLbl").innerHTML("Hello " + sessionStorage.getItem("user"));
        // apply logout
        $("#logLbl").on("click", function () {
            alert("clicked me finally");
            sessionStorage.removeItem('user');
            $("#logLbl").off("click");
            // change text back to login
            navbar = navbar.replace(logout, login);
            $(".navigationbar").html(navbar);
            // change text to register
            //$("#regLbl").innerHTML("Register");
            window.location.href = "../Views/Login.html";
        });
    }
});