$(document).ready(function () {
    //Al hacer click al select-list del estatus del colegio
    $("#select-estatus-colegio").change(function () {
        var seleccion = $(this).val();
    });

    //Al hacer click al select-list del estatus del período escolar
    $("#select-estatus-periodo").change(function () {
        var seleccion = $(this).val();
    });
});