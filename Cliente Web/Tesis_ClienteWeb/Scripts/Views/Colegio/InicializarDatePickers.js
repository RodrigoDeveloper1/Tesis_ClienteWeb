function CalculoDiasLapso1() {
    if ($("#fec-ini-1").val() != '' && $("#fec-fin-1").val() != '') {
        var diff = ($("#fec-fin-1").datepicker("getDate") -
            $("#fec-ini-1").datepicker("getDate")) / 1000 / 60 / 60 / 24;

        $('#nro-dias-lapso-1').val(diff);
    }
}
function CalculoDiasLapso2() {
    if ($("#fec-ini-2").val() != '' && $("#fec-fin-2").val() != '') {
        var diff = ($("#fec-fin-2").datepicker("getDate") -
            $("#fec-ini-2").datepicker("getDate")) / 1000 / 60 / 60 / 24;

        $('#nro-dias-lapso-2').val(diff);
    }
}
function CalculoDiasLapso3() {
    if ($("#fec-ini-3").val() != '' && $("#fec-fin-3").val() != '') {
        var diff = ($("#fec-fin-3").datepicker("getDate") -
            $("#fec-ini-3").datepicker("getDate")) / 1000 / 60 / 60 / 24;

        $('#nro-dias-lapso-3').val(diff);
    }
}

$(document).ready(function () {
    //Para vaciar los campos
    $('#fecha-inicio').val("");
    $('#fecha-finalizacion').val("");
    $('#fec-ini-1').val("");
    $('#fec-ini-2').val("");
    $('#fec-ini-3').val("");
    $('#fec-fin-1').val("");
    $('#fec-fin-2').val("");
    $('#fec-fin-3').val("");

    $('#fecha-inicio').prop('readonly', true)
    $('#fecha-finalizacion').prop('readonly', true)

    $('#fecha-inicio').datepicker({
        beforeShowDay: $.datepicker.noWeekends
    });
    $('#fecha-finalizacion').datepicker({
        beforeShowDay: $.datepicker.noWeekends
    });

    $('#fec-ini-1').datepicker({
        beforeShowDay: $.datepicker.noWeekends,
        onSelect: function (dateText, inst) {            
            CalculoDiasLapso1();
        }
    });
    $('#fec-ini-2').datepicker({
        beforeShowDay: $.datepicker.noWeekends,
        onSelect: function (dateText, inst) {
            CalculoDiasLapso1();
        }
    });
    $('#fec-ini-3').datepicker({
        beforeShowDay: $.datepicker.noWeekends,
        onSelect: function (dateText, inst) {
            CalculoDiasLapso1();
        }
    });

    $('#fec-fin-1').datepicker({
        beforeShowDay: $.datepicker.noWeekends,
        onSelect: function (dateText, inst) {
            CalculoDiasLapso1();
        }
    });
    $('#fec-fin-2').datepicker({
        beforeShowDay: $.datepicker.noWeekends,
        onSelect: function (dateText, inst) {
            CalculoDiasLapso2();
        }
    });
    $('#fec-fin-3').datepicker({
        beforeShowDay: $.datepicker.noWeekends,
        onSelect: function (dateText, inst) {
            CalculoDiasLapso3();
        }
    });
});