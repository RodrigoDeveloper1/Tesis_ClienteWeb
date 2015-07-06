function DialogoNuevaMateria() {
    $("#dialog-nueva-materia").dialog({
        draggable: false,
        dialogClass: "no-close",
        height: 350,
        hide: "explode",
        modal: true,
        resizable: false,
        show: "puff",
        title: "Nueva materia",
        width: 470,
        buttons: {
            "Cargar": function () {
                var name = $("#nombrenuevamateria").val();
                var nombrecurso = $("#select-curso-crear-materia option:selected").text();

                $.ajax({
                    url: "/Materias/CrearMateria",
                    type: "POST",
                    data: {
                        "Name": name,
                        "CursoName": nombrecurso
                    }                 
                });

                $(this).dialog("close");
            },
            "Cerrar": function () {
                $(this).dialog("close");
            }
        },
        close: function (r) {
            window.location.href = 'Materias';
        }
    });
}

var idMateria;

function ModificarMateria() {
    var name = $("#nombre-materia-modif").val();
    var codigo = $("#codigo-materia-modif").val();
    var pensum = $("#pensum-materia-modif").val();
    var idCurso = $("#select-curso-modif option:selected").val();    
    var idProfesor = $("#select-profesor-modif option:selected").val();
    var idColegio = $("#select-colegio-modif option:selected").val();

    $.ajax({
        url: "/Materias/ModificarMateria",
        type: "POST",
        data: {
            "Nombre": name,
            "Codigo": codigo,
            "Pensum": pensum,
            "idMateria": idMateria,
            "idCurso": idCurso,
            "idProfesor": idProfesor,
            "idColegio": idColegio

        }
    }).done(function () {
        window.location.href = 'ModificarMateria';
    });
}

