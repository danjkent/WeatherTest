$(document).ready(function () {
    $("#loading").hide();

    $("#manageTemperatureUoms").click(showCRUDDialog);
    $("#manageWindSpeedUoms").click(showCRUDDialog);
    $("#manageServices").click(showCRUDDialog);
});

function showCRUDDialog() {
    $("#crudDialog").modal();
}