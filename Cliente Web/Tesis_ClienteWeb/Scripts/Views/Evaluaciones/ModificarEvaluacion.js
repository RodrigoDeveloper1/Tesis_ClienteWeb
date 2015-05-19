var idColegio = "";
var idLapso = "";
var idCurso = "";
var idMateria = "";
var idProfesor = "";
var idEvaluacion = "";

// Selectlist de curso, lapso, materia, profesor
function CargarCursos() {
    showProgress();

    //Limpiando la lista de cursos
    $("#select-curso-modif").find('option').remove().end().append("<option>Cargando cursos...</option>");
    $("#select-curso-modif").selectpicker("refresh");

    $.post("/Bridge/ObtenerSelectListCursos",
        {
            idLapso: idLapso
        },
        function (data) {
            if (data != null && data.length > 0) {
                var lista = '<option value="">Seleccione el curso...</option>';

                for (var i = 0; i < data.length; i++) {
                    lista += ('<option value="' + data[i].idCurso + '">' + data[i].nombre + '</option>');
                }

                $("#select-curso-modif").find('option').remove().end().append(lista);
                $("#select-curso-modif").selectpicker("refresh");

                hideProgress();
            }
            else {
                $("#select-curso-modif").find('option').remove().end().append('<option>No se encontraron cursos activos....</option>');
                $("#select-curso-modif").selectpicker("refresh");

                hideProgress();
            }
        })
};
function CargarLapsos() {
    $("#select-lapso-modif").find('option').remove().end().append("<option>Cargando lapsos...</option>");
    $("#select-lapso-modif").selectpicker("refresh");

    showProgress();
    $.post("/Bridge/ObtenerSelectListLapsos",
        {
            idColegio: idColegio
        },
        function (data) {
            if (data != null && data.length > 0) {
                var lista = '<option value="">Seleccione el lapso...</option>';

                for (var i = 0; i < data.length; i++) {
                    lista += ('<option value="' + data[i].idLapso + '">' + data[i].nombre + '</option>');
                }

                $("#select-lapso-modif").find('option').remove().end().append(lista);
                $("#select-lapso-modif").selectpicker("refresh");
                hideProgress();
            }
            else {
                hideProgress();
                swal("¡Oops!", "No hay lapsos asociados", "warning");

                $("#select-lapso-modif").find('option').remove().end()
                    .append('<option>No se encontraron lapsos activos....</option>');
                $("#select-lapso-modif").selectpicker("refresh");
            }
        })

}
function CargarMaterias() {
    showProgress();

    //Limpiando la lista de materias
    $("#select-materia-modif").find('option').remove().end().append("<option>Cargando materias...</option>");
    $("#select-materia-modif").selectpicker("refresh");

    $.post("/Bridge/ObtenerJsonMateriasYDocentes",
    {
        idLapso: idLapso,
        idCurso: idCurso
    },
    function (data) {
        if (data[0].success) {
            var lista = '<option value="">Seleccione la materia...</option>';

            for (var i = 0; i < data.length; i++) {
                lista += (
                    '<option value="' + data[i].idMateria + "_" + data[i].idDocente + '">' +
                        data[i].materia + " - " + data[i].docente +
                    '</option>');
            }

            $("#select-materia-modif").find('option').remove().end().append(lista);
            $("#select-materia-modif").selectpicker("refresh");

            hideProgress();
        }
        else {
            swal("¡No hay materias!", "No hay materias asociadas a este curso", "warning");

            $("#select-materia-modif").find('option').remove().end()
                .append('<option>No se encontraron materias activas....</option>');
            $("#select-materia-modif").selectpicker("refresh");

            hideProgress();
        }
    });
}
function CargarTablaEvaluaciones() {
    showProgress();

    var lista = "";    
    $('#table-lista-evaluaciones').find('tbody').find('tr').remove();
    $.post("/Bridge/ObtenerTablaModificarEvaluaciones",
    {
        idMateria: idMateria,
        idCurso: idCurso,
        idProfesor: idProfesor,
        idLapso: idLapso,
        idColegio: idColegio
    },
    function (data) {
        if (data != null && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                lista += (
                    '<tr id=' + data[i].idevaluacion + ' >' +
                        '<td class="td-evaluacion">' + data[i].nombre + '</td>' +
                        '<td class="td-tecnica">' + data[i].tecnica + '</td>' +
                        '<td class="td-tipo">' + data[i].actividad + '</td>' +
                        '<td class="td-instrumento">' + data[i].instrumento + '</td>' +
                        '<td class="td-porcentaje">' + data[i].porcentaje + '</td>' +
                        '<td class="td-opcion">' + data[i].fechainicio + '</td>' +
                        '<td class="td-opcion">' + data[i].fechafin + '</td>' +
                        '<td class=td-eliminar id=' + data[i].idevaluacion + '>' +
                            '<a class= "fa fa-ban" ></a>' +
                        '</td>' +
                    '</tr>');
            }
            $('#table-lista-evaluaciones').find('tbody').end().append(lista);
            hideProgress();
        }
        else {
            hideProgress();
            $('#table-lista-evaluaciones').find('tbody').find('tr').remove();
            swal("¡No hay evaluaciones!", "Esta materia no posee evaluaciones asociadas", "warning");
        }
    });
}
function ModificarEvaluacion() {
    var name = $("#nombre-evaluacion").val();
    var porcentaje = $("#porcentaje-evaluacion").val();
    var fechainicio = $("#fechainicio-evaluacion").val();
    var fechafin = $("#fechafin-evaluacion").val();
    var horainicio = $("#horainicio-evaluacion").val();
    var horafin = $("#horafin-evaluacion").val();
    var tipoevaluacion = $("#select-tipo option:selected").html();
    var tecnicaevaluacion = $("#tecnica-evaluacion option:selected").html();
    var instrumentoevaluacion = $("#instrumento-evaluacion option:selected").html();
    var idCurso = $("#select-curso-modif option:selected").val();
    var idPeriodo = $("#select-lapso-modif option:selected").val();

    if (name == "" || name == null)
        swal("¡Falta el nombre!", "Por favor especifique el nombre de la evaluación", "warning");
    else if (porcentaje == "" || porcentaje == 0 || porcentaje == null)
        swal("¡Falta el porcentaje!", "Por favor especifique el porcentaje asignado a la evaluación", "warning");
    else
    {
        showProgress();
        $.ajax({
            url: "/Evaluaciones/ModificarEvaluacionCreada",
            type: "POST",
            data: {
                "Name": name,
                "Percentage": porcentaje,
                "StartDate": fechainicio,
                "FinishDate": fechafin,
                "StartHour": horainicio,
                "EndHour": horafin,
                "Activity": tipoevaluacion,
                "Technique": tecnicaevaluacion,
                "Instrument": instrumentoevaluacion,
                "AssessmentId": idEvaluacion,
                "CourseId": idCurso,
                "PeriodId": idPeriodo,
                "SubjectId": idMateria,
                "UserId": idProfesor
            }
        }).done(function () {
            hideProgress();
            window.location.href = 'ModificarEvaluacion';
        });
    }
}

