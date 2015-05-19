var idColegio = "";
var idAnoEscolarActivo = "";
var idCurso = ""; 

function AgregarAlumnos()
{
    if(idColegio == "" || idAnoEscolarActivo == "" || idCurso == "")
        swal("¡Oops!", "Recuerde seleccionar un colegio y/o curso.", "warning");
    else
    {
        showProgress();

        SalvarTodo(); //Desde el archivo GestionTablasAlumnos.js
        $('#div-table-lista-alumnos-cargar tr:last').remove(); //Para borrar la última fila con el signo '+'

        var nroLista;
        var matricula;
        var nombre1;
        var nombre2;
        var apellido1;
        var apellido2;
        var listaEstudiantes = [];
        var estudiante;

        listaEstudiantes.push(idCurso);

        $('#div-table-lista-alumnos-cargar tbody tr').each(function () {
            nroLista = $(this).children("td:nth-child(1)").html();
            matricula = $(this).children("td:nth-child(2)").html();
            nombre1 = $(this).children("td:nth-child(3)").html();
            nombre2 = $(this).children("td:nth-child(4)").html();
            apellido1 = $(this).children("td:nth-child(5)").html();
            apellido2 = $(this).children("td:nth-child(6)").html();

            estudiante = [nroLista, matricula, nombre1, nombre2, apellido1, apellido2];

            listaEstudiantes.push(estudiante);
        });

        var postData = { values: listaEstudiantes };

        $.ajax({
            type: "POST",
            url: "/Alumnos/AgregarAlumno",
            traditional: true,
            data: postData,
            success: function (r) {
                window.location.href = 'AgregarAlumno';
            }
        });
    }
}

$(document).ready(function () {
    //Obtener el año escolar y la lista de cursos de ese año escolar
    $("#select-colegio").change(function () {
        idColegio = $(this).val();

        showProgress();

        $("#ano-escolar").val("Cargando el año escolar");
        $("#select-curso").find('option').remove().end().append('<option>Cargando la lista de cursos...</option>');
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
                            url: "/Bridge/ObtenerListaNombresDeCursosPorAnoEscolarSinEstudiantes",
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

                                    hideProgress();
                                }
                                else {
                                    idCurso = 0;
                                    $("#select-curso").find('option').remove().end().append('<option>El ' +
                                        'colegio no posee cursos</option>');
                                    $("#select-curso").selectpicker("refresh");

                                    hideProgress();
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

                        hideProgress();
                    }
                }
            });
        }
        else {
            idCurso = 0;
            $("#ano-escolar").val("");
            $("#select-curso").find('option').remove().end().append('<option>Seleccione el curso...</option>');
            $("#select-curso").selectpicker("refresh");

            hideProgress();
        }
    });

    //Obteniendo el id del curso
    $("#select-curso").change(function () {
        idCurso = $(this).val();

        if (idCurso != null) {
            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerJsonEstudiantesPorCurso",
                data: {
                    idCurso: idCurso
                },
                success: function (data) {
                    if (data[0].success) {
                        BorrarTodo(); //Ubicado en GestionTablasAlumnos.js
                        AgregadoAutomatico(data); //Ubicado en GestionTablasAlumnos.js
                    }
                    else {
                        BorrarTodo(); //Ubicado en GestionTablasAlumnos.js
                    }
                }
            });
        }
        else {
            BorrarTodo(); //Ubicado en GestionTablasAlumnos.js
        }
    });

    $("#btn-agregar-alumno").bind("click", AgregarAlumnos)
});