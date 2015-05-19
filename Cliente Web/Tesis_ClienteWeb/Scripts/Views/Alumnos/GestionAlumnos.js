var idColegio;
var idAnoEscolarActivo;
var idCurso;

$(document).ready(function () {
    //Obtener el año escolar y la lista de cursos de ese año escolar
    $("#select-colegio").change(function () {
        idColegio = $(this).val();

        $("#ano-escolar").val("Cargando el año escolar");
        $("#select-curso").find('option').remove().end().append('<option>Cargando la lista de cursos...</option>');
        $("#select-curso").selectpicker("refresh");

        //Se borra todas las filas de la tabla
        $("#div-tabla-lista-alumnos-cargar tbody tr").remove();

        if (idColegio != "") {
            showProgress();

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

                                    hideProgress();
                                }
                                else {
                                    idCurso = 0;

                                    //Limpiando el select-list del curso
                                    $("#select-curso").find('option').remove().end().append('<option>El ' +
                                        'colegio no posee cursos</option>');
                                    $("#select-curso").selectpicker("refresh");
                                    //Limpiando la tabla de alumnos
                                    $("#div-tabla-lista-alumnos-cargar tbody tr").remove();

                                    hideProgress();
                                }                                                                
                            }
                        });
                    }
                    else {
                        idCurso = 0;

                        //Limpiando el text-box del año escolar
                        $("#ano-escolar").val("No posee año escolar activo");
                        //Limpiando el select-list del curso
                        $("#select-curso").find('option').remove().end().append('<option>Seleccione ' +
                            'el çurso...</option>');
                        $("#select-curso").selectpicker("refresh");
                        //Limpiando la tabla de alumnos
                        $("#div-tabla-lista-alumnos-cargar tbody tr").remove();

                        hideProgress();
                    }
                }
            });
        }
        else {
            idCurso = 0;
            //Limpiando el text-box del año escolar
            $("#ano-escolar").val("");
            //Limpiando el select-list del curso
            $("#select-curso").find('option').remove().end().append('<option>Seleccione el curso...</option>');
            $("#select-curso").selectpicker("refresh");
            //Limpiando la tabla de alumnos
            $("#div-tabla-lista-alumnos-cargar tbody tr").remove();

            hideProgress();
        }
    });

    //Obteniendo el id del curso
    $("#select-curso").change(function () {
        idCurso = $(this).val();

        if (idCurso != "") {
            showProgress();

            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerJsonEstudiantesYRepresentantesPorCurso",
                data: {
                    idCurso: idCurso
                },
                success: function (data) {
                    if (data[0].success) {
                        var lista = "";
                        
                        for (var i = 0; i < data.length; i++) {
                            lista += (
                                "<tr>" +
                                    "<td class='td-nro-lista'>" + data[i].nroLista + "</td>" +
                                    "<td class='td-apellido'>" + data[i].apellido + "</td>" +
                                    "<td class='td-nombre'>" + data[i].nombre + "</td>" +
                                    "<td class='td-representante1'>" + data[i].representante1 + "</td>" +
                                    "<td class='td-representante2'>" + data[i].representante2 + "</td>" +
                                    "<td class='td-editar-alumno centrar'>" +
                                        "<a class='fa fa-pencil' href='EditarAlumno/" + data[i].idEstudiante + "'></a>" +
                                    "</td>" +
                                    "<td class='td-eliminar centrar' id='" + data[i].idEstudiante + "'>" +
                                        "<a class='fa fa-minus-circle eliminaralumno'></a>" +
                                    "</td>" +
                                "</tr>"
                                );
                        }

                        $("#div-tabla-lista-alumnos-cargar tbody tr").remove();
                        $("#div-tabla-lista-alumnos-cargar tbody").append(lista);

                        hideProgress();
                    }
                    else {
                        hideProgress();

                        swal("¡No hay alumnos asociados!");

                        //Se borra todas las filas de la tabla
                        $("#div-tabla-lista-alumnos-cargar tbody tr").remove();
                    }
                }
            });
        }
        else {
            hideProgress();

            //Se borra todas las filas de la tabla
            $("#div-tabla-lista-alumnos-cargar tbody tr").remove(); 
        }
    });

    //Eliminar alumno
    $('#table-lista-alumnos').on('click', 'td.td-eliminar', function () {
        var selectedId = $(this).attr('id');

        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar este alumno!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórralo!",
            cancelButtonText: "¡No, cancelalo!",
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                showProgress();

                $.ajax({
                    type: "POST",
                    url: "/Alumnos/EliminarAlumno",
                    data: {
                        id: selectedId
                    },
                    success: function (data) {
                        if (data[0].success)
                        {
                            swal({
                                title: "¡Eliminado!",
                                text: "Adios alumno mío. ='(",
                                type: "success",
                                confirmButtonColor: "green",
                                showCancelButton: false,
                                closeOnConfirm: true,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'GestionAlumnos';
                            });
                        }
                        else 
                        {
                            swal({
                                title: "¡Error!",
                                text: "Ha ocurrido un error y no se ha podido eliminar el alumno",
                                type: "danger",
                                confirmButtonColor: "red",
                                showCancelButton: false,
                                closeOnConfirm: true,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'GestionAlumnos';
                            });
                        }
                    }
                });
            }
        });
    });

    $('#table-lista-alumnos').on('click', 'td.td-editar-alumno', function () {
        showProgress();
    });
});