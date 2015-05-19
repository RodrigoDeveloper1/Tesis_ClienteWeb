var idColegio;
var idLapso;
var idCurso;
var idMateria;
var idProfesor;
var UltimoNroEvaluacion;

function Add() {
    UltimoNroEvaluacion = parseInt($('#div-table-lista-evaluaciones-cargar tr:last').prev().find("td:first").text()) + 1;

    if (!UltimoNroEvaluacion)
        UltimoNroEvaluacion = 1;

    /*Se borra la fila que contenía solo el símbolo de añadir un nueva evaluacion*/
    var par = $(this).parent().parent(); //tr
    par.remove();

    /*Se inserta la primera fila para agregar una nueva evaluacion*/
    $("#div-table-lista-evaluaciones-cargar tbody").append(
        "<tr>" +
            "<td class='td-nombre'>" + UltimoNroEvaluacion + "</td>" +
            "<td class='td-porcentaje'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-inicio'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-fin'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-horafin'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-tipo'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-tecnica'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-instrumento'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-agregar-alumno'><i class='ui-icon ui-icon-check i-agregar-alumno'></i></td>" +
            "<td class='td-eliminar-alumno'><i class='fa fa-minus-circle i-eliminar-fila-alumno'></i></td>" +
        "</tr>");

    /*Se inserta una segunda fila solamente con el botón de añadir más alumnos*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
            "<td class='td-matricula'></td>" +
            "<td class='td-primerapellido-alumno'></td>" +
            "<td class='td-segundoapellido-alumno'></td>" +
            "<td class='td-primernombre-alumno'></td>" +
            "<td class='td-segundonombre-alumno'></td>" +
            "<td class='td-agregar-alumno'></td>" +
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $(".i-agregar-alumno").bind("click", Save)
    $("#i-add-fila-alumno").bind("click", Add);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
};
function Save() {
    var par = $(this).parent().parent(); //tr
    var tdNroAlumno = par.children("td:nth-child(1)");
    var tdMatricula = par.children("td:nth-child(2)");
    var tdPrimerApellido = par.children("td:nth-child(3)");
    var tdSegundoApellido = par.children("td:nth-child(4)");
    var tdPrimerNombre = par.children("td:nth-child(5)");
    var tdSegundoNombre = par.children("td:nth-child(6)");
    var tdAgregar = par.children("td:nth-child(7)");
    var tdEliminar = par.children("td:nth-child(8)");

    //Para debuggear
    /*console.log("tdNroAlumno.html(): " + tdNroAlumno.html());
    console.log("tdApellidos: " + tdApellidos.html());
    console.log("tdNombres: " + tdNombres.html());
    console.log("tdAgregar: " + tdAgregar.html());
    console.log("tdEliminar: " + tdEliminar.html()); */

    tdNroAlumno.html(tdNroAlumno.html());
    tdMatricula.html(tdMatricula.children("input[type=text]").val());
    tdPrimerApellido.html(tdPrimerApellido.children("input[type=text]").val());
    tdSegundoApellido.html(tdSegundoApellido.children("input[type=text]").val());
    tdPrimerNombre.html(tdPrimerNombre.children("input[type=text]").val());
    tdSegundoNombre.html(tdSegundoNombre.children("input[type=text]").val());
    tdAgregar.html("<i class='fa fa-edit i-editar-alumno'></i>"); //El ícono cambia al de editar
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>");

    $(".i-editar-alumno").bind("click", Edit);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
};
function Edit() {
    var par = $(this).parent().parent(); //tr
    var tdNroAlumno = par.children("td:nth-child(1)");
    var tdMatricula = par.children("td:nth-child(2)");
    var tdPrimerApellido = par.children("td:nth-child(3)");
    var tdSegundoApellido = par.children("td:nth-child(4)");
    var tdPrimerNombre = par.children("td:nth-child(5)");
    var tdSegundoNombre = par.children("td:nth-child(6)");
    var tdEditar = par.children("td:nth-child(7)");
    var tdEliminar = par.children("td:nth-child(8)");

    tdNroAlumno.html(tdNroAlumno.html());
    tdMatricula.html("<input class='form-control input-sm' type='text' value=" + tdMatricula.html() + ">");
    tdPrimerApellido.html("<input class='form-control input-sm' type='text' value=" + tdPrimerApellido.html() + ">");
    tdSegundoApellido.html("<input class='form-control input-sm' type='text' value=" + tdSegundoApellido.html() + ">");
    tdPrimerNombre.html("<input class='form-control input-sm' type='text' value=" + tdPrimerNombre.html() + ">");
    tdSegundoNombre.html("<input class='form-control input-sm' type='text' value=" + tdSegundoNombre.html() + ">");
    tdEditar.html("<i class='ui-icon ui-icon-check i-agregar-alumno'></i>"); //El ícono cambia al de agregar
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>");

    $(".i-agregar-alumno").bind("click", Save)
    $(".i-eliminar-fila-alumno").bind("click", Delete);
};
function Delete() {
    var numeroLista = 1;
    var par = $(this).parent().parent(); //tr
    par.remove();

    //Se borra la última fila (la que contiene solo el símbolo de añadir)
    $('#div-table-lista-alumnos-cargar tr:last').remove();

    //Se reescriben los números de cada fila
    $('#div-table-lista-alumnos-cargar tbody tr').each(function () {
        var tdNroAlumno = $(this).children("td:nth-child(1)");
        tdNroAlumno.html(numeroLista);

        numeroLista = parseInt(numeroLista + 1);
    });

    //Se agrega la última fila solo con el botón de añadir
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
            "<td class='td-matricula'></td>" +
            "<td class='td-primerapellido-alumno'></td>" +
            "<td class='td-segundoapellido-alumno'></td>" +
            "<td class='td-primernombre-alumno'></td>" +
            "<td class='td-segundonombre-alumno'></td>" +
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $("#i-add-fila-alumno").bind("click", Add);
};


