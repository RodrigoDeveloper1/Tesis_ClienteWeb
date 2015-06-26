var idColegio = "";
var idAnoEscolar = "";

$(document).ready(function () {
    $('.datepicker').datepicker({ beforeShowDay: $.datepicker.noWeekends });
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
        $("#ano-escolar").val("Cargando el año escolar");

        if (idColegio != "") {
            showProgress();

            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerAnoEscolarActivoEnLabel",
                data: {
                    idColegio: idColegio
                },
                success: function (data) {
                    if (data[0].success) {
                        $("#ano-escolar").val(data[0].label);

                        idAnoEscolarActivo = data[0].idAnoEscolar;
                        idAnoEscolar = data[0].idAnoEscolar;
                    }
                    else {
                        //Limpiando el text-box del año escolar
                        $("#ano-escolar").val("No posee año escolar activo");
                    }

                    hideProgress();
                }
            });
        }
        else {
            //Limpiando el text-box del año escolar
            $("#ano-escolar").val("");
        }
    });
});