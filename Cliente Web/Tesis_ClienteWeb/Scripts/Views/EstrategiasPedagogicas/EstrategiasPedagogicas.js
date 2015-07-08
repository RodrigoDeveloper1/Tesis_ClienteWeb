var idCurso = "";
var idLapso = "";
var idMateria = "";

function GenerarReporte()
{
    swal({
        title: "¿Estás seguro?",
        text: "¿Seguro que deseas generar el reporte de estrategias pedagógicas?",
        type: "info",
        showCancelButton: true,
        confirmButtonText: "¡Generar!",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            showProgress();

            $.ajax({
                type: "POST",
                url: "/EstrategiasPedagogicas/EstrategiasPedagogicas_Reporte",
                dataType: "json",
                data: {
                    idCurso: idCurso,
                    idLapso: idLapso,
                    idMateria: idMateria,
                },
                success: function (data) {
                    //hideProgress();

                    if (data[0].Success) {
                        //window.location = '/Reportes/GenerarReporte/?path=' + data[0].Path;
                        //swal("¡Reporte generado!", "El reporte ha sido generado", "success");
                    }
                    else {
                        hideProgress();

                        swal({
                            title: " ¡Error!",
                            text: "Ha ocurrido un error mientras se generaba el reporte.",
                            type: "error",
                            closeOnConfirm: true,
                        }, function (isConfirm) {
                            if (isConfirm) {
                                showProgress();

                                window.location.href = 'EstrategiasPedagogicas';
                            }
                        });
                    }
                },
                error: function () {
                    hideProgress();

                    swal({
                        title: " ¡Error!",
                        text: "Ha ocurrido un error mientras se generaba el reporte",
                        type: "error",
                        closeOnConfirm: true,
                    }, function (isConfirm) {
                        if (isConfirm) {
                            showProgress();

                            window.location.href = 'EstrategiasPedagogicas';
                        }
                    });
                }
            });
        }
    });
}

$(document).ready(function () {
    $("#select-curso").change(function () {
        idCurso = $(this).val();

        if (idCurso != "") {
            showProgress();

            $("#select-lapso").find('option').remove().end().append("<option>Cargando lapsos...</option>");
            $("#select-lapso").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListLapsosProfesorEstrategias",
            {
                idCurso: idCurso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione el lapso...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idLapso + '">' + data[i].nombre + '</option>');
                    }

                    $("#select-lapso").find('option').remove().end().append(lista);
                    $("#select-lapso").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-lapso").find('option').remove().end()
                        .append('<option>No se encontraron lapso activos....</option>');
                    $("#select-lapso").selectpicker("refresh");

                    idCurso = "";

                    hideProgress();
                }
            });
        }
        else {
            $('#select-lapso').find('option').remove().end().append('<option>Seleccione el lapso...</option>');
            $("#select-lapso").selectpicker("refresh");

            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");

            idLapso = "";
            idMateria = "";
        }
    });
    $("#select-lapso").change(function () {
        idLapso = $(this).val();

        if (idLapso != "") {
            showProgress();

            $("#select-materia").find('option').remove().end().append("<option>Cargando materias...</option>");
            $("#select-materia").selectpicker("refresh");

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

                    $("#select-materia").find('option').remove().end().append(lista);
                    $("#select-materia").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-materia").find('option').remove().end()
                        .append('<option>No se encontraron materias activas....</option>');
                    $("#select-materia").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");

            idMateria = "";
        }
    });
    $("#select-materia").change(function () {
        idMateria = $(this).val();
    });

    $("#generar-reporte").click(function () {
        if (idMateria == "")
            swal("¡Oops!", "Seleccione todos los datos de las listas para generar el reporte.", "warning");
        else
            GenerarReporte();
    });
});