$(document).ready(function () {
    $('#btnReload').click(function () {
        location.reload();
    });

    $(".eliminarmateria").click(function () {

        var selectedId = $(this).data("id");
       

            swal({
                title: "¿Estás Seguro?",
                text: "¡No serás capaz de recuperar esta materia!",
                type: "warning", showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "¡Si, bórrala!",
                cancelButtonText: "¡No, cancelala!",
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    swal("¡Borrado!", "Su materia ha sido borrada.", "success");
                    jQuery.get( 
                    "/Materias/EliminarMateria"
                    , { "id": selectedId }
                ).done(function (data) {

                    setTimeout("location.href ='/Materias/Materias';", 3000) /* 3 seconds */
                });
                } else {
                    swal("¡Cancelado!", "Su materia está a salvo :)", "error");
                }
            });


    });
    //Obtener tabla de alumnos 
    $("#select-curso").change(function () {
        var lista = "";
        console.log($(this).val());

        if ($(this).val() != "") {
            $('#table-lista-materias').find('tbody').find('tr').remove();
            idCurso = $(this).val();
            showProgress();

            $.post("/Bridge/ObtenerTablaMateriasPorIdCurso",
            {
                idCurso: idCurso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        lista +=
                            ('<tr>' +
                                '<td class="th-nombre">' + data[i].nombre + '</td>' +
                                '<td class="th-codigo">' + data[i].codigo + '</td>' +
                                '<td class="th-pensum">' + data[i].pensum + '</td>' +
                            '</tr>');
                    }

                    $('#table-lista-materias').find('tbody').end().append(lista);
                    hideProgress();
                }
                else {
                    $('#table-lista-materias').find('tbody').find('tr').remove();
                    hideProgress();
                }
            });
        }
        else {
            $('#table-lista-materias').find('tbody').find('tr').remove();
        }
    });

    $("#btn-nueva-materia").click(function () {
        console.log("Acción: click -> Botón Nueva Materia (#btn-nueva-materia)");      
        

        DialogoNuevaMateria();

    });

    $("#select-colegio-crear").change(function () {

        var lista = "";
        if ($(this).val() != "") {
            $('#table-lista-materias').find('tbody').find('tr').remove();
           
            idColegio = $(this).val();

            $.post("/Bridge/ObtenerTablaCrearMateriaPorIdColegio",
            {
                idColegio: idColegio
            },
            function (data) {
                if (data != null && data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        lista += ('<tr id=' + data[i].idMateria + ' >' +
                                    ' <td class="td-nombre">' + data[i].nombre + '</td>' +
                                    ' <td class="td-codigo">' + data[i].codigo + '</td>' +
                                    ' <td class="td-pensum">' + data[i].pensum + '</td>' +
                                    '<td class=td-eliminar id=' + data[i].idMateria + '>' +
                                        '<a class= "fa fa-ban" ></a>' +
                                    '</td>' +
                                  '</tr>');
                    }
                    $('#table-lista-materias').find('tbody').end().append(lista);
                }
                else {
                    lista = ('<tr>' +
                                    '<td class="th-nombre"></td>' +
                                    '<td class="th-codigo"></td></td>' +
                                    '<td class="th-pensum"></td>' +
                                    '<td class="th-eliminar"></td>' +
                            '</tr>');

                    $('#table-lista-materias').find('tbody').find('tr').end().append(lista);
                }
            });
        }
        else {
            lista = ('<tr>' +
                                    '<td class="th-nombre"></td>' +
                                    '<td class="th-codigo"></td></td>' +
                                    '<td class="th-pensum"></td>' +
                                    '<td class="th-eliminar"></td>' +
                    '</tr>');

            $('#table-lista-materias').find('tbody').find('tr').end().append(lista);

        }
    });

    $("#select-colegio-modif").change(function () {

        if ($(this).val() != "") {
            /* Configuración para select - Fuente del código de configuración del selectpicker: 
             * http://stackoverflow.com/questions/23514318/bootstrap-select-reinitialize-on-dynamically-added-element
             */

            $("#select-curso-modif").find('option').remove().end().append("<option>Cargando cursos...</option>");
            $("#select-curso-modif").selectpicker("refresh");

            idColegio = $(this).val();

            $.post("/Bridge/ObtenerSelectListCursos",
            {
                idColegio: idColegio
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione el curso...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idCurso + '">' + data[i].nombre + '</option>');
                    }

                    $("#select-curso-modif").find('option').remove().end().append(lista);
                    $("#select-curso-modif").selectpicker("refresh");
                }
                else {
                    $("#select-curso-modif").find('option').remove().end().append('<option>No se encontraron cursos activos....</option>');
                    $("#select-curso-modif").selectpicker("refresh");
                }
            })
        }
        else {
            $('#select-curso-modif').find('option').remove().end().append('<option>Seleccione el curso...</option>');
            $("#select-curso-modif").selectpicker("refresh");
        }
    });

    $("#select-curso-modif").change(function () {

        if ($(this).val() != "") {
            /* Configuración para select - Fuente del código de configuración del selectpicker: 
             * http://stackoverflow.com/questions/23514318/bootstrap-select-reinitialize-on-dynamically-added-element
             */

            $("#select-profesor-modif").find('option').remove().end().append("<option>Cargando profesores...</option>");
            $("#select-profesor-modif").selectpicker("refresh");

            idCurso = $(this).val();

            $.post("/Bridge/ObtenerSelectListProfesores",
            {
                idCurso: idCurso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione el profesor...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idProfesor + '">' + data[i].nombre + '</option>');

                    }

                    $("#select-profesor-modif").find('option').remove().end().append(lista);
                    $("#select-profesor-modif").selectpicker("refresh");
                }
                else {
                    $("#select-profesor-modif").find('option').remove().end().append('<option>No se encontraron profesores activas....</option>');
                    $("#select-profesor-modif").selectpicker("refresh");
                }
            });
        }
        else {
            $('#select-profesor-modif').find('option').remove().end().append('<option>Seleccione el profesor...</option>');
            $("#select-profesor-modif").selectpicker("refresh");
        }
    });

    $("#select-profesor-modif").change(function () {
        var lista = "";
        console.log("Entro wawais 1");
        if ($(this).val() != "") {
            console.log("Entro wawais 2");
            $('#table-lista-materias-modif').find('tbody').find('tr').remove();
            idCurso = $('#select-curso-modif').val();
            idProfesor = $(this).val();

            $.post("/Bridge/ObtenerTablaCrearMateriaPorIdCurso",
            {
                idCurso: idCurso,
                idProfesor: idProfesor
            },
            function (data) {
                if (data != null && data.length > 0) {
                    console.log("Entro wawais 3");
                    for (var i = 0; i < data.length; i++) {
                        lista += ('<tr id=' + data[i].idMateria + ' >' +
                                    ' <td class="td-nombre">' + data[i].nombre + '</td>' +
                                    ' <td class="td-codigo">' + data[i].codigo + '</td>' +
                                    ' <td class="td-pensum">' + data[i].pensum + '</td>' +                                    
                                  '</tr>');
                    }
                    $('#table-lista-materias-modif').find('tbody').end().append(lista);
                }
                else {
                    console.log("Entro wawais 4");
                    lista = ('<tr>' +
                                    '<td class="th-nombre"></td>' +
                                    '<td class="th-codigo"></td></td>' +
                                    '<td class="th-pensum"></td>' +
                            '</tr>');

                    $('#table-lista-materias-modif').find('tbody').find('tr').end().append(lista);
                }
            });
        }
        else {
            console.log("Entro wawais 5");
            lista = ('<tr>' +
                                    '<td class="th-nombre"></td>' +
                                    '<td class="th-codigo"></td></td>' +
                                    '<td class="th-pensum"></td>' +
                    '</tr>');

            $('#table-lista-materias-modif').find('tbody').find('tr').end().append(lista);

        }
    });

    $('#table-lista-materias').on('click', 'td.td-eliminar', function (e) {
        
        var selectedId = $(this).attr('id');
        console.log(selectedId);

        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar esta materia!",
            type: "warning", showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórrala!",
            cancelButtonText: "¡No, cancelala!",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                console.log(selectedId);
                swal("¡Borrado!", "Su materia ha sido borrada.", "success");
                jQuery.get(
                "/Materias/EliminarMateria"
                , { "id": selectedId }
            ).done(function (data) {
                swal({
                    title: "¡Eliminada!",
                    text: "La materia acaba de eliminarse.",
                    type: "success",
                    confirmButtonColor: "green",
                    showCancelButton: false,
                    closeOnConfirm: true,
                },
                function (isConfirm) {
                    if (isConfirm) {
                        var lista = "";

                        $('#table-lista-materias').find('tbody').find('tr').remove();
                        idCurso = $('#select-curso-crear').val();
                        idProfesor = $('#select-profesor-crear').val();

                        $.post("/Bridge/ObtenerTablaCrearMateriaPorIdCurso",
                        {
                            idCurso: idCurso,
                            idProfesor: idProfesor
                        },
                        function (data) {
                            console.log("Entro wawa 1");
                            if (data != null && data.length > 0) {
                                console.log("Entro wawa 2");
                                for (var i = 0; i < data.length; i++) {
                                    lista += ('<tr id=' + data[i].idevaluacion + ' >' +
                                     ' <td class="td-nombre">' + data[i].nombre + '</td>' +
                                     ' <td class="td-codigo">' + data[i].codigo + '</td>' +
                                     ' <td class="td-pensum">' + data[i].pensum + '</td>' +
                                     '<td class=td-eliminar id=' + data[i].idMateria + '>' +
                                         '<a class= "fa fa-ban" ></a>' +
                                     '</td>' +
                                   '</tr>');
                                }
                                $('#table-lista-materias').find('tbody').end().append(lista);
                            }
                            else {
                                console.log("Entro wawa 3");
                                lista = ('<tr>' +
                                                '<td class="th-nombre"></td>' +
                                                '<td class="th-codigo"></td></td>' +
                                                '<td class="th-pensum"></td>' +
                                                '<td class="th-eliminar"></td>' +
                                        '</tr>');

                                $('#table-lista-materias').find('tbody').find('tr').end().append(lista);
                            }
                        });
                    }
                });

            });
            } else {
                swal("¡Cancelado!", "Su materia está a salvo :)", "error");
            }
        });


    });

    $('#table-lista-materias-modif').on('click', 'tr', function (e) {

        var state = $(this).hasClass('active');
        $('.active').removeClass('active');

        if (!state) {
            $(this).addClass('active');
        }


        var nombre = "";
        var codigo = "";
        var pensum = "";
        idMateria = $(this).attr('id');

        if ($(this).attr('id') != "") {

            $('#nombre-materia-modif-div').find("input").remove();
            $('#codigo-materia-modif-div').find("input").remove();
            $('#pensum-materia-modif-div').find("textarea").remove();

            $.post("/Bridge/ObtenerDatosModificarMateria",
            {
                idMateria: idMateria

            },
            function (data) {
                if (data != null && data.length > 0) {

                    nombre += ('<input id ="nombre-materia-modif" class="form-control"' +
                    ' placeholder ="Nombre de la materia" value="' + data[0].nombre + '">')

                    codigo += ('<input id ="codigo-materia-modif" class="form-control"' +
                    ' placeholder =" Código de la materia" value="' + data[0].codigo + '">')

                    pensum += ('<textarea rows="4" cols="50" class="form-control"  id="pensum-materia-modif"'+ 
                    'placeholder="Pensum de la materia value="> ' + data[0].pensum + '</textarea>  ')

                    $('#nombre-materia-modif-div').find("input").end().append(nombre);
                    $('#codigo-materia-modif-div').find("input").end().append(codigo);
                    $('#pensum-materia-modif-div').find("textarea").end().append(pensum);

                }
                else {
                }
            });
        }
        else {
        }

    });

    $("#btn-modificar-materia").click(function () {
        console.log("Acción: click -> Botón modificar materia");

        ModificarMateria();

    });
});