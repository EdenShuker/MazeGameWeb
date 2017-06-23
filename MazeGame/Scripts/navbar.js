var navbar =
    " <nav id='navbar'>\
    <div class='w3-bar w3-black w3-round-xlarge'>\
        <a class='w3-bar-item w3-button' href='../index.html'>Maze</a>\
        <a href='../Views/SinglePlayerPage.html' class='w3-bar-item w3-button'>Single Game</a>\
        <a id='multi' href='../Views/MultiPlayerPage.html' class='w3-bar-item w3-button'>Multiplayer Game</a>\
        <a href='../Views/Settings.html' class='w3-bar-item w3-button'>Settings</a>\
        <a href='../Views/UserRankings.html' class='w3-bar-item w3-button'>User Rankings</a>\
        <a id='logLbl' href='../Views/Login.html' class='w3-bar-item w3-button w3-right'>Login</a>\
        <a id='regLbl' href='../Views/Register.html' class='w3-bar-item w3-button w3-right'>Register</a>\
    </div >\
</nav >";

var login = ">Login";
var logout = ">Logout";


$(function() {
    if (sessionStorage.user === undefined) {
        // change text to login
        navbar = navbar.replace(logout, login);
        $(".navigationbar").html(navbar);
    } else {
        // change text to logout
        navbar = navbar.replace(login, logout);
        $(".navigationbar").html(navbar);
        // say hello to user
        document.getElementById("regLbl").innerHTML = "Hello " + sessionStorage.getItem("user");
        // stop href of register
        $("#regLbl").on("click", function() { return false });
        // apply logout
        $("#logLbl").on("click",
            function() {
                sessionStorage.removeItem('user');
                $("#logLbl").off("click");
                // change text back to login
                navbar = navbar.replace(logout, login);
                $(".navigationbar").html(navbar);
                // change text to register
                document.getElementById("regLbl").innerHTML = "Register";
                $("#regLbl").off("click");
                //$("#regLbl").innerHTML("Register");
                window.location.href = "../Views/Login.html";
            });
    }
});

$(function () {
    $("#multi").click(function () {
        if (sessionStorage.user === undefined) {
            // user is not logged in
            alert("In order to play multiplayer-game you need to login first");
            window.location.href = "../Views/Login.html";
            // prevent href
            return false;
        }
        return true;
    });
});