﻿$(function() {
    var ViewModel = function() {
        var self = this;

        // set values
        self.rows = ko.observable(localStorage.getItem("rows"));
        self.cols = ko.observable(localStorage.getItem("cols"));
        self.searchAlgo = ko.observable(localStorage.getItem("searchAlgo"));
    }

    ko.applyBindings(new ViewModel());
});


// calls when start game button is pressed.
$("#startGame").click(function() {
    // loader
    document.getElementById("loader").style.display = "block";

    var name = $("#mazeName").val();
    if (name === "") {
        alert("Please enter a name");
        // disable loader
        document.getElementById("loader").style.display = "none";
        return;
    }
    var rows = $("#rows").val();
    var cols = $("#cols").val();

    // Ask maze from server
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "../api/SingleGame/GenerateMaze/" + name + "/" + rows + "/" + cols,
        success: function(recData) {
            // change title
            $("title").text(recData["Name"]);
            // call plugin
            var mazeStr = recData["Maze"];
            var mazeData = [recData["Rows"], recData["Cols"], recData["Maze"]];
            var myMazeBoard = $("#myCanvas").mazeBoard(
                mazeData,
                recData["Start"]["Row"],
                recData["Start"]["Col"],
                recData["End"]["Row"],
                recData["End"]["Col"],
                document.getElementById("playerImg"),
                document.getElementById("exitImg"),
                true,
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
        error: function(result) {
            alert(result.responseJSON["ExceptionMessage"]);
            $("#mazeName").val("");
        }
    });
    // disable loader
    document.getElementById("loader").style.display = "none";
});