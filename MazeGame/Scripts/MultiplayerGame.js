﻿// Declare a proxy to reference the hub
var multiGame = $.connection.gameHub;

// vars for tracking competitor
var competitorContext;
var competitorImg;
var competitorRow, competitorCol;
var cellWidth, cellHeight;
var isConnStart = false;

// set the function to draw a board
multiGame.client.drawBoard = function (canvasName,
    recData,
    playerImgId,
    exitImgId,
    isEnable) {

    recData = JSON.parse(recData);
    // call plugin
    var rows = recData["Rows"], cols = recData["Cols"];
    var mazeData = [rows, cols, recData["Maze"]];
    var board = $("#" + canvasName).mazeBoard(mazeData,
        recData["Start"]["Row"],
        recData["Start"]["Col"],
        recData["End"]["Row"],
        recData["End"]["Col"],
        document.getElementById(playerImgId),
        document.getElementById(exitImgId),
        isEnable,
        function (direction, playerRow, playerCol) {
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
        competitorRow = recData["Start"]["Row"];
        competitorCol = recData["Start"]["Col"];
        cellWidth = board.cellWidth;
        cellHeight = board.cellHeight;
        competitorContext = board[0].getContext("2d");
        competitorImg = board.playerImage;
    }
};

// function for updating competitor's position
multiGame.client.moveOtherPlayer = function (direction) {
    // clear previous image
    competitorContext.fillStyle = "#ffffff";
    competitorContext.fillRect(cellWidth * competitorCol, cellHeight * competitorRow, cellWidth, cellHeight);

    // change position of competitor
    switch (direction) {
        case 37:
            // left
            competitorCol -= 1;
            break;
        case 38:
            // up
            competitorRow -= 1;
            break;
        case 39:
            // right
            competitorCol += 1;
            break;
        case 40:
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

multiGame.client.presentAvailableGames = function (games) {
    var i = 0;
    var dropdown = $("#dropdown").empty();
    games.forEach(function (game) {
        dropdown.append("<option value=" + i + ">" + game + "</option>");
    });
};

// todo: fill method
multiGame.client.closeGame = function () {
    alert("close");
};


$(document).ready(function () {
    // set button-click of start-new-game
    $("#startGame").click(function () {
        // extract info
        var name = $("#mazeName").val();
        var rows = $("#rows").val();
        var cols = $("#cols").val();
        if (!isConnStart) {
            isConnStart = true;
            $.connection.hub.start().done(function () {
                // start new game
                multiGame.server.startGame(name, rows, cols);
            });
        } else {
            multiGame.server.startGame(name, rows, cols);
        }
    });

    // set button-click of join-game
    $("#joinGame").click(function () {
        // join to game
        var name = $("#dropdown").text();
        multiGame.server.joinTo(name);
    });

    // drop down game list
    $('#dropdown-games').on('show.bs.dropdown',
        function () {
            if (!isConnStart) {
                isConnStart = true;
                $.connection.hub.start().done(function () {
                    multiGame.server.getAvailablesGame();
                });
            } else {
                multiGame.server.getAvailablesGame();
            }
        });
});


