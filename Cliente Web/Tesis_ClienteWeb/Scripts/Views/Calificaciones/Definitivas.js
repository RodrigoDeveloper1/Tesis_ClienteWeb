var idCurso = "";
var idMateria = "";

var container;
var hot;
var nombresEvaluaciones = [];
var filaAlumno = [];
var nombresAlumnos = [];
var notasAlumnos = [];
var idsAlumnos = [];
var idsExamenes = [];
var countAlumnos;
var countExamenes;
var countidsExamenes;
var nota;
var listaNotas = [];
var idLapso;


var stop = 0;
var countChange = 0;

$(document).ready(function () {
    $("#select-curso").change(function () {
        idCurso = $(this).val();

        if ($(this).val() != "") {
            showProgress();

            $("#select-materia").find('option').remove().end().append("<option>Cargando materias...</option>");
            $("#select-materia").selectpicker("refresh");

            idCurso = $(this).val();

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

            idCurso = "";
            idMateria = "";
        }
        
    });

    $("#select-materia").change(function () {
        idMateria = $(this).val();

        if (idMateria != "") {
            showProgress();

            var lista = "";
       
            $('#table-lista-evaluaciones').find('tbody').find('tr').remove();

            $.post("/Bridge/ObtenerNotaDefinitivaPor_Materia",
            {
                idCurso: idCurso,
                idMateria: idMateria
            },
            function (data) {
                if (data != null && data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        lista += (
                            '<tr>' +
                                '<td class="td-num-lista verticalLine">' + data[i].numLista + '</td>' +
                                '<td class="td-nombre-alumno verticalLine">' +
                                    data[i].alumnoApellido + ', ' + data[i].alumnoNombre +
                                '</td>' +
                                '<td class="td-1er-lapso verticalLine">' + data[i].definitivaLapso1 + '</td>' +
                                '<td class="td-2do-lapso verticalLine">' + data[i].definitivaLapso2 + '</td>' +
                                '<td class="td-3er-lapso verticalLine">' + data[i].definitivaLapso3 + '</td>' +
                                '<td class="td-definitiva verticalLine" ' +
                                'style="color:' + data[i].colorfuente + ';" ' +
                                'bgcolor=' + data[i].color + '>' + data[i].definitivaFinal + '</td>' +
                            '</tr>');
                    }
                    $('#table-lista-notas').find('tbody').end().append(lista);

                    hideProgress();
                }
                else {
                    $('#table-lista-notas').find('tbody').find('tr').remove();

                    hideProgress();
                }
            });
        }
        else {
            $('#table-lista-notas').find('tbody').find('tr').remove();
            idMateria = "";

            hideProgress();
        }
    });
});