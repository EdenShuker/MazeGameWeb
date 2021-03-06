﻿(function() {
    if (sessionStorage.user === undefined) {
        // user is not logged in
        alert("In order to play multiplayer-game you need to login first");
        window.location.href = "../Views/Login.html";
    }
})();

$(function() {
    var ViewModel = function() {
        var self = this;

        // set values
        self.rows = ko.observable(localStorage.getItem("rows"));
        self.cols = ko.observable(localStorage.getItem("cols"));
    }

    ko.applyBindings(new ViewModel());
});

$(function() {
    // Declare a proxy to reference the hub
    var multiGame = $.connection.gameHub;
    var isConnStart = false;

    // vars for tracking competitor
    var competitorContext;
    var competitorImg;
    var competitorRow, competitorCol;
    var cellWidth, cellHeight;

    // set the function to draw a board
    multiGame.client.drawBoard = function(canvasName,
        recData,
        playerImgId,
        exitImgId,
        isEnable) {

        recData = JSON.parse(recData);
        // call plugin
        var rows = recData["Rows"], cols = recData["Cols"];
        var mazeStr = recData["Maze"];
        var mazeData = [rows, cols, recData["Maze"]];
        var board = $("#" + canvasName).mazeBoard(mazeData,
            recData["Start"]["Row"],
            recData["Start"]["Col"],
            recData["End"]["Row"],
            recData["End"]["Col"],
            document.getElementById(playerImgId),
            document.getElementById(exitImgId),
            isEnable,
            function(direction, playerRow, playerCol) {
                var isNewMove = false;
                switch (direction) {
                case 37:
                    // left
                    if (playerCol > 0 && mazeStr[playerRow * cols + playerCol - 1] !== "1") {
                        playerCol -= 1;
                        isNewMove = true;
                    }
                    break;
                case 38:
                    // up
                    if (playerRow > 0 && mazeStr[(playerRow - 1) * cols + playerCol] !== "1") {
                        playerRow -= 1;
                        isNewMove = true;
                    }
                    break;
                case 39:
                    // right
                    if (playerCol < cols - 1 && mazeStr[playerRow * cols + playerCol + 1] !== "1") {
                        playerCol += 1;
                        isNewMove = true;
                    }
                    break;
                case 40:
                    // down
                    if (playerRow < rows - 1 && mazeStr[(playerRow + 1) * cols + playerCol] !== "1") {
                        playerRow += 1;
                        isNewMove = true;
                    }
                    break;
                }
                if (isNewMove) {
                    // if valid move --> notify
                    multiGame.server.play(direction);
                }
                return [playerRow, playerCol];
            });
        if (!isEnable) {
            // init competitor vars
            var compCanvas = document.getElementById(canvasName);
            competitorRow = compCanvas.startRow;
            competitorCol = compCanvas.startCol;
            cellWidth = compCanvas.cellWidth;
            cellHeight = compCanvas.cellHeight;
            competitorContext = compCanvas.context;
            competitorImg = compCanvas.playerImg;
        }
    };

    // function for updating competitor's position
    multiGame.client.moveOtherPlayer = function(direction) {
        // clear previous image
        competitorContext.fillStyle = "#ffffff";
        competitorContext.fillRect(cellWidth * competitorCol, cellHeight * competitorRow, cellWidth, cellHeight);

        // change position of competitor
        switch (direction) {
        case "37":
            // left
            competitorCol -= 1;
            break;
        case "38":
            // up
            competitorRow -= 1;
            break;
        case "39":
            // right
            competitorCol += 1;
            break;
        case "40":
            // down
            competitorRow += 1;
            break;
        }

        // update on screen
        competitorContext.drawImage(competitorImg,
            competitorCol * cellWidth,
            competitorRow * cellHeight,
            cellWidth,
            cellHeight);
    };

    multiGame.client.presentAvailableGames = function(games) {
        var i = 0;
        var dropdown = $("#dropdown").empty();
        dropdown.append("<option value='' selected disabled>Games &dArr;</option>");
        games.forEach(function(game) {
            dropdown.append("<option value=" + i + ">" + game + "</option>");
        });
    };

    multiGame.client.closeGame = function(isWon) {
        var api = "../api/Users/";
        var user = { Name: sessionStorage.getItem("user") };
        var errFunc = function() {
            alert("Could not update the server with your score");
        };
        if (!isWon) {
            // update db
            $.ajax({
                url: api + "lose",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(user),
                success: function() {
                    // disable move
                    $("body").off("keydown", $("#myCanvas").movePlayerFunc);
                    // change title
                    $("title").text("Loser");
                    // notify
                    alert("You lost...");
                },
                error: errFunc
            });
        } else {
            // update db
            $.ajax({
                url: api + "win",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(user),
                success: function() {
                    // change title
                    $("title").text("Winner!");
                },
                error: errFunc
            });
        }
    };

    multiGame.client.gameStarted = function() {
        document.getElementById("loader").style.display = "none";
    }

    $(document).ready(function() {
        // set button-click of start-new-game
        $("#startGame").click(function() {
            // loader
            document.getElementById("loader").style.display = "block";

            // extract info
            var name = $("#mazeName").val();
            var rows = $("#rows").val();
            var cols = $("#cols").val();

            // change title
            $("title").text(name);

            if (!isConnStart) {
                isConnStart = true;
                $.connection.hub.start().done(function() {
                    // start new game
                    multiGame.server.startGame(name, rows, cols);
                }).fail(function() {
                    alert("Could not start connection with the server");
                });
            } else {
                multiGame.server.startGame(name, rows, cols);
            }
        });

        // set button-click of join-game
        $("#joinGame").click(function() {
            if (isConnStart) {
                // loader
                document.getElementById("loader").style.display = "block";

                // join to game
                var dropdown = document.getElementById("dropdown");
                var name = dropdown.options[dropdown.selectedIndex].text;
                // change title
                $("title").text(name);
                multiGame.server.joinTo(name);

                // disable loader
                document.getElementById("loader").style.display = "none";
            } else {
                alert("Choose a game from the list first");
            }
        });

        // drop down game list
        $('#dropdown-games').on('show.bs.dropdown',
            function() {
                if (!isConnStart) {
                    isConnStart = true;
                    $.connection.hub.start().done(function() {
                        multiGame.server.getAvailablesGame();
                    }).fail(function() {
                        alert("Could not start connection with the server");
                    });
                } else {
                    multiGame.server.getAvailablesGame();
                }
            });
    });
});