$(document).ready(function () {
    $('.datepicker').datepicker({
        beforeShowDay: $.datepicker.noWeekends
    });

    $('#horainicio-evaluacion').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });

    $('#horafin-evaluacion').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });

    $("#select-colegio-modif").change(function () {
        idColegio = $(this).val();

        if (idColegio != "") {
            CargarLapsos();
        }
    });
    $("#select-lapso-modif").change(function () {
        idLapso = $(this).val();

        if (idColegio != "" && idLapso != "") {
            CargarCursos();
        }
    });
    $("#select-curso-modif").change(function () {
        idCurso = $(this).val();

        if (idColegio != "" && idLapso != "" && idCurso != "") 
            CargarMaterias();        
    });
    $("#select-materia-modif").change(function () {
        var auxId = $(this).val();
        var arrayId = auxId.split("_");
        idMateria = arrayId[0];
        idProfesor = arrayId[1];


        if (idColegio != "" && idLapso != "" && idCurso != "" && idMateria != "" && idProfesor != "")
            CargarTablaEvaluaciones();        
    });

    //Acción de eliminar
    $('#table-lista-evaluaciones').on('click', 'td.td-eliminar', function (e) {
        var selectedId = $(this).attr('id');

        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar esta evaluación!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórrala!",
            cancelButtonText: "¡No, cancelala!",
            closeOnConfirm: true,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                showProgress();

                $.ajax({
                    type: "POST",
                    url: "/Evaluaciones/EliminarEvaluacion",
                    data: {
                        id: selectedId
                    },
                    success: function () {
                        swal({
                            title: "¡Borrado!",
                            text: "Su evaluación ha sido borrada.",
                            type: "success",
                            showCancelButton: false,
                            closeOnConfirm: true,
                        },
                        function (isConfirm) {
                            window.location.href = 'ModificarEvaluacion';
                        });
                    },
                    error: function () {
                        swal({
                            title: "¡Error!",
                            text: "Ha ocurrido un error que ha impedido que se elimine la evaluación.",
                            type: "error",
                            showCancelButton: false,
                            closeOnConfirm: true,
                        },
                        function (isConfirm) {
                            window.location.href = 'ModificarEvaluacion';
                        });
                    }
                });
            }
        });
    });

    $('#table-lista-evaluaciones').on('click', 'tr', function (e) {
        var state = $(this).hasClass('active');
        $('.active').removeClass('active');

        if (!state) {
            $(this).addClass('active');
        }

        var nombre = "";
        var percentage = "";
        var startdate = "";
        var finishdate = "";
        var starthour = "";
        var endhour = "";
        var activity = "";
        var technique = "";
        var instrument = "";
        idEvaluacion = $(this).attr('id');

        if ($(this).attr('id') != "") {

            showProgress();

            $('#nombre-evaluacion-div').find("input").remove();
            $('#porcentaje-evaluacion-div').find("input").remove();
            $('#select-tipo').find('option:selected').remove();
            $('#tecnica-evaluacion').find('option:selected').remove();
            $('#instrumento-evaluacion').find('option:selected').remove();


            $.post("/Bridge/ObtenerDatosModificarEvaluacion",
            {
                idEvaluacion: idEvaluacion

            },
            function (data) {

                if (data != null && data.length > 0) {

                    nombre += ('<input id ="nombre-evaluacion" class="form-control"' +
                    ' placeholder ="Nombre de la evaluación" value="' + data[0].nombre + '">')

                    percentage += ('<input id ="porcentaje-evaluacion" class="form-control"' +
                    ' placeholder ="Porcentaje de la evaluación" value="' + data[0].porcentaje + '">')

                    activity += ('<option selected>' + data[0].actividad + '</option>')

                    technique += ('<option selected>' + data[0].tecnica + '</option>')

                    instrument += ('<option selected>' + data[0].instrumento + '</option>')

                    $('#fechainicio-evaluacion').datepicker({ dateFormat: 'dd-MM-yy' })
                        .datepicker('setDate', data[0].fechainicio);

                    $('#fechafin-evaluacion').datepicker({ dateFormat: 'dd-MM-yy' })
                        .datepicker('setDate', data[0].fechafin);

                    $('#horainicio-evaluacion').timepicker('setTime', data[0].horainicio);
                    $('#horafin-evaluacion').timepicker('setTime', data[0].horafin);


                    $('#nombre-evaluacion-div').find("input").end().append(nombre);
                    $('#porcentaje-evaluacion-div').find("input").end().append(percentage);
                    $('#select-tipo').find('option').end().append(activity);
                    $('#tecnica-evaluacion').find('option').end().append(technique);
                    $('#instrumento-evaluacion').find('option').end().append(instrument);
                    $('.selectpicker').selectpicker('refresh')

                    hideProgress();
                }
                else {
                    hideProgress();
                }
            });
        }
        else {
            hideProgress();
        }
    });

    $("#btn-modificar-evaluacion").click(function () {
        ModificarEvaluacion();
    });
});