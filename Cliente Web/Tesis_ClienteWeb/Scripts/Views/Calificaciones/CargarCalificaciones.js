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
function AgregarNotas() {
    console.log("entra");
    var listaAlumnos = [];
    var listaExamenes = [];
    var listaNotas = [];

     idCurso = $("#select-curso option:selected").val();
     idLapso = $("#select-lapso option:selected").val();
     idMateria = $("#select-materia option:selected").val();

     for (i = 0; i <= countAlumnos; i++) { // for que recorre la columna de alumnos
      
         for (t = 1; t <= countExamenes; t++) {
             // for que recorre la fila de los ex[amenes del alumno
            if (hot.getData()[i][t] != "") { // agarro la data de la celda en la posici[on si es diferente a ""
         
                var datavalidation = TryParseInt(hot.getData()[i][t], 50);// valida que se pueda parsear a int
                var letravalidation = hot.getData()[i][t];
          
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
                console.log("sismo");
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
    console.log("entra3");
            $.ajax({
                type: "POST",
                url: "/Calificaciones/CargarCalificaciones",
                traditional: true,
                data: postData,
                success: function (r) {
                  
                }
            });        
       


    }

function ObtenerAlumnosDelCurso() {
    var lista = "";

    idCurso = $("#select-curso option:selected").val();
    $.post("/Alumnos/ObtenerTablaAlumnosPorIdCurso",
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
function NoExistenEvaluaciones() {
    
            swal({
                title: "¡No Existen Evaluaciones!",
                text: "No existen evaluaciones para la materia en el curso y periodo seleccionados .",
                type: "warning",
                confirmButtonColor: "green",
                showCancelButton: false,
                closeOnConfirm: true,
            },
            function (isConfirm) {
               
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

            $.post("/Evaluaciones/ObtenerSelectListLapsosProfesor",
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

            

            $.post("/Materias/ObtenerSelectListMaterias",
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

    //Obtener lista de evaluaciones
    $("#select-materia").change(function () {
        var lista = "";
        if ($(this).val() != "") {
            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();
            console.log(idLapso);
            $.post("/Evaluaciones/ObtenerTablaEvaluacionesPorMateriaCursoYLapso",
            {
                idMateria: idMateria,
                idCurso: idCurso,
                idLapso: idLapso
            },
            function (data) {
                nombresEvaluaciones.push("Alum/Evalu");
                if (data != null && data.length > 0) {

                    for (var i = 0; i < data.length; i++) {

                        nombresEvaluaciones.push(data[i].nombre);
                        idsExamenes.push(data[i].idEvaluacion);
                    }
                    ObtenerAlumnosDelCurso();
                }
                else {
                    NoExistenEvaluaciones();
                }
            });

          
        }
        else {
            nombresEvaluaciones.push("");
        }
    });

    $("#CargarNotasButton").click(function (e) {
      
        AgregarNotas();
    });

});