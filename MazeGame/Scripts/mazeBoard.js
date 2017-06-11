(function ($) {

    $.fn.drawMaze = function (maze,startRow, startCol, exitRow,
        exitCol,playerImage, exitImage, isEnable, funcMove) {
        var myCanvas = document.getElementById("mazeCanvas");
        var context = mazeCanvas.getContext("2d");
        var rows = maze.length;
        var cols = maze[0].length;
        var cellWidth = mazeCanvas.width / cols;
        var cellHeight = mazeCanvas.height / rows;
        for (var i = 0; i < rows; i++) {
            for (var j = 0; j < cols; j++) {
                if (maze[i][j] == 1) {
                    context.fillRect(cellWidth * j, cellHeight * i,
                   cellWidth, cellHeight);
                }
            }
        }
        var playerImg = new Image(cellWidth, cellHeight);
        playerImg.onload = function () {
            context.drawImage(playerImage, startRow, startCol);
        }
        playerImage.src = "../Views/Images/minion.gif";

        var playerImg = new Image(cellWidth, cellHeight);
        playerImg.onload = function () {
            context.drawImage(playerImage, exitRow, exitCol);
        }
        playerImage.src = "../Views/Images/Exit.gif";

        if (isEnable == true) {
            funcMove();
        }

        return this;
    };

}(jQuery));



