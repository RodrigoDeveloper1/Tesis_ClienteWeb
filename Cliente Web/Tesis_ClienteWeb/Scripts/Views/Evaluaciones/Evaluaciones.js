var idEvaluacion;

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
        window.location.href = 'CargarCalificaciones';
    });


}

$(document).ready(function () {
    $("#btn-nueva-evaluacion").click(function (e) { DialogoNuevaEvaluacion(); });

    $('.datepicker').datepicker({
        beforeShowDay: $.datepicker.noWeekends
    });        

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
    //Obtener lista de evaluaciones
    $("#select-materia").change(function () {
        var lista = "";
        if ($(this).val() != "") {
            $('#table-lista-evaluaciones').find('tbody').find('tr').remove();
            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();
            $.post("/Bridge/ObtenerTablaEvaluacionesPorMateriaCursoYLapso",
            {
                idMateria: idMateria,
                idCurso: idCurso,
                idLapso: idLapso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    console.log('Entra');
                    for (var i = 0; i < data.length; i++) {

                        console.log(data[i].tecnica);

                        console.log(data[i].actividad);

                        console.log(data[i].instrumento);

                        lista += ('<tr>' +                                          
                                    ' <td class="th-evaluacion-prof">' + data[i].nombre + '</td>' +
                                    ' <td class="th-tecnica-prof">' + data[i].tecnica + '</td>' +
                                    ' <td class="th-tipo-prof">' + data[i].actividad + '</td>' +
                                    ' <td class="th-instrumento-prof">' + data[i].instrumento + '</td>' +
                                    ' <td class="th-porcentaje-prof">' + data[i].porcentaje + '</td>' +
                                    ' <td class="th-opcion-prof">' + data[i].fechainicio + '</td>' +
                                    ' <td class="th-opcion-prof">' + data[i].fechafin + '</td>' +
                                  '</tr>');
                    }
                    $('#table-lista-evaluaciones').find('tbody').end().append(lista);
                }
                else {
                    console.log('Entra');

                    lista = ('<tr>' +
                                    '<td class="th-evaluacion-prof"></td>' +
                                    '<td class="th-tecnica-prof"></td></td>' +
                                    '<td class="th-tipo-prof"></td>' +
                                    ' <td class="th-instrumento-prof"></td>' +
                                    ' <td class="th-porcentaje-prof"></td>' +
                                    ' <td class="th-opcion-prof"></td>' +
                                    ' <td class="th-opcion-prof"></td>' +
                            '</tr>');

                    $('#table-lista-evaluaciones').find('tbody').find('tr').end().append(lista);
                }
            });
        }
        else {
            lista = ('<tr>' +
                                    ' <td class="th-evaluacion-prof"></td>' +
                                    ' <td class="th-tecnica-prof"></td></td>' +
                                    ' <td class="th-tipo-prof"></td>' +
                                    ' <td class="th-instrumento-prof"></td>' +
                                    ' <td class="th-porcentaje-prof"></td>' +
                                    ' <td class="th-opcion-prof"></td>' +
                                    ' <td class="th-opcion-prof"></td>' +
                    '</tr>');

            $('#table-lista-evaluaciones').find('tbody').find('tr').end().append(lista);
        }
    });    

 



});