var navbar =
    " <nav class='navbar navbar-inverse'><div class='container-fluid'><div class='navbar-header'><a class='navbar-brand' href='../index.html'>Maze</a></div><div id='mazeNavbar' class='navbar-collapse collapse'><ul class='nav navbar-nav'><li><a href='../Views/SinglePlayerPage.html'>Single Game</a></li><li><a href='#'>Multiplayer Game</a></li><li><a href='#'>Settings</a></li><li><a href='../Views/UserRankings.html'>User Rankings</a></li></ul><ul class='nav navbar-nav navbar-right'><li><a href='../Views/Register.html'>Register</a></li><li><a href='../Views/Login.html'>Login</a></li></ul></div><!--/.nav-collapse --></div><!--/.container-fluid --></nav>"


$(function() {
    $(".navigationbar").html(navbar);

});