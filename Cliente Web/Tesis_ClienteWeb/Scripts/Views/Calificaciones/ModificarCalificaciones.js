var idCurso = "";
var idLapso = "";
var idMateria = "";
var idEvaluacion = "";
var idAlumno = "";
var nota = "";

//Función que intenta convertir un string a número
function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}

function ModificarNotas() {
    showProgress();

    nota = $("#nota-input").val();

    var datavalidation = TryParseInt(nota, 50);// valida que se pueda parsear a int
    var letravalidation = nota;

    if (datavalidation == 50) // devuelve 50 si no parsea a int
    {
        if (letravalidation != "a" && letravalidation != "A" && letravalidation != "b"
            && letravalidation != "B" && letravalidation != "c" && letravalidation != "C"
            && letravalidation != "d" && letravalidation != "D" && letravalidation != "e"
            && letravalidation != "E") { // valida que solo sean letras a,b,c,d,e

            ValidacionNotas();
            return false;
        }
    }
    else {
        if (datavalidation < 0 || datavalidation > 20) { // valida números negativos y mayores a 20
            ValidacionNotas();

            return false;
        }
    }

    var postData = {
        idCurso: idCurso,
        idAlumno: idAlumno,
        idEvaluacion: idEvaluacion,
        nota: nota,
        idMateria: idMateria
    };

    $.ajax({
        type: "POST",
        url: "/Calificaciones/ModificarCalificaciones",
        traditional: true,
        data: postData,
        success: function (data) {
            window.location.href = 'ModificarCalificaciones';
        },
        error: function () {
            window.location.href = 'ModificarCalificaciones';
        }
    });
}

function ValidacionNotas() {
    swal({
        title: "¡Error en formato de nota!",
        text: "No se aceptan números negativos, números mayores a 20 ni letras diferentes a: A,B,C,D,E .",
        type: "warning",
        confirmButtonColor: "green",
        showCancelButton: false,
        closeOnConfirm: true,
    }, function (isConfirm) {
        // window.location.href = 'CargarCalificaciones';
    });
}

$(document).ready(function () {
    $("#select-curso").change(function () {
        idCurso = $(this).val();

        if (idCurso != "") {
            showProgress();

            $("#select-lapso").find('option').remove().end().append("<option>Cargando lapsos...</option>");
            $("#select-lapso").selectpicker("refresh");

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

                    $("#select-lapso").find('option').remove().end().append(lista);
                    $("#select-lapso").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-lapso").find('option').remove().end()
                        .append('<option>No se encontraron lapso activos....</option>');
                    $("#select-lapso").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-lapso').find('option').remove().end().append('<option>Seleccione el lapso...</option>');
            $("#select-lapso").selectpicker("refresh");

            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");

            $('#select-evaluacion').find('option').remove().end().append('<option>Seleccione la evaluación...</option>');
            $("#select-evaluacion").selectpicker("refresh");

            $('#select-alumno').find('option').remove().end().append('<option>Seleccione el alumno...</option>');
            $("#select-alumno").selectpicker("refresh");

            idCurso = "";
            idLapso = "";
            idMateria = "";
            idEvaluacion = "";
            idAlumno = "";
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

            $('#select-evaluacion').find('option').remove().end().append('<option>Seleccione la evaluación...</option>');
            $("#select-evaluacion").selectpicker("refresh");

            $('#select-alumno').find('option').remove().end().append('<option>Seleccione el alumno...</option>');
            $("#select-alumno").selectpicker("refresh");

            idMateria = "";
            idEvaluacion = "";
            idAlumno = "";
        }
    });

    $("#select-materia").change(function () {
        idMateria = $(this).val();

        if (idMateria != "") {
            showProgress();

            $("#select-evaluacion").find('option').remove().end().append("<option>Cargando las evaluaciones...</option>");
            $("#select-evaluacion").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListEvaluacionesDeCASUS",
            {
                idMateria: idMateria,
                idCurso: idCurso,
                idLapso: idLapso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione la evaluación...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += (
                            '<option value="' + data[i].idEvaluacion + '">' +
                                data[i].nombre + " (" + data[i].porcentaje + "%)" +
                            '</option>');
                    }

                    $("#select-evaluacion").find('option').remove().end().append(lista);
                    $("#select-evaluacion").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-evaluacion").find('option').remove().end()
                        .append('<option>No se encontraron evaluaciones activas....</option>');
                    $("#select-evaluacion").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-evaluacion').find('option').remove().end().append('<option>Seleccione la evaluación...</option>');
            $("#select-evaluacion").selectpicker("refresh");

            $('#select-alumno').find('option').remove().end().append('<option>Seleccione el alumno...</option>');
            $("#select-alumno").selectpicker("refresh");

            idEvaluacion = "";
            idAlumno = "";
        }
    });

    $("#select-evaluacion").change(function () {
        idEvaluacion = $(this).val();

        if (idEvaluacion != "") {
            showProgress();

            $("#select-alumno").find('option').remove().end().append("<option>Cargando los alumnos con notas...</option>");
            $("#select-alumno").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListAlumnosConNotas",
            {
                idMateria: idMateria,
                idCurso: idCurso,
                idLapso: idLapso,
                idEvaluacion: idEvaluacion
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione el alumno...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idAlumno + '">' + data[i].nombre + '</option>');
                    }

                    $("#select-alumno").find('option').remove().end().append(lista);
                    $("#select-alumno").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-evaluacion").find('option').remove().end()
                        .append('<option>No se encontraron alumnos activos....</option>');
                    $("#select-evaluacion").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-alumno').find('option').remove().end().append('<option>Seleccione el estudiante...</option>');
            $("#select-alumno").selectpicker("refresh");

            idAlumno = "";
        }
    });

    $("#select-alumno").change(function () {
        idAlumno = $(this).val();

        if (idAlumno != "") {
            showProgress();

            nota = "";
            $('#nota-input-div').find("input").remove();

            $.post("/Calificaciones/ObtenerCalificacionPorCursoAlumnoYExamen",
            {
                idCurso: idCurso,
                idAlumno: idAlumno,
                idEvaluacion: idEvaluacion,
            },
            function (data) {
                if (data != null && data.length > 0) {
                    nota += (
                        '<input id ="nota-input" class="form-control" placeholder ="Nota del Alumno" ' +
                        'maxlength="2" value="' + data[0].nota + '">');

                    $('#nota-input-div').find("input").end().append(nota);

                    hideProgress();
                }
            });
        }
    });

    $("#ModificarNotasButton").click(function (e) {
        ModificarNotas();
    });
});