function CargarCursos() {
    //Limpiando la lista de cursos
    $("#select-curso-crear").find('option').remove().end().append("<option>Cargando cursos...</option>");
    $("#select-curso-crear").selectpicker("refresh");

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
            }
            else {
                $("#select-curso-crear").find('option').remove().end().append('<option>No se encontraron cursos activos....</option>');
                $("#select-curso-crear").selectpicker("refresh");
            }
        })
};
function CargarLapsos() {
    //Limpiando la lista de lapsos
    $("#select-lapso-crear").find('option').remove().end().append("<option>Cargando lapsos...</option>");
    $("#select-lapso-crear").selectpicker("refresh");

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
            }
            else {
                $("#select-lapso-crear").find('option').remove().end().append('<option>No se encontraron lapsos activos....</option>');
                $("#select-lapso-crear").selectpicker("refresh");
            }
        })

}
function CargarMaterias() {
    //Limpiando la lista de materias
    $("#select-materia-crear").find('option').remove().end().append("<option>Cargando materias...</option>");
    $("#select-materia-crear").selectpicker("refresh");

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
            }
            else {
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
            }
            else {
                $("#select-profesor-crear").find('option').remove().end().append
                                            ('<option>No se encontraron profesores activas....</option>');
                $("#select-profesor-crear").selectpicker("refresh");
            }
        })
}

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

    $("#select-curso-crear").change(function () {

        if ($(this).val() != "") {

            $("#select-lapso-crear").find('option').remove().end().append("<option>Cargando lapsos...</option>");
            $("#select-lapso-crear").selectpicker("refresh");

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
                    console.log("Entro 2");
                    $("#select-lapso-crear").find('option').remove().end().append(lista);
                    $("#select-lapso-crear").selectpicker("refresh");
                }
                else {
                    $("#select-lapso-crear").find('option').remove().end().append('<option>No se encontraron lapso activos....</option>');
                    $("#select-lapso-crear").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-lapso-crear').find('option').remove().end().append('<option>Seleccione el lapso...</option>');
            $("#select-lapso-crear").selectpicker("refresh");
        }
    });


    $("#select-lapso-crear").change(function () {
        idCurso = $("#select-curso-crear option:selected").val();
        idLapso = $(this).val();
        if ($(this).val() != "") {

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
                    console.log("Entro 2");
                    $("#select-materia-crear").find('option').remove().end().append(lista);
                    $("#select-materia-crear").selectpicker("refresh");
                }
                else {
                    $("#select-materia-crear").find('option').remove().end().append('<option>No se encontraron materias activas....</option>');
                    $("#select-materia-crear").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-materia-crear').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia-crear").selectpicker("refresh");
        }
    });
    //Obtener lista de evaluaciones
    $("#select-materia-crear").change(function () {
        idMateria = $(this).val();
    });

});