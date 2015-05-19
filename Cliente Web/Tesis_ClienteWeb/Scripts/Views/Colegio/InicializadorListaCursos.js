var idColegio;
var nombreCurso;

$(document).ready(function () {
    //Obtener lista de cursos
    $("#select-colegio").change(function () {
        if ($(this).val() != "") {
            /* Configuración para select - Fuente del código de configuración del selectpicker: 
             * http://stackoverflow.com/questions/23514318/bootstrap-select-reinitialize-on-dynamically-added-element
             */

            $("#select-curso").find('option').remove().end().append("<option>Cargando cursos...</option>");
            $("#select-curso").selectpicker("refresh");

            idColegio = $(this).val();

            $.post("/Cursos/ObtenerSelectListCursos",
            {
                idColegio: idColegio
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione el curso...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i] + '">' + data[i] + '</option>');
                    }

                    $("#select-curso").find('option').remove().end().append(lista);
                    $("#select-curso").selectpicker("refresh");
                }
                else {
                    $("#select-curso").find('option').remove().end().append('<option>No se encontraron cursos activos....</option>');
                    $("#select-curso").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-curso').find('option').remove().end().append('<option>Seleccione el curso...</option>');
            $("#select-curso").selectpicker("refresh");
        }
    });

    //Obtener el nombre del curso
    $("#select-curso").change(function () {
        if ($(this).val() != "") {
            nombreCurso = $(this).val();
        }
    });
});