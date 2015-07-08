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
var porcExamenes = [];
var definitiva = 0;
var auxDefinitiva = 0;
var deffinal = 0;

function RenderizarTabla() {
    countAlumnos = nombresAlumnos.length - 1;
    countExamenes = nombresEvaluaciones.length -1;
  

    hot = new Handsontable(container, {
            data: nombresAlumnos,
            rowHeaders: true,
            colHeaders : nombresEvaluaciones,
            minSpareRows: 0,
            persistentState: true,
            fixedColumnsLeft: 0,
            fixedRowsTop: 0,
          
    });   

    ReadOnlyCells();
    stop = 1;     
}
function definitivasAprobadas(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.color = 'green';
    td.style.background = '#CEC';
}
function definitivasAplazadas(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.color = 'red';
    td.style.background = '#f9998e';
}
var countrow = 0;
function ReadOnlyCells() {
    hot.updateSettings({       
        cells: function (row, col, prop) {

            var cellProperties = {};

            if (hot.getData()[row][col] != ' ' && stop == 0) {
                cellProperties.readOnly = true;
            }
            if ( col === nombresEvaluaciones.length - 1 && this.instance.getData()[row][col] >=10 ||
                col === nombresEvaluaciones.length - 1 && this.instance.getData()[row][col] === "A" ||
                col === nombresEvaluaciones.length - 1 && this.instance.getData()[row][col] === "B" ||
                col === nombresEvaluaciones.length - 1 && this.instance.getData()[row][col] === "C" ||
                col === nombresEvaluaciones.length - 1 && this.instance.getData()[row][col] === "D" ) {
                cellProperties.renderer = definitivasAprobadas; // uses function directly
            } else if (col === nombresEvaluaciones.length - 1 && this.instance.getData()[row][col] < 10 ||
                col === nombresEvaluaciones.length - 1 && this.instance.getData()[row][col] === "E" ) {
                cellProperties.renderer = definitivasAplazadas; // uses function directly
            }

            return cellProperties;
        }
    })
   
}
function SetearTabla() {
  //  var w = 0;
 //   for (i = 0; i <= countAlumnos; i++) {
  //      for (t = 1; t <= countExamenes; t++) {
 //           hot.setDataAtCell(i, t, listaNotas[w]);
 //           w++;
  //      }
    //   }
    
}
function TryParseFloat(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseFloat(str);
            }
        }
    }
    return retValue;
}
function AgregarNotas() {
    showProgress();

    var listaAlumnos = [];
    var listaExamenes = [];
    var listaNotas = [];
    idCurso = $("#select-curso option:selected").val();
    idLapso = $("#select-lapso option:selected").val();
    idMateria = $("#select-materia option:selected").val();

    for (i = 0; i <= countAlumnos; i++) { // for que recorre la columna de alumnos      
        for (t = 1; t <= countExamenes - 1; t++) {
            if (hot.getData()[i][t] != " ") { // agarro la data de la celda en la posición si es diferente a ""         
                var datavalidation = TryParseFloat(hot.getData()[i][t], 50);// valida que se pueda parsear a float
                var letravalidation = hot.getData()[i][t];

                if (datavalidation == 50) { // devuelve 50 si no parsea a float
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

                listaAlumnos.push(idsAlumnos[i]); // agrego el id del alumno
                listaExamenes.push(idsExamenes[t - 1]); // agrego el id de la evaluaci[on
                nota = hot.getData()[i][t];
                listaNotas.push(nota); // agrego la nota       
            }
        }
    }
    
    var postData = {
        idCurso: idCurso,
        idLapso: idLapso,
        idMateria: idMateria,
        alumnos: listaAlumnos,
        examenes: listaExamenes,
        notas: listaNotas
    };

    $.ajax({
        type: "POST",
        url: "/Calificaciones/CargarCalificaciones",
        traditional: true,
        data: postData,
        success: function () {
            swal({
                title: " ¡Carga de notas exitoso!",
                text: "Se han cargado las notas correctamente.",
                type: "success",
                closeOnConfirm: true,
            }, function (isConfirm) {
                if (isConfirm) {
                    showProgress();

                    window.location.href = 'GestionCalificaciones';
                }
            });

            window.location.href = 'GestionCalificaciones';
        },
        error: function () {
            hideProgress();

            swal({
                title: " ¡Error!",
                text: "Ha ocurrido un error con la carga de notas.",
                type: "error",
                closeOnConfirm: true,
            }, function (isConfirm) {
                if (isConfirm) {
                    showProgress();

                    window.location.href = 'GestionCalificaciones';
                } 
            });
        }
    });
}

function ObtenerNotas() {
    idCurso = $("#select-curso option:selected").val();
    idLapso = $("#select-lapso option:selected").val();
    idMateria = $("#select-materia option:selected").val();
    var postData = {
        idCurso: idCurso,
        idLapso: idLapso,
        idMateria: idMateria,
        alumnos: idsAlumnos,
        examenes: idsExamenes
    };
    $.ajax({
        type: "POST",
        url: "/Calificaciones/ObtenerNotas",
        traditional: true,
        data: postData,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                listaNotas.push(data[i].nota);              
            }
           // RenderizarTabla();
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
    },
    function (isConfirm) {
        
        hideProgress();
    });


}
function ObtenerAlumnosDelCurso() {
    var lista = "";

    idCurso = $("#select-curso option:selected").val();
    $.post("/Bridge/ObtenerTablaAlumnosPorIdCurso",
    {
        idCurso: idCurso
    },
    function (data1) {
        if (data1 != null && data1.length > 0) {

            for (var i = 0; i < data1.length; i++) {
                idsAlumnos.push(data1[i].idEstudiante);
            }

            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();
            idMateria = $("#select-materia option:selected").val();
            var postData = {
                idCurso: idCurso,
                idLapso: idLapso,
                idMateria: idMateria,
                alumnos: idsAlumnos,
                examenes: idsExamenes
            };
            $.ajax({
                type: "POST",
                url: "/Calificaciones/ObtenerNotas",
                traditional: true,
                data: postData,
                success: function (data) {
              
                    for (var i = 0; i < data.length; i++) {
                        listaNotas.push(data[i].nota);
                    }
                    var w = 0;
                    countidsExamenes = idsExamenes.length;

                    var gradoValidation = TryParseFloat(listaNotas[0], 50);// valida que se pueda parsear a float
                  
                    if (gradoValidation == 50) // devuelve 50 si no parsea a float
                    { // Primaria
                        for (var i = 0; i < data1.length; i++) {
                            definitiva = 0;
                            var definitivaLetra = "";
                            var resultado = [];
                            resultado.push(data1[i].apellido1 + ', ' + data1[i].nombre1);
                            for (var r = 0; r < countidsExamenes; r++) {
                                auxDefinitiva = 0;
                                resultado.push(listaNotas[w]);
                                if (listaNotas[w] == "A" || listaNotas[w] == "a")
                                    auxDefinitiva = 5;
                                else if (listaNotas[w] == "B" || listaNotas[w] == "b")
                                    auxDefinitiva = 4;
                                else if(listaNotas[w] == "C" || listaNotas[w] == "c")
                                    auxDefinitiva = 3;
                                else if(listaNotas[w] == "D" || listaNotas[w] == "d")
                                    auxDefinitiva = 2;
                                else if (listaNotas[w] == "E" || listaNotas[w] == "e")
                                    auxDefinitiva = 1;
                                definitiva = definitiva + auxDefinitiva;
                                
                                ;
                                w++;
                            }
                            deffinal = Math.round(definitiva / countidsExamenes);

                             if (deffinal <= 5 &&
                            deffinal > 4 )
                                definitivaLetra = "A";
                             else if (deffinal <= 4 &&
                            deffinal > 3)
                                definitivaLetra = "B";
                             else if (deffinal <= 3 &&
                            deffinal > 2)
                                definitivaLetra = "C";
                             else if (deffinal <= 2 &&
                            deffinal > 1)
                                definitivaLetra = "D";
                             else if (deffinal <= 1)
                                definitivaLetra = "E";

                             deffinal = 0;
                            resultado.push(definitivaLetra);
                            nombresAlumnos.push(resultado);

                        }
                    }
                    else { // Bachillerato
                        for (var i = 0; i < data1.length; i++) {
                            definitiva = 0;

                            var resultado = [];
                            resultado.push(data1[i].apellido1 + ',' + data1[i].nombre1);
                            for (var r = 0; r < countidsExamenes; r++) {
                                auxDefinitiva = 0;
                                resultado.push(listaNotas[w]);

                                auxDefinitiva = porcExamenes[r] * listaNotas[w];
                                definitiva = definitiva + auxDefinitiva;
                              

                                ;
                                w++;
                            }

                            resultado.push(Math.round(definitiva.toFixed(2))); // redondea la definitiva
                            nombresAlumnos.push(resultado);

                        }
                    }



                    
                   
                    RenderizarTabla();
                }
            }).done(function () {
                hideProgress();

            });
        }
        else {
            NoExistenAlumnos()
        }
    });

}

