$(document).ready(function () {
    $("#btn-crear-ano-escolar").click(function () {
        showProgress();
    });

    //Cambio de estatus del año escolar
    $("#select-estatus-periodo").change(function () {
        if ($(this).val() != "") {
            if($(this).val() == "Activo") {
                $("#fec-ini-1").prop("disabled", false);
                $("#fec-ini-2").prop("disabled", false);
                $("#fec-ini-3").prop("disabled", false);
                $("#fec-fin-1").prop("disabled", false);
                $("#fec-fin-2").prop("disabled", false);
                $("#fec-fin-3").prop("disabled", false);
            }
            else {
                $("#fec-ini-1").prop("disabled", true);
                $("#fec-ini-2").prop("disabled", true);
                $("#fec-ini-3").prop("disabled", true);
                $("#fec-fin-1").prop("disabled", true);
                $("#fec-fin-2").prop("disabled", true);
                $("#fec-fin-3").prop("disabled", true);
            }
        }
        else {
            $("#fec-ini-1").prop("disabled", true);
            $("#fec-ini-2").prop("disabled", true);
            $("#fec-ini-3").prop("disabled", true);
            $("#fec-fin-1").prop("disabled", true);
            $("#fec-fin-2").prop("disabled", true);
            $("#fec-fin-3").prop("disabled", true);
        }
    });

    //Enable iCheck plugin for checkboxes
    //iCheck for checkbox and radio inputs
    //$('#div-tabla-lista-colegios input[type="checkbox"]').iCheck({
        //checkboxClass: 'icheckbox_flat-blue',
        //radioClass: 'iradio_flat-blue'
    //});
});