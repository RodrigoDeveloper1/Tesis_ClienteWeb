var idMateria = "";
var idColegio;

function ModificarMateria() {
    var name = $("#nombre-materia-modif").val();
    var codigo = $("#codigo-materia-modif").val();
    var pensum = $("#pensum-materia-modif").val();
    var idCurso = $("#select-curso-modif option:selected").val();    
    var idProfesor = $("#select-profesor-modif option:selected").val();
    var idColegio = $("#select-colegio-modif option:selected").val();

    showProgress();

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
        },
        success: function () {
            window.location.href = 'ModificarMateria';
        },
        error: function () {
            swal({
                title: "¡Error!",
                text: "Ha ocurrido un error mientras se modificaba la materia.",
                type: "error",
                closeOnConfirm: true,
            },
            function (isConfirm) {
                if (isConfirm) {
                    showProgress();
                    window.location.href = 'ModificarMateria';
                }
            });
        }
    });
}

$(document).ready(function () {
    $("#select-colegio-modif").change(function () {
        var lista = "";
        if ($(this).val() != "") {
            $('#table-lista-materias-modif').find('tbody').find('tr').remove();
           
            idColegio = $(this).val();
            
            showProgress();
            $.post("/Bridge/ObtenerTablaMateriaPorIdColegio",
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
                                  '</tr>');
                    }
                    $('#table-lista-materias-modif').find('tbody').end().append(lista);
                    hideProgress();
                }
                else {
                    lista = ('<tr>' +
                                    '<td class="th-nombre"></td>' +
                                    '<td class="th-codigo"></td></td>' +
                                    '<td class="th-pensum"></td>' +
                            '</tr>');

                    $('#table-lista-materias-modif').find('tbody').find('tr').end().append(lista);

                    hideProgress();
                }
            });
        }
        else {
            lista = ('<tr>' +
                                    '<td class="th-nombre"></td>' +
                                    '<td class="th-codigo"></td></td>' +
                                    '<td class="th-pensum"></td>' +
                    '</tr>');

            $('#table-lista-materias-modif').find('tbody').find('tr').end().append(lista);
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
            });
        }
        else
            idMateria = "" //Reiniciando la variable.
    });

    $("#btn-modificar-materia").click(function () {
        if (idMateria == "" || idMateria == null)
            swal("¡Oops!", "Seleccione una materia para modificarla", "warning");
        else
            ModificarMateria();
    });
});