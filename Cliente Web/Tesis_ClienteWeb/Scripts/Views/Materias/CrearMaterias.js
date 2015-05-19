var idMateria;

$(document).ready(function () {
    //Acción para eliminar materias
    $('#table-lista-materias').on('click', 'td.td-eliminar', function (e) {
        var selectedId = $(this).attr('id');

        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar esta materia!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórrala!",
            cancelButtonText: "¡No, cancelala!",
            closeOnConfirm: true,
            closeOnCancel: false
        },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: "GET",
                    url: "/Materias/EliminarMateria",
                    data: {
                        id: selectedId
                    },
                    success: function (data) {
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
                                showProgress();
                                window.location.href = 'CrearMateria';
                            }
                        });
                    },
                    error: function (data) {
                        swal({
                            title: "¡Error!",
                            text: "Ha ocurrido un error eliminando la materia.",
                            type: "error",
                            showCancelButton: false,
                            closeOnConfirm: true,
                        },
                        function (isConfirm) {
                            if (isConfirm) {
                                showProgress();
                                window.location.href = 'CrearMateria';
                            }
                        });
                    }
                });
            }
            else
                swal("¡Cancelado!", "Su materia está a salvo :)", "error");
        });
    });

    $("#select-colegio-crear").change(function () {
        var lista = "";

        if ($(this).val() != "") {
            showProgress();

            $('#table-lista-materias').find('tbody').find('tr').remove();
            idColegio = $(this).val();

            $.post("/Bridge/ObtenerTablaMateriaPorIdColegio",
            {
                idColegio: idColegio
            },
            function (data) {
                if (data != null && data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        lista += ('<tr id=' + data[i].idMateria + ' >' +
                                    ' <td class="td-nombre">' + data[i].nombre + '</td>' +
                                    ' <td class="centrar td-codigo">' + data[i].codigo + '</td>' +
                                    ' <td class="centrar td-grado">' + data[i].grado + '</td>' +
                                    '<td class=td-eliminar id=' + data[i].idMateria + '>' +
                                        '<a class= "fa fa-ban" ></a>' +
                                    '</td>' +
                                  '</tr>');
                    }
                    $('#table-lista-materias').find('tbody').end().append(lista);

                    hideProgress();
                }
                else {
                    lista = ('<tr>' +
                                    '<td class="th-nombre"></td>' +
                                    '<td class="centrar th-codigo"></td></td>' +
                                    '<td class="centrar th-grado"></td>' +
                                    '<td class="th-eliminar"></td>' +
                            '</tr>');

                    $('#table-lista-materias').find('tbody').find('tr').end().append(lista);

                    swal("¡No hay materias!", "El colegio no tiene materias asociadas", "warning");
                    hideProgress();
                }
            });
        }
        else {
            lista = ('<tr>' +
                                    '<td class="th-nombre"></td>' +
                                    '<td class="centrar th-codigo"></td></td>' +
                                    '<td class="centrar th-grado"></td>' +
                                    '<td class="th-eliminar"></td>' +
                    '</tr>');

            $('#table-lista-materias').find('tbody').find('tr').end().append(lista);
                        
            hideProgress();
        }
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

    $('#btn-crear-materia-nueva').click(function () {
        showProgress();
    });
});