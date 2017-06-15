var mazeStr;
var startRow, startCol;
var currentRow, currentCol;
var cellWidth, cellHeight;
var playerImg;
var context;

(function($) {

    $.fn.mazeBoard = function(mazeData,
        initRow,
        initCol,
        exitRow,
        exitCol,
        playerImagePath,
        exitImagePath,
        isEnable,
        funcMove) {
        context = this[0].getContext("2d");
        var rows = mazeData[0];
        var cols = mazeData[1];
        cellWidth = this[0].width / cols;
        cellHeight = this[0].height / rows;
        // init vars
        mazeStr = mazeData[2];
        startRow = currentRow = initRow;
        startCol = currentCol = initCol;
        // draw squares
        for (var i = 0; i < rows; i++) {
            for (var j = 0; j < cols; j++) {
                if (mazeStr[i * cols + j] === "1") {
                    context.fillRect(cellWidth * j,
                        cellHeight * i,
                        cellWidth,
                        cellHeight);
                }
            }
        }
        // draw player image
        playerImg = new Image();
        playerImg.onload = function() {
            context.drawImage(playerImg, initCol * cellWidth, initRow * cellHeight, cellWidth, cellHeight);
        }
        playerImg.src = playerImagePath;
        // draw exit image
        var exitImg = new Image();
        exitImg.onload = function() {
            context.drawImage(exitImg, exitCol * cellWidth, exitRow * cellHeight, cellWidth, cellHeight);
        }
        exitImg.src = exitImagePath;

        if (isEnable == true) {
            $("body").keydown(function updatePos(e) {
                var newPosition = funcMove(e.which, currentRow, currentCol);
                if (currentRow !== newPosition[0] || currentCol !== newPosition[1]) {
                    // "delete" prev player-image
                    context.fillStyle = "#ffffff";
                    context.fillRect(cellWidth * currentCol, cellHeight * currentRow, cellWidth, cellHeight);
                    // draw the new one
                    currentRow = newPosition[0];
                    currentCol = newPosition[1];
                    context.drawImage(playerImg, currentCol * cellWidth, currentRow * cellHeight, cellWidth, cellHeight);
                }
                // check for end of game
                window.setTimeout(function() {
                        if (currentRow === exitRow && currentCol === exitCol) {
                            // disable movement once reached the end
                            $("body").off("keydown", updatePos);
                            alert("You did it!\nFinally...");
                            return;
                        }
                    },
                    100);
            });
        }

        return this;
    };
}(jQuery));