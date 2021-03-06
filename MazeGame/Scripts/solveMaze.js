﻿
// calls when solve game button is pressed.
$("#solveGame").click(function () {
    var name = $("#mazeName").val();
    var algo = $("#searchAlgo").val();

    // ask for solution from server.
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "../api/SingleGame/SolveMaze/" + name + "/" + algo,
        success: function(recData) {
            var sol = recData["Solution"];

            // init vars
            var canvasToSolve = document.getElementById("myCanvas");
            var context = canvasToSolve.context;
            var currentRow = canvasToSolve.currentRow, currentCol = canvasToSolve.currentCol;
            var startRow = canvasToSolve.startRow, startCol = canvasToSolve.startCol;
            var endRow = canvasToSolve.endRow, endCol = canvasToSolve.endCol;
            var cellWidth = canvasToSolve.cellWidth, cellHeight = canvasToSolve.cellHeight;
            var playerImg = canvasToSolve.playerImg;

            // "delete" prev player-image
            context.fillStyle = "#ffffff";
            context.fillRect(cellWidth * currentCol, cellHeight * currentRow, cellWidth, cellHeight);
            // draw image back to the start
            context.drawImage(playerImg, startCol * cellWidth, startRow * cellHeight, cellWidth, cellHeight);
            currentRow = startRow;
            currentCol = startCol;
            // disable movement
            $("body").off();
            // start showing solution
            var i = 0;
            sol = sol.split("");
            var intervalId = window.setInterval(function () {
                // check for end of solution
                if (currentRow === endRow && currentCol === endCol) {
                    clearInterval(intervalId);
                    alert("Reached the end!\nTry next time but without being lazy...");
                }
                // "delete" prev player-image
                context.fillStyle = "#ffffff";
                context.fillRect(cellWidth * currentCol, cellHeight * currentRow, cellWidth, cellHeight);
                switch (sol[i]) {
                    case "0":
                        // left
                        currentCol -= 1;
                        break;
                    case "1":
                        // right
                        currentCol += 1;
                        break;
                    case "2":
                        // up
                        currentRow -= 1;
                        break;
                    case "3":
                        // down
                        currentRow += 1;
                        break;
                    default:
                        break;
                }
                context.drawImage(playerImg, currentCol * cellWidth, currentRow * cellHeight, cellWidth, cellHeight);
                i++;
            }, 400);
        },
        error: function(result) { alert("error " + result[0]); }
    });
});