$("#startGame").click(function() {
    var name = $("#mazeName").val();
    var rows = $("#rows").val();
    var cols = $("#cols").val();
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "../api/SingleGame/GenerateMaze/" + name + "/" + rows + "/" + cols,
        success: function(recData) {
            // init
            // call plugin
            var mazeData = [recData["Rows"], recData["Cols"], recData["Maze"]];
            var myMazeBoard = $("#myCanvas").mazeBoard(
                mazeData,
                recData["Start"]["Row"],
                recData["Start"]["Col"],
                recData["End"]["Row"],
                recData["End"]["Col"],
                "Images/minion.gif",
                "Images/Exit.png",
                true,
                /* idea: the func below will return the new pos 
                 * return true --> movement to the given direction is valid
                 * return false --> movement invalid
                 * so this func will be responsible for the logic
                 * and the plugin will be responsible for the draw (view)
                */
                function(direction, playerRow, playerCol) {
                    switch (direction) {
                    case 37:
                        // left
                        if (playerCol > 0 && mazeStr[playerRow * cols + playerCol - 1] !== "1") {
                            playerCol -= 1;
                        }
                        break;
                    case 38:
                        // up
                        if (playerRow > 0 && mazeStr[(playerRow - 1) * cols + playerCol] !== "1") {
                            playerRow -= 1;
                        }
                        break;
                    case 39:
                        // right
                        if (playerCol < cols - 1 && mazeStr[playerRow * cols + playerCol + 1] !== "1") {
                            playerCol += 1;
                        }
                        break;
                    case 40:
                        // down
                        if (playerRow < rows - 1 && mazeStr[(playerRow + 1) * cols + playerCol] !== "1") {
                            playerRow += 1;
                        }
                        break;
                    }
                    return [playerRow, playerCol];
                });
        },
        error: function(result) { alert("error " + result[0]); }
    });
});