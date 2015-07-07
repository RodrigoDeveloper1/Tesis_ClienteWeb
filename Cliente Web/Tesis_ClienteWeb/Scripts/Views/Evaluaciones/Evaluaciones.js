var idEvaluacion = "";
var idDocente = "";
var grade = "";

$(document).ready(function () {
    idDocente = $('#id-docente').val(); //Obteniendo el id del usuario de la sesión

    $("#btn-nueva-evaluacion").click(function (e) { DialogoNuevaEvaluacion(); });

    $('.datepicker').datepicker({ beforeShowDay: $.datepicker.noWeekends });        

    $("#select-curso").change(function () {
        if ($(this).val() != "") {
            showProgress();
            $("#select-lapso").find('option').remove().end().append("<option>Cargando lapsos...</option>");
            $("#select-lapso").selectpicker("refresh");

            idCurso = $(this).val();

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
                    hideProgress();

                    $("#select-lapso").find('option').remove().end().append(lista);
                    $("#select-lapso").selectpicker("refresh");
                }
                else {
                    hideProgress();
                    swal("¡No hay lapsos!", "No hay lapsos asociados a este curso", "warning");

                    $("#select-lapso").find('option').remove().end().append('<option>No se encontraron' + 
                        ' lapso activos....</option>');
                    $("#select-lapso").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-lapso').find('option').remove().end().append('<option>Seleccione el lapso...</option>');
            $("#select-lapso").selectpicker("refresh");

            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");

            $('#table-lista-evaluaciones').find('tbody').find('tr').remove(); //Limpiando la tabla
        }
    });

    $("#select-lapso").change(function () {
        idCurso = $("#select-curso option:selected").val();
        idLapso = $(this).val();

        if ($(this).val() != "") {
            showProgress();

            $("#select-materia").find('option').remove().end().append("<option>Cargando materias...</option>");
            $("#select-materia").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListMaterias",
            {
                idCurso: idCurso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    grade = data[0].grado;

                    var lista = '<option value="">Seleccione la materia...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idMateria + '">' + data[i].nombre + '</option>');
                    }
                    
                    $("#select-materia").find('option').remove().end().append(lista);
                    $("#select-materia").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    hideProgress();
                    swal('!No hay materias¡', 'No hay materias asociadas al curso', 'warning');

                    $("#select-materia").find('option').remove().end().append('<option>No se encontraron' +
                        ' materias activas....</option>');
                    $("#select-materia").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");

            $('#table-lista-evaluaciones').find('tbody').find('tr').remove(); //Limpiando la tabla
        }
    });

    $("#select-materia").change(function () {
        var lista = "";

        if ($(this).val() != "") {
            showProgress();

            $('#table-lista-evaluaciones').find('tbody').find('tr').remove();
            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();

            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerJsonEvaluacionesPor_Curso_Materia_Lapso_Docente",
                data: {
                    idMateria: idMateria,
                    idCurso: idCurso,
                    idLapso: idLapso,
                    idDocente: idDocente
                },
                error: function () {
                    hideProgress();
                    swal('¡Error de carga!', '¡Ha ocurrido un error y no se han podido cargar las ' +
                        'evaluaciones!', 'error');
                },
                success: function (data) {
                    if (data != null && data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            lista += (
                                '<tr id="' + data[i].idEvaluacion + '">' +
                                    '<td class="th-evaluacion-prof">' + data[i].nombre + '</td>' +
                                    '<td class="th-tecnica-prof">' + data[i].tecnica + '</td>' +
                                    '<td class="th-tipo-prof">' + data[i].actividad + '</td>' +
                                    '<td class="th-instrumento-prof">' + data[i].instrumento + '</td>' +
                                    '<td class="th-porcentaje-prof">' + data[i].porcentaje + '</td>' +
                                    '<td class="th-opcion-prof">' + data[i].fechainicio + '</td>' +
                                    '<td class="th-opcion-prof">' + data[i].fechafin + '</td>' +
                                '</tr>');
                        }
                        $('#table-lista-evaluaciones').find('tbody').end().append(lista);

                        hideProgress();
                    }
                    else {
                        hideProgress();
                        swal('¡No hay evaluaciones!', '¡La materia, en el lapso seleccionado, no se le han' +
                            ' asignado evaluaciones aún!');

                        $('#table-lista-evaluaciones').find('tbody').find('tr').remove(); //Limpiando la tabla
                    }
                }
            });
        }
        else
            $('#table-lista-evaluaciones').find('tbody').find('tr').remove(); //Limpiando la tabla
    });

    $("#table-lista-evaluaciones tbody").on("click", "tr", function () {
        $(this).closest("tr").siblings().removeClass("activado");
        $(this).addClass("activado");

        idEvaluacion = $(this).attr("id"); //Obteniendo el id de la evaluación

        if (grade <= 6)
            $('#btn-asociacion-indicadores-literales').removeAttr('disabled');
    });

    $("#btn-asociacion-indicadores-literales").click(function () {
        if (idEvaluacion != "") {
            showProgress();

            $.ajax({
                type: "POST",
                url: "/Bridge/ObteniendoPorcentajeNotasPor_Evaluacion",
                data: {
                    idEvaluacion: idEvaluacion
                },
                success: function (data) {
                    if (data[0].success)
                    {
                        if (data[0].porcentaje < 80) {
                            hideProgress();
                            swal({
                                title: "¿Estás seguro?",
                                text: "Se debería realizar este proceso de asociación únicamente después" +
                                    " de que la evaluación se le hayan cargado al menos un 80% de las notas" +
                                    " respectivas. ",
                                type: "info",
                                showCancelButton: true,
                                confirmButtonText: "Continuar",
                                cancelButtonText: "Regresar",
                                closeOnConfirm: true,
                                closeOnCancel: true
                            },
                            function (isConfirm) {
                                if (isConfirm) {
                                    showProgress();
                                    window.location.href = 'AsociacionIndicadoresLiterales/?id=' + idEvaluacion;
                                }
                            });
                        }
                        else
                            window.location.href = 'AsociacionIndicadoresLiterales/?id=' + idEvaluacion;
                    }
                    else {
                        hideProgress();
                        swal('Error de operación', 'Ha ocurrido un error. Intente de nuevo', 'error');
                    }
                }
            });
        }
        else {
            swal('¿Y la evaluación?', 'Por favor seleccione primero una evaluación', 'warning');
            $('#btn-asociacion-indicadores-literales').attr("disabled", "disabled");
        }
    });
});