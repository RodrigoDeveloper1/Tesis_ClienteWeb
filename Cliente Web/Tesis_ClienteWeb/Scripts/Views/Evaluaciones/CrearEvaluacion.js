var idColegio = "";
var idLapso = "";
var idCurso = "";
var idMateria = "";
var idProfesor = "";
var UltimoNroEvaluacion;

// Agregar Evaluaciones
function AgregarEvaluaciones() {
    if (idMateria == "" || idProfesor == "")
        swal("¡No ha seleccionado todos los datos!", "Por favor seleccione una materia y/o un docente", "warning");
    else
    {
        SalvarTodo();
        $('#div-table-lista-evaluaciones-cargar tr:last').remove(); //Para borrar la última fila con el signo '+'

        var tdNombre;
        var tdPorcentaje;
        var tdInicio;
        var tdFin;
        var tdTipo;
        var tdAgregar;
        var tdEliminar;
        var evaluacion;
        var listaEvaluaciones = [];

        listaEvaluaciones.push(idColegio);
        listaEvaluaciones.push(idLapso);
        listaEvaluaciones.push(idCurso);
        listaEvaluaciones.push(idMateria);
        listaEvaluaciones.push(idProfesor);

        $('#div-table-lista-evaluaciones-cargar tbody tr').each(function () {
            tdNombre = $(this).children("td:nth-child(1)").html();
            tdPorcentaje = $(this).children("td:nth-child(2)").html();
            tdInicio = $(this).children("td:nth-child(3)").html();
            tdFin = $(this).children("td:nth-child(4)").html();
            tdTipo = $(this).children("td:nth-child(5)").html();
            tdAgregar = $(this).children("td:nth-child(6)").html();
            tdEliminar = $(this).children("td:nth-child(7)").html();


            evaluacion = [tdNombre, tdPorcentaje, tdInicio, tdFin, tdTipo];

            listaEvaluaciones.push(evaluacion);
        });

        var postData = { values: listaEvaluaciones };

        showProgress();
        $.ajax({
            type: "POST",
            url: "/Evaluaciones/CrearEvaluacion",
            traditional: true,
            data: postData,
            success: function (r) {
                window.location.href = 'CrearEvaluacion';
            }
        });
    }
}

// Selectlist de curso, lapso, materia, profesor
function CargarLapsos() {
    //Limpiando la lista de lapsos
    $("#select-lapso-crear").find('option').remove().end().append("<option>Cargando lapsos...</option>");
    $("#select-lapso-crear").selectpicker("refresh");

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

                $("#select-lapso-crear").find('option').remove().end().append(lista);
                $("#select-lapso-crear").selectpicker("refresh");
                hideProgress();
            }
            else {
                hideProgress();
                swal("¡Oops!", "No hay lapsos asociados. Seleccione otro colegio", "warning");
                $("#select-lapso-crear").find('option').remove().end().append('<option>No se encontraron lapsos activos....</option>');
                $("#select-lapso-crear").selectpicker("refresh");
            }
        });
}
function CargarCursos() {
    //Limpiando la lista de cursos
    $("#select-curso-crear").find('option').remove().end().append("<option>Cargando cursos...</option>");
    $("#select-curso-crear").selectpicker("refresh");

    showProgress();
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

                $("#select-curso-crear").find('option').remove().end().append(lista);
                $("#select-curso-crear").selectpicker("refresh");
                hideProgress();
            }
            else {
                hideProgress();
                swal("¡Oops!", "No se encuentran cursos asociados al colegio", "warning");
                $("#select-curso-crear").find('option').remove().end().append('<option>No se encontraron cursos activos....</option>');
                $("#select-curso-crear").selectpicker("refresh");
            }
        })
}
function CargarMaterias() {
    //Limpiando la lista de materias
    $("#select-materia-crear").find('option').remove().end().append("<option>Cargando materias...</option>");
    $("#select-materia-crear").selectpicker("refresh");

    showProgress();
    $.post("/Bridge/ObtenerSelectListMateriasPorLapsoYCurso",
        {
            idLapso: idLapso,
            idCurso: idCurso
        },
        function (data) {
            if (data != null && data.length > 0) {
                var lista = '<option value="">Seleccione el materia...</option>';

                for (var i = 0; i < data.length; i++) {
                    lista += ('<option value="' + data[i].idMateria + '">' + data[i].nombre + '</option>');
                }

                $("#select-materia-crear").find('option').remove().end().append(lista);
                $("#select-materia-crear").selectpicker("refresh");
                hideProgress();
            }
            else {
                hideProgress();
                swal("¡Oops!", "No existen materias asociadas", "warning");
                $("#select-materia-crear").find('option').remove().end().append
                                                  ('<option>No se encontraron materias activas....</option>');
                $("#select-materia-crear").selectpicker("refresh");
            }
        })
}
function CargarProfesores() {
    //Limpiando la lista de profesores
    $("#select-profesor-crear").find('option').remove().end().append("<option>Cargando profesores...</option>");
    $("#select-profesor-crear").selectpicker("refresh");

    showProgress();
    $.post("/Bridge/ObtenerSelectListProfesores",
        {
            idLapso: idLapso,
            idCurso: idCurso,
            idMateria: idMateria
        },
        function (data) {
            if (data != null && data.length > 0) {
                var lista = '<option value="">Seleccione el profesor...</option>';

                for (var i = 0; i < data.length; i++) {
                    lista += ('<option value="' + data[i].idProfesor + '">' + data[i].nombre + '</option>');
                }

                $("#select-profesor-crear").find('option').remove().end().append(lista);
                $("#select-profesor-crear").selectpicker("refresh");
                hideProgress();
            }
            else {
                hideProgress();
                swal("¡Oops!", "No existen docentes asociados", "warning");
                $("#select-profesor-crear").find('option').remove().end().append
                                            ('<option>No se encontraron profesores activas....</option>');
                $("#select-profesor-crear").selectpicker("refresh");
            }
        })
}

$(document).ready(function () {    
    $("#select-colegio-crear").change(function () {
        idColegio = $(this).val();

        if (idColegio != "") {
            CargarLapsos();
        }
    });
    $("#select-lapso-crear").change(function () {
        idLapso = $(this).val();

        if (idColegio != "" && idLapso != "") {
            CargarCursos();
        }
    });
    $("#select-curso-crear").change(function () {
        idCurso = $(this).val();

        if (idColegio != "" && idLapso != "" && idCurso != "") {
            CargarMaterias();
        }
    });
    $("#select-materia-crear").change(function () {
        idMateria = $(this).val();

        if (idColegio != "" && idLapso != "" && idCurso != "" && idMateria != "") {
            CargarProfesores();
        }
    });
    $("#select-profesor-crear").change(function () {
        idProfesor = $(this).val();
    });

    $("#btn-agregar-evaluaciones").bind("click", AgregarEvaluaciones)
    $('.datepicker').datepicker({
        beforeShowDay: $.datepicker.noWeekends
    });
});