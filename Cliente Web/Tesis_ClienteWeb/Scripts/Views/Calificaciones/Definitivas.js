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
var idCurso;
var idLapso;
var idMateria;
var stop = 0;
var countChange = 0;

$(document).ready(function () {

 

    $("#select-curso").change(function () {
        

        if ($(this).val() != "") {

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
                }
                else {
                    $("#select-materia").find('option').remove().end().append('<option>No se encontraron materias activas....</option>');
                    $("#select-materia").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");
        }
        
    });

    $("#select-materia").change(function () {
   
        showProgress();
        var lista = "";    
        
        if ($(this).val() != "") {
        
       
            $('#table-lista-evaluaciones').find('tbody').find('tr').remove();
            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();
            $.post("/Bridge/ObtenerNotaDefinitivaPor_Materia",
            {
                idCurso: idCurso,
                idMateria: idMateria
             
               
            },
            function (data) {
                if (data != null && data.length > 0) {
                   
                    for (var i = 0; i < data.length; i++) {


                        lista += ('<tr>' +
                                    ' <td class="td-num-lista verticalLine">' + data[i].numLista + '</td>' +
                                    ' <td class="td-nombre-alumno verticalLine">' + data[i].alumnoApellido + ', ' + data[i].alumnoNombre + '</td>' +
                                    ' <td class="td-1er-lapso verticalLine">' + data[i].definitivaLapso1 + '</td>' +
                                    ' <td class="td-2do-lapso verticalLine">' + data[i].definitivaLapso2 + '</td>' +
                                    ' <td class="td-3er-lapso verticalLine">' + data[i].definitivaLapso3 + '</td>' +
                                    ' <td class="td-definitiva verticalLine" style="color:'+data[i].colorfuente+';" bgcolor=' + data[i].color + '>' + data[i].definitivaFinal + '</td>' +
                                  '</tr>');
                    }
                    $('#table-lista-notas').find('tbody').end().append(lista);
                    hideProgress();
                }
                else {

                    lista = ('<tr>' +
                                    ' <td class="td-num-lista"></td>' +
                                    ' <td class="td-nombre-alumno"></td>' +
                                    ' <td class="td-1er-lapso"></td>' +
                                    ' <td class="td-2do-lapso"></td>' +
                                    ' <td class="td-3er-lapso"></td>' +
                                    ' <td class="td-definitiva"></td>' +
                            '</tr>');

                    $('#table-lista-notas').find('tbody').find('tr').end().append(lista);
                    hideProgress();
                }
            });
        }
        else {
            lista = ('<tr>' +
                                    ' <td class="td-num-lista"></td>' +
                                    ' <td class="td-nombre-alumno"></td>' +
                                    ' <td class="td-1er-lapso"></td>' +
                                    ' <td class="td-2do-lapso"></td>' +
                                    ' <td class="td-3er-lapso"></td>' +
                                    ' <td class="td-definitiva"></td>' +
                    '</tr>');

            $('#table-lista-notas').find('tbody').find('tr').end().append(lista);
            hideProgress();
        }
       
    });
    
});

