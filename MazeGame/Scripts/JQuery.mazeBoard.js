var mazeStr;
var startRow, startCol;
var currentRow, currentCol;
var endRow, endCol;
var cellWidth, cellHeight;
var context;
var movePlayerFunc;
var playerImage;

(function($) {

    $.fn.mazeBoard = function(mazeData,
        initRow,
        initCol,
        exitRow,
        exitCol,
        playerImg,
        exitImg,
        isEnable,
        funcMove) {

        var canvas = this[0];
        context = this[0].getContext("2d");
        var rows = mazeData[0];
        var cols = mazeData[1];
        cellWidth = this.cellWidth = this[0].width / cols;
        cellHeight = this.cellHeight = this[0].height / rows;
        playerImage = this.playerImage = playerImg;

        // clear previous board
        context.clearRect(0, 0, canvas.width, canvas.height);

        // init vars
        mazeStr = mazeData[2];
        startRow = currentRow = initRow;
        startCol = currentCol = initCol;
        endRow = exitRow;
        endCol = exitCol;

        // draw squares
        context.fillStyle = "#000000";
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
        context.drawImage(playerImg, startCol * cellWidth, startRow * cellHeight, cellWidth, cellHeight);

        // draw exit image
        context.drawImage(exitImg, exitCol * cellWidth, exitRow * cellHeight, cellWidth, cellHeight);

        if (isEnable === true) {
            movePlayerFunc = function(e) {
                var newPosition = funcMove(e.which, currentRow, currentCol);
                if (currentRow !== newPosition[0] || currentCol !== newPosition[1]) {
                    // "delete" prev player-image
                    context.fillStyle = "#ffffff";
                    context.fillRect(cellWidth * currentCol, cellHeight * currentRow, cellWidth, cellHeight);
                    // draw the new one
                    currentRow = newPosition[0];
                    currentCol = newPosition[1];
                    context
                        .drawImage(playerImg, currentCol * cellWidth, currentRow * cellHeight, cellWidth, cellHeight);
                }
                // check for end of game
                window.setTimeout(function() {
                        if (currentRow === exitRow && currentCol === exitCol) {
                            // disable movement once reached the end
                            $("body").off("keydown", movePlayerFunc);
                            alert("You did it!\nFinally...");
                            return;
                        }
                    },
                    100);
            }

            $("body").keydown(movePlayerFunc);
        }

        return this;
    };
}(jQuery));