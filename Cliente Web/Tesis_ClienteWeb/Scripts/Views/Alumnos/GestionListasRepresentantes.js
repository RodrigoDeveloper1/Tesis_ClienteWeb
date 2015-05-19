var idColegio;
var idAnoEscolarActivo;
var idCurso;
var idEstudiante = 0;
var representante1_id = 0;
var representante2_id = 0;
var poseeRepresentante_1;
var poseeRepresentante_2;

function ClearTextBoxRepresentante_1()
{
    $('#cedula-representante-1').val("");
    $('#select-sexo-1').val("V-");
    $("#select-sexo-1").selectpicker("refresh");
    $('#nombre-representante-1').val("");
    $('#apellido-1-representante-1').val("");
    $('#apellido-2-representante-1').val("");
    $('#correo-1').val("");
}
function ClearTextBoxRepresentante_2() {
    $('#cedula-representante-2').val("");
    $('#select-sexo-2').val("V-");
    $("#select-sexo-2").selectpicker("refresh");
    $('#nombre-representante-2').val("");
    $('#apellido-1-representante-2').val("");
    $('#apellido-2-representante-2').val("");
    $('#correo-2').val("");
}

$(document).ready(function () {
    //Obtener el año escolar y la lista de cursos de ese año escolar
    $("#select-colegio").change(function () {
        idColegio = $(this).val();

        $("#ano-escolar").val("Cargando el año escolar");

        $("#select-curso").find('option').remove().end().append('<option>Cargando la lista ' +
            'de cursos...</option>');
        $("#select-curso").selectpicker("refresh");

        if (idColegio != "") {
            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerAnoEscolarActivoEnLabel",
                data: {
                    idColegio: idColegio
                },
                success: function (data) {
                    if (data[0].success) {
                        $("#ano-escolar").val(data[0].label);
                        idAnoEscolarActivo = data[0].idAnoEscolar;

                        $.ajax({
                            type: "POST",
                            url: "/Bridge/ObtenerListaNombresDeCursosPorAnoEscolar",
                            data: {
                                idAnoEscolar: idAnoEscolarActivo
                            },
                            success: function (data2) {
                                if (data2[0].success) {
                                    var lista = '<option value="">Seleccione el curso...</option>';
                                    
                                    for (var i = 0; i < data2.length; i++) {
                                        lista += ('<option value="' + data2[i].idCurso + '">' +
                                            data2[i].nombreCurso + '</option>');
                                    }

                                    $("#select-curso").find('option').remove().end().append(lista);
                                    $("#select-curso").selectpicker("refresh");

                                    $("#select-estudiantes").find('option').remove().end().append('<option>' +
                                        'Seleccione un estudiante...</option>');
                                    $("#select-estudiantes").selectpicker("refresh");

                                    $('#cedula-representante-1').val("");
                                    $('#select-sexo-1').val(0);
                                    $("#select-sexo-1").selectpicker("refresh");
                                    $('#nombre-representante-1').val("");
                                    $('#apellido-1-representante-1').val("");
                                    $('#apellido-2-representante-1').val("");
                                    $('#correo-1').val("");

                                    $('#cedula-representante-2').val("");
                                    $('#select-sexo-2').val(0);
                                    $("#select-sexo-2").selectpicker("refresh");
                                    $('#nombre-representante-2').val("");
                                    $('#apellido-1-representante-2').val("");
                                    $('#apellido-2-representante-2').val("");
                                    $('#correo-2').val("");

                                    idEstudiante = 0
                                    representante1_id = 0;
                                    representante2_id = 0;
                                }
                                else {
                                    idCurso = 0;
                                    $("#select-curso").find('option').remove().end().append('<option>El ' +
                                        'colegio no posee cursos</option>');
                                    $("#select-curso").selectpicker("refresh");

                                    $("#select-estudiantes").find('option').remove().end().append('<option>' +
                                        'Seleccione un estudiante...</option>');
                                    $("#select-estudiantes").selectpicker("refresh");

                                    $('#cedula-representante-1').val("");
                                    $('#select-sexo-1').val(0);
                                    $("#select-sexo-1").selectpicker("refresh");
                                    $('#nombre-representante-1').val("");
                                    $('#apellido-1-representante-1').val("");
                                    $('#apellido-2-representante-1').val("");
                                    $('#correo-1').val("");

                                    $('#cedula-representante-2').val("");
                                    $('#select-sexo-2').val(0);
                                    $("#select-sexo-2").selectpicker("refresh");
                                    $('#nombre-representante-2').val("");
                                    $('#apellido-1-representante-2').val("");
                                    $('#apellido-2-representante-2').val("");
                                    $('#correo-2').val("");

                                    idEstudiante = 0
                                    representante1_id = 0;
                                    representante2_id = 0;
                                }                                                                
                            }
                        });
                    }
                    else {
                        idCurso = 0;
                        $("#ano-escolar").val("No posee año escolar activo");

                        $("#select-curso").find('option').remove().end().append('<option>Seleccione ' +
                            'el çurso...</option>');
                        $("#select-curso").selectpicker("refresh");

                        $("#select-estudiantes").find('option').remove().end().append('<option>Seleccione' +
                            ' un estudiante...</option>');
                        $("#select-estudiantes").selectpicker("refresh");

                        $('#cedula-representante-1').val("");
                        $('#select-sexo-1').val(0);
                        $("#select-sexo-1").selectpicker("refresh");
                        $('#nombre-representante-1').val("");
                        $('#apellido-1-representante-1').val("");
                        $('#apellido-2-representante-1').val("");
                        $('#correo-1').val("");

                        $('#cedula-representante-2').val("");
                        $('#select-sexo-2').val(0);
                        $("#select-sexo-2").selectpicker("refresh");
                        $('#nombre-representante-2').val("");
                        $('#apellido-1-representante-2').val("");
                        $('#apellido-2-representante-2').val("");
                        $('#correo-2').val("");

                        idEstudiante = 0
                        representante1_id = 0;
                        representante2_id = 0;
                    }
                }
            });
        }
        else {
            idCurso = 0;
            $("#ano-escolar").val("");
            $("#select-curso").find('option').remove().end().append('<option>Seleccione el curso...</option>');
            $("#select-curso").selectpicker("refresh");

            $("#select-estudiantes").find('option').remove().end().append('<option>Seleccione un ' +
                'estudiante...</option>');
            $("#select-estudiantes").selectpicker("refresh");

            $('#cedula-representante-1').val("");
            $('#select-sexo-1').val(0);
            $("#select-sexo-1").selectpicker("refresh");
            $('#nombre-representante-1').val("");
            $('#apellido-1-representante-1').val("");
            $('#apellido-2-representante-1').val("");
            $('#correo-1').val("");

            $('#cedula-representante-2').val("");
            $('#select-sexo-2').val(0);
            $("#select-sexo-2").selectpicker("refresh");
            $('#nombre-representante-2').val("");
            $('#apellido-1-representante-2').val("");
            $('#apellido-2-representante-2').val("");
            $('#correo-2').val("");

            idEstudiante = 0
            representante1_id = 0;
            representante2_id = 0;
        }
    });

    //Obteniendo la lista de estudiantes
    $("#select-curso").change(function () {
        idCurso = $(this).val();

        $("#select-estudiantes").find('option').remove().end().append('<option>Cargando la lista ' +
            'de estudiantes...</option>');
        $("#select-estudiantes").selectpicker("refresh");

        if (idCurso != null) {
            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerJsonEstudiantesPorCurso",
                data: {
                    idCurso: idCurso
                },
                success: function (data) {
                    if (data[0].success) {
                        var lista = '<option value="">Seleccione un estudiante...</option>';

                        for (var i = 0; i < data.length; i++) {
                            lista += ('<option value="' + data[i].studentId + '">' + data[i].numLista + '# ' +
                                data[i].apellido1 + ' ' + data[i].apellido2 + ', ' + data[i].nombre1 + ' ' +
                                data[i].nombre2 + '</option>');
                        }

                        $("#select-estudiantes").find('option').remove().end().append(lista);
                        $("#select-estudiantes").selectpicker("refresh");

                        $('#cedula-representante-1').val("");
                        $('#select-sexo-1').val(0);
                        $("#select-sexo-1").selectpicker("refresh");
                        $('#nombre-representante-1').val("");
                        $('#apellido-1-representante-1').val("");
                        $('#apellido-2-representante-1').val("");
                        $('#correo-1').val("");

                        $('#cedula-representante-2').val("");
                        $('#select-sexo-2').val(0);
                        $("#select-sexo-2").selectpicker("refresh");
                        $('#nombre-representante-2').val("");
                        $('#apellido-1-representante-2').val("");
                        $('#apellido-2-representante-2').val("");
                        $('#correo-2').val("");

                        idEstudiante = 0
                        representante1_id = 0;
                        representante2_id = 0;
                    }
                    else {
                        idCurso = 0;

                        $("#select-estudiantes").find('option').remove().end().append('<option>El ' +
                            'curso no posee estudiantes</option>');
                        $("#select-estudiantes").selectpicker("refresh");

                        $('#cedula-representante-1').val("");
                        $('#select-sexo-1').val(0);
                        $("#select-sexo-1").selectpicker("refresh");
                        $('#nombre-representante-1').val("");
                        $('#apellido-1-representante-1').val("");
                        $('#apellido-2-representante-1').val("");
                        $('#correo-1').val("");

                        $('#cedula-representante-2').val("");
                        $('#select-sexo-2').val(0);
                        $("#select-sexo-2").selectpicker("refresh");
                        $('#nombre-representante-2').val("");
                        $('#apellido-1-representante-2').val("");
                        $('#apellido-2-representante-2').val("");
                        $('#correo-2').val("");

                        idEstudiante = 0
                        representante1_id = 0;
                        representante2_id = 0;
                    }
                }
            });
        }
        else {
            idCurso = 0;

            $("#select-estudiantes").find('option').remove().end().append('<option>El ' +
                'curso no posee estudiantes</option>');
            $("#select-estudiantes").selectpicker("refresh");

            $('#cedula-representante-1').val("");
            $('#select-sexo-1').val(0);
            $("#select-sexo-1").selectpicker("refresh");
            $('#nombre-representante-1').val("");
            $('#apellido-1-representante-1').val("");
            $('#apellido-2-representante-1').val("");
            $('#correo-1').val("");

            $('#cedula-representante-2').val("");
            $('#select-sexo-2').val(0);
            $("#select-sexo-2").selectpicker("refresh");
            $('#nombre-representante-2').val("");
            $('#apellido-1-representante-2').val("");
            $('#apellido-2-representante-2').val("");
            $('#correo-2').val("");

            idEstudiante = 0
            representante1_id = 0;
            representante2_id = 0;
        }
    });

    //Obteniendo los datos de los representantes
    $("#select-estudiantes").change(function () {
        idEstudiante = $(this).val();
        
        if (idEstudiante != null) {
            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerJsonInfoRepresentantes",
                data: {
                    idEstudiante: idEstudiante
                },
                success: function (data) {
                    poseeRepresentante_1 = data[0].poseeRepresentante_1;
                    poseeRepresentante_2 = data[0].poseeRepresentante_2;

                    if(data[0].success)
                    {
                        if (data[0].poseeRepresentantes) {
                            $('#cedula-representante-1').val(data[0].representante1_cedula);
                            $('#select-sexo-1').val(data[0].representante1_sexo);
                            $("#select-sexo-1").selectpicker("refresh");
                            $('#nombre-representante-1').val(data[0].representante1_nombre);
                            $('#apellido-1-representante-1').val(data[0].representante1_apellido1);
                            $('#apellido-2-representante-1').val(data[0].representante1_apellido2);
                            $('#correo-1').val(data[0].representante1_correo);

                            representante1_id = data[0].representante1_id;
                            
                            if (data[0].poseeRepresentante_2) {
                                $('#cedula-representante-2').val(data[0].representante2_cedula);
                                $('#select-sexo-2').val(data[0].representante2_sexo);
                                $("#select-sexo-2").selectpicker("refresh");
                                $('#nombre-representante-2').val(data[0].representante2_nombre);
                                $('#apellido-1-representante-2').val(data[0].representante2_apellido1);
                                $('#apellido-2-representante-2').val(data[0].representante2_apellido2);
                                $('#correo-2').val(data[0].representante2_correo);

                                representante2_id = data[1].representante2_id;
                            }
                            else {
                                $("#div-alerta-no-representante-2").show(400).delay(5000).slideUp(400);
                                representante2_id = 0;
                                
                                ClearTextBoxRepresentante_1();
                                ClearTextBoxRepresentante_2();
                            }
                        }
                        else {
                            $("#div-alerta-no-representantes").show(400).delay(5000).slideUp(400);
                            representante1_id = 0;
                            representante2_id = 0;

                            ClearTextBoxRepresentante_1();
                            ClearTextBoxRepresentante_2();
                        }
                    }
                    else {
                        $("#div-alerta-no-alumno").show(400).delay(5000).slideUp(400);
                        representante1_id = 0;
                        representante2_id = 0;
                    }
                }
            });
        }
        else {
            ClearTextBoxRepresentante_1();
            ClearTextBoxRepresentante_2();

            idEstudiante = 0
            representante1_id = 0;
            representante2_id = 0;
        }
    });
});