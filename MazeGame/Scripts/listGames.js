
$('#dropdown-games').on('show.bs.dropdown', function () {
    var list = document.getElementById("dropdown");
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "../api/ListGames",
        success: function (recData) {
            var games = JsonConvert.DeserializeObject(recData);
           
        },
        error: function (result) { alert("error " + result[0]); }
    });

});