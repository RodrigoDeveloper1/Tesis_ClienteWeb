var idColegio;

$(document).ready(function () {
    $('.datepicker').datepicker({
        beforeShowDay: $.datepicker.noWeekends
    });

    $('#hora-inicio').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });

    $('#hora-finalizacion').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });

    $("#select-colegio-crear").change(function () {
        idColegio = $(this).val();        
    });
});