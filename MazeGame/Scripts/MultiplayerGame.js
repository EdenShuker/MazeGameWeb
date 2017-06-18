var multiGame = $.connection.gameHub;

var competitorContext;
var competitorImg;
var competitorRow, competitorCol;
var cellWidth, cellHeight;

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
    competitorContext.drawImage(competitorImg, competitorCol * cellWidth, competitorRow * cellHeight, cellWidth, cellHeight);
};

(function() {
    $("#startGame").click(function () {
        $.connection.start().done(function() {
            // extract info
            var name = $("#mazeName").val();
            var rows = $("#rows").val();
            var cols = $("#cols").val();

            // start new game
            var mazeJson = multiGame.StartGame(name, rows, cols);
            // call plugin
            var mazeData = [mazeJson["Rows"], mazeJson["Cols"], mazeJson["Maze"]];
            var myMazeBoard = $("#myCanvas").mazeBoard(
                mazeData,
                mazeJson["Start"]["Row"],
                mazeJson["Start"]["Col"],
                mazeJson["End"]["Row"],
                mazeJson["End"]["Col"],
                "Images/minion.gif",
                "Images/Exit.png",
                true,
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
                        multiGame.server.Play(direction);
                    }
                    return [playerRow, playerCol];
                });
        });
    });
    drawCompetitorBoard();
})();

(function() {
    $("#joinGame").click(function () {
        // join to game
        var name = $("#dropdown").val();
        var mazeJson = multiGame.server.JoinTo(name);
        // call plugin
        var mazeData = [mazeJson["Rows"], mazeJson["Cols"], mazeJson["Maze"]];
        var myMazeBoard = $("#myCanvas").mazeBoard(
            mazeData,
            mazeJson["Start"]["Row"],
            mazeJson["Start"]["Col"],
            mazeJson["End"]["Row"],
            mazeJson["End"]["Col"],
            "Images/minion.gif",
            "Images/Exit.png",
            true,
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
                    multiGame.server.Play(direction);
                }
                return [playerRow, playerCol];
            });
    });
    drawCompetitorBoard();
})();

var drawCompetitorBoard = function(mazeJson) {
    // call plugin
    var mazeData = [mazeJson["Rows"], mazeJson["Cols"], mazeJson["Maze"]];
    var competitorBoard = $("#competitorCanvas").mazeBoard(
        mazeData,
        mazeJson["Start"]["Row"],
        mazeJson["Start"]["Col"],
        mazeJson["End"]["Row"],
        mazeJson["End"]["Col"],
        "Images/pokemon.gif",
        "Images/Exit.png");
    // init competitor vars
    competitorRow = mazeJson["Start"]["Row"];
    competitorCol = mazeJson["End"]["Col"];
    cellWidth = competitorBoard[0].cellWidth;
    cellHeight = competitorBoard[0].cellHeight;
    competitorContext = competitorBoard[0].context;
    competitorImg = competitorBoard[0].playerImg;
}

multiGame.client.closeGame = function() {

};