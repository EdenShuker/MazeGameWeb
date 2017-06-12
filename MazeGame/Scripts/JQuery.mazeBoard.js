(function ($) {

    $.fn.myFunc = function () {
        alert("in the plugin first");
    }

    $.fn.mazeBoard = function (mazeData, startRow, startCol, exitRow, exitCol, playerImage, exitImage, isEnable, funcMove) {
        // todo: 'maze' object will be with the \r\n chars, 
        // todo: so --> rows = number of \r\n --> cols = number of chars in one row till \r\n
        alert("in the plugin");
        //var myCanvas = document.getElementById("mazeCanvas");
        var context = this[0].getContext("2d");
        var rows = mazeData[0];
        var cols = mazeData[1];
        var cellWidth = this[0].width / cols;
        var cellHeight = this[0].height / rows;
        for (var i = 0; i < rows; i++) {
            for (var j = 0; j < cols; j++) {
                if (mazeData[2][i * cols + j] === "1") {
                    context.fillRect(cellWidth * j,
                        cellHeight * i,
                        cellWidth,
                        cellHeight);
                }
            }
        }
        var playerImg = new Image();
        playerImg.onload = function () {
            context.drawImage(playerImg, startCol * cellWidth, startRow * cellHeight, cellWidth, cellHeight);
        }
        playerImg.src = "../Views/Images/minion.gif";

        var exitImg = new Image();
        exitImg.onload = function () {
            context.drawImage(exitImg, exitCol * cellWidth, exitRow * cellHeight, cellWidth, cellHeight);
        }
        exitImg.src = "../Views/Images/Exit.png";

        if (isEnable == true) {
            funcMove();
        }

        return this;
    };
}(jQuery));