var idLapso = "";
var idCurso = "";
var idMateria = "";
var UltimoNroEvaluacion;

$(document).ready(function () {
    $("#btn-agregar").click(function () {
        showProgress();
    });

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

    $("#select-curso-crear").change(function () {
        idCurso = $(this).val();
        if (idCurso != "") {
            showProgress();

            $("#select-lapso-crear").find('option').remove().end().append("<option>Cargando lapsos...</option>");
            $("#select-lapso-crear").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListLapsosProfesor",
            {
                idCurso: idCurso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione el lapso...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idLapso + '">' + data[i].nombre + '</option>');
                    }
                    $("#select-lapso-crear").find('option').remove().end().append(lista);
                    $("#select-lapso-crear").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-lapso-crear").find('option').remove().end()
                        .append('<option>No se encontraron lapso activos....</option>');
                    $("#select-lapso-crear").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            idCurso = "";
            idLapso = "";
            idMateria = "";

            $('#select-lapso-crear').find('option').remove().end().append('<option>Seleccione el lapso...</option>');
            $("#select-lapso-crear").selectpicker("refresh");

            $('#select-materia-crear').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia-crear").selectpicker("refresh");
        }
    });
    $("#select-lapso-crear").change(function () {
        idLapso = $(this).val();

        if (idLapso != "") {
            showProgress();

            $("#select-materia-crear").find('option').remove().end().append("<option>Cargando materias...</option>");
            $("#select-materia-crear").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListMaterias",
            {
                idCurso: idCurso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione la materia...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idMateria + '">' + data[i].nombre + '</option>');
                    }

                    $("#select-materia-crear").find('option').remove().end().append(lista);
                    $("#select-materia-crear").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-materia-crear").find('option').remove().end().append('<option>No se encontraron materias activas....</option>');
                    $("#select-materia-crear").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            idLapso = "";
            idMateria = "";

            $('#select-materia-crear').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia-crear").selectpicker("refresh");
        }
    });
    $("#select-materia-crear").change(function () {
        idMateria = $(this).val();
    });
});