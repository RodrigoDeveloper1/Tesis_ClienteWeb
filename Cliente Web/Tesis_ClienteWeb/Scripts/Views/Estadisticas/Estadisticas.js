var idCurso;
var idLapso;
var idMateria;
var idEvaluacion;

$(document).ready(function () {
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

$("#select-materia").change(function () {
    if ($(this).val() != "") {
        idMateria = $(this).val();
        idCurso = $("#select-curso option:selected").val();
        idLapso = $("#select-lapso option:selected").val();

        $("#select-evaluacion").find('option').remove().end().append("<option>Cargando las evaluaciones...</option>");
        $("#select-evaluacion").selectpicker("refresh");

        $.post("/Evaluaciones/ObtenerSelectListEvaluacionesDeCASUS",
        {
            idMateria: idMateria,
            idCurso: idCurso,
            idLapso: idLapso
        },
        function (data) {
            if (data != null && data.length > 0) {
                var lista = '<option value="">Seleccione la evaluación...</option>';

                for (var i = 0; i < data.length; i++) {
                    lista += ('<option value="' + data[i].idEvaluacion + '">' + data[i].nombre + '</option>');

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

});
});