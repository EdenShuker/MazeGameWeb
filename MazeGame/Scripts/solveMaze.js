$("#solveGame").click(function() {
    var name = $("#mazeName").val();
    var algo = 0; // todo: get val from drop-down

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "../api/SingleGame/SolveMaze/" + name + "/" + algo,
        success: function(recData) {
            var sol = recData["Solution"];
            // "delete" prev player-image
            context.fillStyle = "#ffffff";
            context.fillRect(cellWidth * currentCol, cellHeight * currentRow, cellWidth, cellHeight);
            // draw image back to the start
            context.drawImage(playerImg, startCol * cellWidth, startRow * cellHeight, cellWidth, cellHeight);
            currentRow = startRow;
            currentCol = startCol;
            sol.split("").forEach(function(c) {
                window.setTimeout(function() {
                    switch (c) {
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
                },
                300);
            });
        },
        error: function(result) { alert("error " + result[0]); }
    });
});