function NoExistenAlumnos() {

    swal({
        title: "¡No Existen Alumnos!",
        text: "No existen alumnos para el curso y periodo seleccionados .",
        type: "warning",
        confirmButtonColor: "green",
        showCancelButton: false,
        closeOnConfirm: true,
    },
    function (isConfirm) {
        window.location.href = 'CargarCalificaciones';
    });


}

$(document).ready(function () {
    container = document.getElementById('tablaCargarCalificaciones');

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
               
                    $("#select-lapso").find('option').remove().end().append(lista);
                    $("#select-lapso").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-lapso").find('option').remove().end().append('<option>No se encontraron lapso activos....</option>');
                    $("#select-lapso").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-lapso').find('option').remove().end().append('<option>Seleccione el lapso...</option>');
            $("#select-lapso").selectpicker("refresh");
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
                    var lista = '<option value="">Seleccione la materia...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idMateria + '">' + data[i].nombre + '</option>');
                    }

                    $("#select-materia").find('option').remove().end().append(lista);
                    $("#select-materia").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-materia").find('option').remove().end().append('<option>No se encontraron materias activas....</option>');
                    $("#select-materia").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");
        }
    });
    $("#select-materia").change(function () {
        stop = 0;
        nombresEvaluaciones = [];
        filaAlumno = [];
        nombresAlumnos = [];
        notasAlumnos = [];
        idsAlumnos = [];
        idsExamenes = [];
        countAlumnos = 0;
        countExamenes = 0;
        countidsExamenes = 0;
        nota = "";
        listaNotas = [];
        countChange++;

        $("#tablaCargarCalificaciones-div").find('div').remove().end().append("<div class='col-lg-12 handsontable' id=tablaCargarCalificaciones" + countChange + "></div>");
        container = document.getElementById('tablaCargarCalificaciones'+countChange);
        var lista = "";
        if ($(this).val() != "") {
            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();
           
            showProgress();

            $.post("/Bridge/ObtenerTablaEvaluacionesPorMateriaCursoYLapso",
            {
                idMateria: idMateria,
                idCurso: idCurso,
                idLapso: idLapso
            },
            function (data) {
               
                nombresEvaluaciones.push("Alumnos");
                if (data != null && data.length > 0) {

                    for (var i = 0; i < data.length; i++) {
                        nombresEvaluaciones.push(data[i].nombre + " " + data[i].porcentaje + "%");
                        idsExamenes.push(data[i].idEvaluacion);
                        porcExamenes.push(parseInt(data[i].porcentaje)/100);
                    }
                    nombresEvaluaciones.push("Definitivas");
                    ObtenerAlumnosDelCurso();
                }
                else {
                    swal('¡Oops!', 'No hay evaluaciones asociadas', 'warning');
                }
            });
        }
        else {
            nombresEvaluaciones.push("");
        }
    });
    
    $("#CargarNotasButton").click(function (e) { AgregarNotas(); });
    $("#ModificarNotasButton").click(function (e) {     
        window.location.href = 'ModificarCalificaciones';
    });
});