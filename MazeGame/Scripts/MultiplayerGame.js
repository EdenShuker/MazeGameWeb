// Declare a proxy to reference the hub
var multiGame = $.connection.gameHub;

// vars for tracking competitor
var competitorContext;
var competitorImg;
var competitorRow, competitorCol;
var cellWidth, cellHeight;

// set the function to draw a board
multiGame.client.drawBoard = function(canvasName,
    maze,
    playerImagePath,
    exitImagePath,
    isEnable) {

    // call plugin
    var mazeData = [maze.Rows, maze.Cols, maze.ToString()];
    var board = $("#" + canvasName).mazeBoard(mazeData,
        maze.InitialPos.Row,
        maze.InitialPos.Col,
        maze.GoalPos.Row,
        maze.GoalPos.Col,
        playerImagePath,
        exitImagePath,
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
                multiGame.server.Play(direction);
            }
            return [playerRow, playerCol];
        });
    if (!isEnable) {
        // init competitor vars
        competitorRow = maze.InitialPos.Row;
        competitorCol = maze.InitialPos.Col;
        cellWidth = board[0].cellWidth;
        cellHeight = board[0].cellHeight;
        competitorContext = board[0].context;
        competitorImg = board[0].playerImg;
    }
};

// function for updating competitor's position
multiGame.client.moveOtherPlayer = function(direction) {
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

// set button-click of start-new-game
$("#startGame").click(function() {
    $.connection.hub.start().done(function() {
        // extract info
        var name = $("#mazeName").val();
        var rows = $("#rows").val();
        var cols = $("#cols").val();

        // start new game
        multiGame.StartGame(name, rows, cols);
    });
});

// set button-click of join-game
$("#joinGame").click(function() {
    // join to game
    var name = $("#dropdown").val();
    multiGame.server.JoinTo(name);
});

// todo: fill method
multiGame.client.closeGame = function() {
    alert("close");
}