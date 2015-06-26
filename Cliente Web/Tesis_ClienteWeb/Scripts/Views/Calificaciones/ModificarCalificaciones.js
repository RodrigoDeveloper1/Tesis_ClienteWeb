var container; 
var hot;
var nombresEvaluaciones = [];
var filaAlumno = [];
var nombresAlumnos = [];
var idsAlumnos = [];
var idsExamenes = [];
var countAlumnos;
var countExamenes;
var nota;
var idCurso;
var idLapso;
var idMateria;
var idEvaluacion;
function RenderizarTabla() {
    countAlumnos = nombresAlumnos.length - 1;
    countExamenes = nombresEvaluaciones.length -1;

    hot = new Handsontable(container, {
            data: nombresAlumnos,
            rowHeaders: true,
            colHeaders : nombresEvaluaciones,
            minSpareRows: 0,
            persistentState: true
        });      
}
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
     idCurso = $("#select-curso option:selected").val();
     idAlumno = $("#select-alumno option:selected").val();
     idEvaluacion = $("#select-evaluacion option:selected").val();
     idMateria = $("#select-materia option:selected").val();
     var nota = $("#nota-input").val();
    

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
                else
                {               
                    if (datavalidation <0 || datavalidation > 20) { // valida números negativos y mayores a 20
                        
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
                success: function (r) {
                    window.location.href = 'ModificarCalificaciones';
                }
            });        
       


    }
function ObtenerAlumnosDelCurso() {
    var lista = "";

    idCurso = $("#select-curso option:selected").val();
    $.post("/Bridge/ObtenerTablaAlumnosPorIdCurso",
    {
        idCurso: idCurso
    },
    function (data) {
        if (data != null && data.length > 0) {

            for (var i = 0; i < data.length; i++) {

                nombresAlumnos.push([data[i].apellido1 + ',' + data[i].nombre1, "", "", "", "", "", ""
                , "", "", ""]);
                idsAlumnos.push(data[i].idEstudiante);
            }
            RenderizarTabla();
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
       // window.location.href = 'CargarCalificaciones';
    });


}
$(document).ready(function () {

    container = document.getElementById('tablaCargarCalificaciones');

    $("#select-curso").change(function () {

        if ($(this).val() != "") {

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
                    console.log("Entro 2");
                    $("#select-lapso").find('option').remove().end().append(lista);
                    $("#select-lapso").selectpicker("refresh");
                }
                else {
                    $("#select-lapso").find('option').remove().end().append('<option>No se encontraron lapso activos....</option>');
                    $("#select-lapso").selectpicker("refresh");
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
                    console.log("Entro 2");
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
        if ($(this).val() != "") {
            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();
                
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
                        lista += ('<option value="' + data[i].idEvaluacion + '">' + data[i].nombre + " " + data[i].porcentaje + "%" + '</option>');

                    }
                    $("#select-evaluacion").find('option').remove().end().append(lista);
                    $("#select-evaluacion").selectpicker("refresh");
                }
                else {
                    $("#select-evaluacion").find('option').remove().end().append('<option>No se encontraron evaluaciones activas....</option>');
                    $("#select-evaluacion").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");
        }
    });
   
    $("#select-evaluacion").change(function () {
        if ($(this).val() != "") {
            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();
            idEvaluacion = $("#select-evaluacion option:selected").val();

            $("#select-alumno").find('option').remove().end().append("<option>Cargando los alumnos con notas...</option>");
            $("#select-alumno").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListAlumnosConNotas",
            {
                idMateria: idMateria,
                idCurso: idCurso,
                idLapso: idLapso,
                idEvaluacion : idEvaluacion
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione el alumno...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idAlumno + '">' + data[i].nombre + '</option>');

                    }
                    $("#select-alumno").find('option').remove().end().append(lista);
                    $("#select-alumno").selectpicker("refresh");
                }
                else {
                    $("#select-evaluacion").find('option').remove().end().append('<option>No se encontraron alumnos activos....</option>');
                    $("#select-evaluacion").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");
        }
    });

    $("#select-alumno").change(function () {
        if ($(this).val() != "") {
            console.log("Entro brouuu");
            var nota = "";
            idAlumno = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idEvaluacion = $("#select-evaluacion option:selected").val();

            $('#nota-input-div').find("input").remove();

            $.post("/Calificaciones/ObtenerCalificacionPorCursoAlumnoYExamen",
            {
                idCurso: idCurso,
                idAlumno: idAlumno,
                idEvaluacion: idEvaluacion,
            },
            function (data) {
                if (data != null && data.length > 0) {

                    nota += ('<input id ="nota-input" class="form-control"' +
                     ' placeholder ="Nota del Alumno" maxlength="2" value="' + data[0].nota + '">')

                    $('#nota-input-div').find("input").end().append(nota);
                }
                else {
                   
                    console.log("Else brouuu");
                }
            });
        }
        else {
           
        }
    });


    $("#ModificarNotasButton").click(function (e) {
      
        ModificarNotas();
    });

});